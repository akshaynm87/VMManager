using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;

using System.Text;
using System.Windows.Forms;

namespace VmManagerConfiguration
{
    public partial class VMManagerConfigUI : Form
    {
        List<vmEntry> vmList;
        vmEntry currentEntry;

        int CLIENT_REQ_LEN = 1;
        int CLIENT_PORT_NUM = 8400;
        int MAX_CLIENT_CONN = 1000;

        private byte CREATE_NEW_USER = 1;
        private byte RESP_LEN_OFFSET = 4;
        string regDir = @"SOFTWARE\RetherNetworksInc\DOFS-SandBox";
        string regMonDir = "HKEY_LOCAL_MACHINE\\SOFTWARE\\RetherNetworksInc\\DOFS-SandBox";
        string valid = "valid";
        string interval = "checkInterval";
        string numVM = "numVM";
        string vmName = "VMName";
        string vmFile = "VMFile";
        string vmSnapshot = "VMSnapshot";


        string vmTimestamp = "VMTimestamp";
        string vmUsername = "VMUsername";
        string vmPassword = "VMPassword";
        public Boolean ownWrite { get; set; }
        public int shouldSelect { get; set; }


        int numVMs, currentIndex, numInterval, numValid;

        public VMManagerConfigUI()
        {
            InitializeComponent();

            ownWrite = false;
            GetVMInfo();
            updateList();
            regMon();


        }

        private void GetVMInfo()
        {
            try
            {
                vmList = new List<vmEntry>();

                Debug.WriteLine("Opening Registry");
                RegistryKey key = Registry.LocalMachine.OpenSubKey(regDir);
                string tempVMName, tempVMFile, tempVMSnapshot, tempVMUsername, tempVMPassword, tempVMTimestamp, fileConv;

                if (key != null)
                {
                    Debug.WriteLine("Successfully opened Registry");
                    if (key.GetValue(numVM) != null)
                    {
                        numVMs = Convert.ToInt32(key.GetValue(numVM).ToString());
                        if (key.GetValue(interval) != null)
                        {
                            numInterval = Convert.ToInt32(key.GetValue(interval).ToString());
                        }
                        if (key.GetValue(valid) != null)
                        {
                            numValid = Convert.ToInt32(key.GetValue(valid).ToString());
                        }
                        for (int i = 1; i <= numVMs; i++)
                        {
                            tempVMName = vmName + i;
                            tempVMFile = vmFile + i;
                            tempVMSnapshot = vmSnapshot + i;
                            tempVMUsername = vmUsername + i;
                            tempVMPassword = vmPassword + i;
                            tempVMTimestamp = vmTimestamp + i;

                            Debug.WriteLine("Temp VM String " + tempVMName + " " + tempVMFile + " " + tempVMSnapshot);

                            vmEntry entry = new vmEntry();
                            entry.indexNum = i;
                            if (key.GetValue(tempVMName) != null)
                            {
                                entry.vmName = key.GetValue(tempVMName).ToString();
                            }
                            if (key.GetValue(tempVMFile) != null)
                            {
                                fileConv = key.GetValue(tempVMFile).ToString();
                                fileConv = fileConv.Substring(1, fileConv.Length - 2);
                                entry.vmFile = Path.GetFullPath(fileConv);
                            }
                            if (key.GetValue(tempVMSnapshot) != null)
                            {
                                entry.snapshotName = key.GetValue(tempVMSnapshot).ToString();
                            }

                            if (key.GetValue(tempVMUsername) != null)
                            {
                                entry.username = key.GetValue(tempVMUsername).ToString();
                                entry.password = key.GetValue(tempVMPassword).ToString();
                                entry.timestamp = Convert.ToDateTime(key.GetValue(tempVMTimestamp).ToString());
                                entry.busy = true;
                            }
                            else
                            {

                                entry.busy = false;
                            }

                            vmList.Add(entry);
                        }
                    }
                    key.Close();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception while reading from reg " + e.Message);
            
            }

        }

        
        private void listBoxVMList_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentIndex = listBoxVMList.SelectedIndex;
            groupBoxDetail.Enabled = true;
            try
            {
                string curItem = listBoxVMList.SelectedItem.ToString();

                foreach (vmEntry entry in vmList)
                {
                    if (curItem.Equals(entry.vmName))
                    {
                        textBoxVMFile.Text = entry.vmFile;
                        textBoxVMSnapshotName.Text = entry.snapshotName;
                        textBoxIP.Text = entry.vmName;
                        if (entry.busy)
                        {
                            textBoxIP.ReadOnly = true;
                            labelSetActive.Text = "Yes";
                            textBoxVMFile.ReadOnly = true;
                            buttonVMFile.Enabled = false;
                            textBoxVMSnapshotName.ReadOnly = true;
                            textBoxTimestamp.Text = entry.timestamp.ToString();
                            textBoxUsername.Text = entry.username;
                            textBoxPassword.Text = entry.password;
                            buttonCreateUser.Enabled = false;
                            buttonDeleteEntry.Enabled = false;
                        }
                        else
                        {
                            textBoxIP.ReadOnly = false;
                            labelSetActive.Text = "No";
                            textBoxVMFile.ReadOnly = false;
                            textBoxVMSnapshotName.ReadOnly = false;
                            buttonVMFile.Enabled = true;
                            textBoxTimestamp.Text = "";
                            textBoxUsername.Text = "";
                            textBoxPassword.Text = "";
                            buttonCreateUser.Enabled = true;
                            buttonDeleteEntry.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
        }

        private void buttonVMFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                openFileDialog.InitialDirectory = "c:\\";
                textBoxVMFile.Text = openFileDialog.FileName;

            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonAddEntry_Click(object sender, EventArgs e)
        {

            AddEntry newEntry = new AddEntry(vmList, this);
            newEntry.Visible = true;
            //listBoxVMList.SelectedIndex = listBoxVMList.Items.Count;

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            ownWrite = true;
            int index = listBoxVMList.SelectedIndex;
            if (cleanReg())//&&writeReg())
            {
                writeReg();
                GetVMInfo();
                updateList();
                listBoxVMList.SelectedIndex = index;
                MessageBox.Show("Information succesfully stored", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Ann error occured while trying to save the settings.", "Error while trying to save settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ownWrite = false;
        }
        private void sortVMList()
        {
            List<vmEntry> list = new List<vmEntry>();

            foreach (vmEntry entry in vmList)
            {
                if (entry.busy)
                {
                    list.Add(entry);
                }
            }
            foreach (vmEntry entry in vmList)
            {
                if (!entry.busy)
                {
                    list.Add(entry);
                }
            }


        }
        public void updateList()
        {
            textBoxInterval.Text = numInterval.ToString();
            textBoxValid.Text = numValid.ToString();
            listBoxVMList.Items.Clear();

            foreach (vmEntry entry in vmList)
            {
                listBoxVMList.Items.Add(entry.vmName);


            }
            if (listBoxVMList.Items.Count >= 1)
            {
             
                listBoxVMList.SelectedIndex = shouldSelect;
                listBoxVMList.SetSelected(shouldSelect, true);
               
            }


        }
        private Boolean cleanReg()
        {
            string tempVMFile, tempVMTimestamp, tempVMUsername, tempVMPassword, tempVMSnapshotName, tempVMName;
            int vmNumber;
            try
            {
             
                RegistryKey key = Registry.LocalMachine.CreateSubKey(regDir);

                
                if (key != null)
                {
                    if (key.GetValue(numVM) != null)
                    {
                        //code if key Not Exist
                        vmNumber = Convert.ToInt32(key.GetValue(numVM).ToString());
                        if (vmNumber > 0)
                        {
                            for (int i = 1; i <= vmNumber; i++)
                            {

                                tempVMFile = vmFile + i;
                                tempVMSnapshotName = vmSnapshot + i;
                                tempVMName = vmName + i;
                                tempVMUsername = vmUsername + i;
                                tempVMPassword = vmPassword + i;
                                tempVMTimestamp = vmTimestamp + i;
                                
                                key.DeleteValue(tempVMFile);
                                key.DeleteValue(tempVMSnapshotName);
                                key.DeleteValue(tempVMName);

                                if (key.GetValue(tempVMUsername) != null)
                                {
                                    key.DeleteValue(tempVMTimestamp);
                                    key.DeleteValue(tempVMUsername);
                                    key.DeleteValue(tempVMPassword);

                                }
                            }
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
                return true;
            }
            catch (Exception e)
            {

                Debug.WriteLine("Exeption " + e.Message);
                return false;
            }

        }
        private Boolean writeReg()
        {

           string[] tempVMFileArray;
           string fileNameJoined;
           int vmNumber,vmInterval,vmValid;
       
            string tempVMFile, tempVMTimestamp, tempVMUsername, tempVMPassword, tempVMSnapshotName, tempVMName;
            try
                {

                   RegistryKey key = Registry.LocalMachine.CreateSubKey(regDir);
                if (key != null)
                {
                    vmNumber = vmList.Count;
                    vmInterval = Convert.ToInt32(textBoxInterval.Text);
                    vmValid= Convert.ToInt32(textBoxValid.Text);

                    key.SetValue(numVM, vmNumber);
                    key.SetValue(interval, vmInterval);
                    key.SetValue(valid, vmValid);
                    int i = 1;
                    foreach(vmEntry entry in vmList){
                 
                        tempVMFileArray = entry.vmFile.Split('\\');
                        fileNameJoined = String.Join("\\\\", tempVMFileArray, 0, tempVMFileArray.Length);
                        fileNameJoined = "\"" + fileNameJoined + "\"";

                            tempVMFile = vmFile + i;
                            tempVMSnapshotName = vmSnapshot + i;
                            tempVMName = vmName + i;
                            tempVMUsername = vmUsername + i;
                            tempVMPassword = vmPassword + i;
                            tempVMTimestamp = vmTimestamp + i;

                            key.SetValue(tempVMFile, fileNameJoined);
                            key.SetValue(tempVMName, entry.vmName);
                            key.SetValue(tempVMSnapshotName, entry.snapshotName);

                            if(entry.busy==true){

                                key.SetValue(tempVMTimestamp, entry.timestamp.ToString());
                                key.SetValue(tempVMUsername, entry.username);
                                key.SetValue(tempVMPassword, entry.password);

                            }
                            i++;
                      }
                   
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }

        private void buttonDeleteEntry_Click(object sender, EventArgs e)
        {
            string vmNameToDel = (string)listBoxVMList.SelectedItem;
            int toDel = listBoxVMList.SelectedIndex;
            vmList.RemoveAt(toDel);

            if (listBoxVMList.Items.Count >= 1)
            {
                shouldSelect = 0;

            }
            updateList();
        }

       

        private vmEntry getCurrentEntry(){
        
          string curItem = listBoxVMList.SelectedItem.ToString();
         
            foreach (vmEntry entry in vmList){
                if (curItem.Equals(entry.vmName))
                {
                  return entry;
            
                }
              
            } return null;
        }
        private void buttonRestoreVM_Click(object sender, EventArgs e)
        {
           
                   restoreVM(getCurrentEntry());
                  
                          
        }

        public void restoreVM(vmEntry entry){
                    
           
            string fileNameJoined;
            string [] tempVMFileArray = entry.vmFile.Split('\\');
            fileNameJoined = String.Join("\\\\", tempVMFileArray, 0, tempVMFileArray.Length);
            fileNameJoined = "\"" + fileNameJoined + "\"";

            try
            {
              
                /* Now restore the VM to a snapshot */
                string vmrun = "C:\\Program Files\\VMware\\VMware Workstation\\vmrun.exe";
                string command = " revertToSnapshot " + fileNameJoined + " " + entry.snapshotName;
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
                command = " start " + fileNameJoined;
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

        public vmEntry getEntryfromReg(int index)
        {
            string tempvmFile, temVMSnapshotName, tempVMName, tempUsername;
            vmEntry entry = new vmEntry();
            try
            {

                RegistryKey key = Registry.LocalMachine.CreateSubKey(regDir);

                if (key != null)
                {
                    if (key.GetValue(numVM) != null)
                    {
                        tempVMName = vmName + index;
                        tempvmFile = vmFile + index;
                        temVMSnapshotName = vmSnapshot + index;

                        if (key.GetValue(tempVMName) != null)
                        {
                            entry.vmName = key.GetValue(tempVMName).ToString();
                        }
                        if (key.GetValue(tempvmFile) != null)
                        {
                            entry.vmFile = key.GetValue(tempvmFile).ToString();
                        }
                        if (key.GetValue(temVMSnapshotName) != null)
                        {
                            entry.snapshotName = key.GetValue(temVMSnapshotName).ToString();
                        }

                 }
                }
                currentEntry = entry;
            }
            catch (Exception exe)
            {

            }

            return null;
        }


        private void DeleteEntryFromReg(vmEntry entry)
        {
            try
            {
                Debug.WriteLine("Opening Registry");
                RegistryKey key = Registry.LocalMachine.CreateSubKey(regDir);

                // int vmNumber;
                string tempVMTimestamp, tempUsername, tempPassword;

                if (key != null)
                {
                    if (key.GetValue(numVM) != null)
                    {

                        Debug.WriteLine("entry index in delete reg");
                      
                        tempVMTimestamp = vmTimestamp + entry.indexNum;
                        tempUsername = vmUsername + entry.indexNum;
                        tempPassword = vmPassword + entry.indexNum;

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

        public void regMon()
        {
            RegistryMonitor monitor = new RegistryMonitor(regMonDir);

            monitor.RegChanged += new EventHandler(OnRegChanged);
            monitor.Start();

        }

        private void OnRegChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("registry key has changed");
            if (ownWrite == false)
            {
                GetVMInfo();
               
                if (vmList.Count > 0)
                    shouldSelect = 0;

                updateList();
            }
        }

        private void buttonCreateUser_Click(object sender, EventArgs e)
        {

            vmEntry entry = getCurrentEntry();
            if (entry.busy == false) CreateNewUserInVM(entry);
            else MessageBox.Show("can't do it", "busy", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        public byte[] CreateNewUserInVM(vmEntry entry)
        {
            
            bool done = false;

            byte[] request = new byte[CLIENT_REQ_LEN];
            request[0] = CREATE_NEW_USER;

            byte[] clientResp = null;

            /* Process the request in clientData */
            Client vmClient = new Client();
            Debug.WriteLine("Connecting to VM ");

            
                if ((entry.busy == false) && vmClient.ConnectToVM(entry.vmName))//"129.49.16.93"))//""192.168.20.174"))
                {
                    Debug.WriteLine("Sending Request to VM");
                    clientResp = vmClient.sendReqToVM(request);

                    if (clientResp.Length > RESP_LEN_OFFSET)
                    {

                        entry.timestamp = DateTime.Now;
                        string[] temp = System.Text.Encoding.ASCII.GetString(clientResp).Split(':');

                        if (temp.Length == 3)
                        {

                            entry.username = temp[1].ToString();
                            entry.password = temp[2].ToString();
                        }

                        entry.busy = true;
                       
                        WriteEntryToReg(entry);
                        done = true;
                    }
                }
                
            
            return clientResp;
        }

        public void CloseClientConnection(Socket s)
        {
            s.Shutdown(SocketShutdown.Both);
            s.Close();
        }
        private void WriteEntryToReg(vmEntry entry)
        {
            
            try
            {
                ownWrite = true;
                Debug.WriteLine("Opening Registry");
          
                RegistryKey key = Registry.LocalMachine.CreateSubKey(regDir);
                int vmNumber;
                string tempVMFile, tempVMTimestamp, tempVMUsername, tempVMPassword;

                if (key != null)
                {

                    tempVMUsername = vmUsername + entry.indexNum;
                    tempVMPassword = vmPassword + entry.indexNum;
                    tempVMTimestamp = vmTimestamp + entry.indexNum;


                    key.SetValue(tempVMTimestamp, entry.timestamp.ToString());
                    key.SetValue(tempVMUsername, entry.username);
                    key.SetValue(tempVMPassword, entry.password);

                    key.Close();
                }
                ownWrite = false;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception while adding entry to Reg " + e.Message);
            }
        }

        private void textBoxVMFile_TextChanged(object sender, EventArgs e)
        {
            if (vmList[currentIndex].busy == false)
                vmList[currentIndex].vmFile = textBoxVMFile.Text;
        }

        private void textBoxVMSnapshotName_TextChanged(object sender, EventArgs e)
        {
            if (vmList[currentIndex].busy == false)
                vmList[currentIndex].snapshotName = textBoxVMSnapshotName.Text;

        }

        private void textBoxIP_TextChanged(object sender, EventArgs e)
        {
            if (vmList[currentIndex].busy == false)
            {
                vmList[currentIndex].vmName = textBoxIP.Text;
                listBoxVMList.Items[currentIndex] = textBoxIP.Text;
                listBoxVMList.Refresh();
            }
        }

    }

}

