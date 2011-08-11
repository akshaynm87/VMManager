using System;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;
using System.Diagnostics;

namespace OSAccountsManager
{
    class UserAccountManager
    {
        private byte PASS_LEN = 10;

        public UserAccountManager()
        {
        }

        public string AddUserAccount()
        {
            string ret = null;
            DirectoryEntry hostMachineDirectory = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
            Debug.WriteLine(" Entering Add User Account to " + Environment.MachineName);
            DirectoryEntries entries = hostMachineDirectory.Children;

            try
            {
                string username = "TestUser1";
                string password = GenerateRandomString(PASS_LEN);
                Debug.WriteLine(" Adding New User " + password);
                
                DirectoryEntry obUser = entries.Add(username, "User");
                obUser.Invoke("SetPassword", password);
                obUser.Properties["FullName"].Add("Local user");
                obUser.Invoke("Put", new object[] { "UserFlags", 0x10000 });
                obUser.CommitChanges();

                DirectoryEntry grp = entries.Find("Remote Desktop Users", "group");
                if (grp != null)
                {
                    Debug.WriteLine(" Adding To RemoteDesktopUsers " + password);
                    grp.Invoke("Add", new object[] { obUser.Path.ToString() });
                }
                Debug.WriteLine(" Commited Changes to hostMachine");
                ret = username + ":" + password;
            }
            catch (Exception e)
            {
                Debug.WriteLine(" Exception while creating user accounts " + e.Message);
            }
            return ret;
        }

        private string GenerateRandomString(int length) 
        { 
            string allowedLetterChars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ"; 
            string allowedNumberChars = "23456789"; 
            char[] chars = new char[length]; 
            Random rd = new Random(); 

            bool useLetter =  true; 
            for (int i = 0; i < length; i++) 
            { 
                if (useLetter) 
                { 
                    chars[i] = allowedLetterChars[rd.Next(0, allowedLetterChars.Length)]; 
                    useLetter = false; 
                } 
                else 
                { 
                    chars[i] = allowedNumberChars[rd.Next(0, allowedNumberChars.Length)]; 
                    useLetter = true; 
                } 
            } 
            return new string(chars); 
        }
    }
}
