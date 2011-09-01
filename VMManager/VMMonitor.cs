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

        private int monitorInterval ;//= 1; // specified in MINUTES
        private int expiryTime; //= 2;
        private int defaultMonitorInterval = 1;
        private int defaultExpiryTime = 2;

        //string regMonDir = "HKEY_LOCAL_MACHINE\\SOFTWARE\\RetherNetworksInc\\DOFS-SandBox";
        string valid = "valid";
        string interval = "checkInterval";

        //string curVMRegDir = @"SOFTWARE\RetherNetworksInc\DOFS-SandBox\CurrentVMSet";
        string numVM = "numVM";
        string regDir = @"SOFTWARE\RetherNetworksInc\DOFS-SandBox";
        //string vmFile = "VMFile";
        string vmTimestamp = "VMTimestamp";
        string vmUsername = "VMUsername";
        string vmPassword = "VMPassword";

        public VMMonitor(VMActivityQueue q)
        {
            getInfo();
            vmMonitorTimer = new Timer(1000 * 60 * monitorInterval);
            queue = q;
            checkVM();
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
        public void getInfo()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(regDir);
            if (key != null)
            {
                if (key.GetValue(valid) != null)
                {
                    monitorInterval = Convert.ToInt32(key.GetValue(interval).ToString());
                }
                else
                {
                    monitorInterval = defaultMonitorInterval;
                }
               if(key.GetValue(interval) != null)
               {
                   expiryTime = Convert.ToInt32(key.GetValue(valid).ToString());
                }
               else{
               expiryTime=defaultExpiryTime;
               }
            }
        }

        private void checkVM()
        {

            Debug.WriteLine("Timer fired");
            while (queue.isEmpty() == false)
            {
                Debug.WriteLine("Queue not empty");

                vmEntry entry = (vmEntry)queue.Peek();
                DateTime ts = DateTime.Now.AddMinutes(-expiryTime);

                Debug.WriteLine("Time stamps " + ts.ToString() + " and " + entry.timestamp.ToString());

                if (ts.CompareTo(entry.timestamp) >= 0)
                {
                    Debug.WriteLine("Dequeuing entry");
                    try
                    {
                        queue.Dequeue();

                        /* Now restore the VM to a snapshot */
                        string vmrun = "C:\\Program Files\\VMware\\VMware Workstation\\vmrun.exe";
                        string command = " revertToSnapshot " + entry.vmFile + " " + entry.snapshotName;
                        Debug.WriteLine("command " + command);
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
                        command = " start " + entry.vmFile;
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
        private void RestoreVM(object sender, ElapsedEventArgs e)
        {
            checkVM();
        }

        private void DeleteEntryFromReg(vmEntry entry)
        {
            try
            {
                Debug.WriteLine("Opening Registry");
                RegistryKey key = Registry.LocalMachine.CreateSubKey(regDir);
                
  
                string tempVMTimestamp,tempUsername,tempPassword;

                if (key != null)
                {
                    if (key.GetValue(numVM) != null)
                    {
                       

                   
                        tempVMTimestamp = vmTimestamp + entry.indexNum;
                        tempUsername=vmUsername+entry.indexNum;
                        tempPassword=vmPassword+entry.indexNum;

                        key.DeleteValue(tempVMTimestamp);
                        key.DeleteValue(tempUsername);
                        key.DeleteValue(tempPassword);
                       
                    }
                    key.Close();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception while deleting entry from Reg " + e.Message);
            }
        }

        public void regChanged()
        {
            Debug.WriteLine("vmMonitor regChanged");
            getInfo();
            vmMonitorTimer.Interval=(1000 * 60 * monitorInterval);
        }
    }
}
