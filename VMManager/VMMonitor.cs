using System;
using System.Timers;
using System.Diagnostics;
using Microsoft.Win32;

namespace VMManager
{
    public class VMMonitor
    {
        Timer vmMonitorTimer;
        VMActivityQueue queue;

        private int MONITOR_INTERVAL = 1; // specified in MINUTES
        private int EXPIRY_TIME = 2;

        string curVMRegDir = @"SOFTWARE\RetherNetworksInc\DOFS-SandBox\CurrentVMSet";
        string numVM = "numVM";

        string vmPath = "VMPath";
        string vmTimestamp = "VMTimestamp";

        public VMMonitor(VMActivityQueue q)
        {
            vmMonitorTimer = new Timer(1000 * 60 * MONITOR_INTERVAL);
            queue = q;
        }

        public void Start()
        {
            vmMonitorTimer.Elapsed += new ElapsedEventHandler(RestoreVM);
            vmMonitorTimer.Start();
            Debug.WriteLine("Starting Timer ");
        }

        public void Stop()
        {
            vmMonitorTimer.Stop();
        }

        private void RestoreVM(object sender, ElapsedEventArgs e)
        {
            Debug.WriteLine("TImer fired");
            while (queue.isEmpty() == false)
            {
                Debug.WriteLine("Queue not empty");

                vmEntry entry = (vmEntry)queue.Peek();
                DateTime ts = DateTime.Now.AddMinutes(-EXPIRY_TIME);

                Debug.WriteLine("Time stamps " + ts.ToString() + " and " + entry.timestamp.ToString());

                if (ts.CompareTo(entry.timestamp) >= 0)
                {
                    Debug.WriteLine("Dequeuing entry");
                    try
                    {
                        queue.Dequeue();

                        /* Now restore the VM to a snapshot */
                        string vmrun = "C:\\Program Files\\VMware\\VMware Workstation\\vmrun.exe";
                        string command = " revertToSnapshot " + entry.vmPath + " " + entry.snapshotName;
//                        string command = " stop " + entry.vmPath;
                        
                        System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo(vmrun, command);
                        procStartInfo.UseShellExecute = false;
                        procStartInfo.RedirectStandardOutput = true;
                        Debug.WriteLine("Dequeue3");

                        // Now we create a process, assign its ProcessStartInfo and start it
                        System.Diagnostics.Process proc = new System.Diagnostics.Process();

                        proc.StartInfo = procStartInfo;
                        proc.Start();
                        string result = proc.StandardOutput.ReadToEnd();

                        proc.WaitForExit();
                        // Display the command output.
                        Debug.WriteLine("Result " + result);


                        Debug.WriteLine("Dequeue4");
                        Debug.WriteLine("Dequeue1");


                        System.Threading.Thread.Sleep(5000);
                        command = " start " + entry.vmPath;
                        procStartInfo = new System.Diagnostics.ProcessStartInfo(vmrun, command);

                        procStartInfo.UseShellExecute = false;
                        // Do not create the black window.
                        procStartInfo.CreateNoWindow = false;
                        procStartInfo.RedirectStandardOutput = true;

                        proc.StartInfo = procStartInfo;
                        proc.Start();
                        result = proc.StandardOutput.ReadToEnd();
                        proc.WaitForExit();

                        Debug.WriteLine("Result2 " + result);

                        Debug.WriteLine("Dequeue2");
                        entry.busy = false;
                        DeleteEntryFromReg(entry);
                    }
                    catch (Exception exc)
                    {
                        Debug.WriteLine("Exception while stopping VM " + exc.Message);
                    }
                }
                else
                    break;
            }
        }

        private void DeleteEntryFromReg(vmEntry entry)
        {
            try
            {
                Debug.WriteLine("Opening Registry");
                RegistryKey key = Registry.LocalMachine.CreateSubKey(curVMRegDir);
                
                int vmNumber;
                string tempVMPath, tempVMTimestamp;

                if (key != null)
                {
                    if (key.GetValue(numVM) != null)
                    {
                        vmNumber = Convert.ToInt32(key.GetValue(numVM).ToString());
                        vmNumber--;

                        tempVMPath = vmPath + entry.indexNum;
                        tempVMTimestamp = vmTimestamp + entry.indexNum;

                        key.SetValue(numVM, vmNumber);
                        key.DeleteValue(tempVMPath);
                        key.DeleteValue(tempVMTimestamp);
                    }
                    key.Close();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception while deleting entry from Reg " + e.Message);
            }
        }
    }
}
