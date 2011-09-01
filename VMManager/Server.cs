using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using Microsoft.Win32;
using System.Collections.Generic;

namespace VMManager
{
    public class Server
    {
        Socket listeningSocket;
        Socket connectedSocket;


        Thread listeningThread;
        Thread WorkerThread;
        VMActivityQueue queue;

        int CLIENT_REQ_LEN = 1;
        int CLIENT_PORT_NUM = 8400;
        int MAX_CLIENT_CONN = 1000;

        private byte CREATE_NEW_USER = 1;
        private byte RESP_LEN_OFFSET = 4;

        string regDir = @"SOFTWARE\RetherNetworksInc\DOFS-SandBox";
      //  string regMonDir = "HKEY_LOCAL_MACHINE\\SOFTWARE\\RetherNetworksInc\\DOFS-SandBox";
     
        string numVM = "numVM";
        string vmName = "VMName";
        string vmFile = "VMFile";
        string vmSnapshot = "VMSnapshot";
        string vmUsername = "VMUsername";
        string vmPassword = "VMPassword";
        string valid = "valid";
        string interval = "checkInterval";

        string vmTimestamp = "VMTimestamp";
        
        int numVMs;
        List<vmEntry> vmList;
        public Boolean ownWrite { get; set; }

        public Server(VMActivityQueue q)
        {
            queue = q;
            vmList = new List<vmEntry>();
            GetVMInfo();
           // CheckActiveVMSet();
            ownWrite = false;

            listeningThread = new Thread(new ThreadStart(ListenForRequests));
            listeningThread.Start();
           
        }

        public void ListenForRequests()
        {
            try
            {
                listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listeningSocket.Blocking = true;

                IPEndPoint ipall = new IPEndPoint(IPAddress.Any, CLIENT_PORT_NUM);
                listeningSocket.Bind(ipall);

                listeningSocket.Listen(MAX_CLIENT_CONN);
                Debug.WriteLine("Waiting for Connections");
                while (true)
                {
                    connectedSocket = listeningSocket.Accept();
                    if (connectedSocket.Connected)
                    {
                        WorkerThread = new Thread(HandleClientRequest);
                        WorkerThread.IsBackground = true;
                        WorkerThread.Start(connectedSocket);
                    }
                }
            }
            catch (ThreadInterruptedException e)
            {
                Console.WriteLine("Listening thread interrupted");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                if (listeningSocket != null)
                {
                    if (listeningSocket.Connected)
                        listeningSocket.Disconnect(false);
                    listeningSocket.Close();
                }
            }
        }

        public void HandleClientRequest(object conSocket)
        {
            Debug.WriteLine("Connection Accepted");
            Socket sock = (Socket)conSocket;

            try
            {
                bool clientClosed = false;

                byte[] clientData = new byte[CLIENT_REQ_LEN];
                byte[] clientResp = null;
                int bytesRead = 0;
                int bytesReceived = 0;

                Debug.WriteLine("Waiting for Data from Client");

                do
                {
                    bytesRead = sock.Receive(clientData, bytesReceived, clientData.Length - bytesReceived, 0);
                    if (bytesRead == 0)
                    {
                        clientClosed = true;
                        break;
                    }
                    bytesReceived += bytesRead;
                } while (bytesReceived < clientData.Length);

                Debug.WriteLine("Received Data from Client " + bytesReceived);

                if (clientClosed == false)
                {
                    byte request = clientData[0];
                    request -= Convert.ToByte('0');
                    Debug.WriteLine("Processing Req from Client " + request);
                    if (request == CREATE_NEW_USER)
                    {
                        clientResp = CreateNewUserInVM();
                    }
                    if (clientResp != null)
                    {
                        Debug.WriteLine("Sending Response to  Client " + clientResp.Length);

                        /* First send the length of the data */
                        sock.Send(clientResp, RESP_LEN_OFFSET, 0);
                        Debug.WriteLine("Respone 1: "+ System.Text.ASCIIEncoding.ASCII.GetString(clientResp));
/*
                        string address = "192.168.20.174:TestUser1:abcdefghij";
                        byte[] ipAddr = System.Text.ASCIIEncoding.ASCII.GetBytes(address);
                        Debug.WriteLine("Sending Response to  Client Byte " + ipAddr.Length);

                        if (clientResp.Length > RESP_LEN_OFFSET)
                            sock.Send(ipAddr, ipAddr.Length, 0);
*/
                        if(clientResp.Length > RESP_LEN_OFFSET)
                            sock.Send(clientResp, RESP_LEN_OFFSET, clientResp.Length - RESP_LEN_OFFSET, 0);

                        Debug.WriteLine("Respone 2: " + System.Text.ASCIIEncoding.ASCII.GetString(clientResp));
                    }
                    if (clientResp == null) {
                        String zeroString = "0000";

                        clientResp = System.Text.ASCIIEncoding.ASCII.GetBytes(zeroString);
                        Debug.WriteLine("Sending sorry 0000 response ");
                        sock.Send(clientResp, RESP_LEN_OFFSET, 0);
                        
                    }
                }
            }
            catch (ThreadInterruptedException x)
            {
                Debug.WriteLine("Worker Thread interrupted");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }
            finally
            {
                CloseClientConnection(sock);
//                CLogger.WriteLog(ELogLevel.INFO, "Closing connection of Client: " + newclient.Address);
                Console.WriteLine( "Closing connection of Client: ");
            }
        }

        public byte[] CreateNewUserInVM()
        {
            bool done = false;

            byte[] request = new byte[CLIENT_REQ_LEN];
            request[0] = CREATE_NEW_USER;

            byte[] clientResp = null;
            
            /* Process the request in clientData */
            Client vmClient = new Client();
            Debug.WriteLine("Connecting to VM ");

            foreach (vmEntry entry in vmList)
            {
                if ((entry.busy == false) && vmClient.ConnectToVM(entry.vmName))//"129.49.16.93"))//""192.168.20.174"))
                {
                    Debug.WriteLine("Sending Request to VM");
                    clientResp = vmClient.sendReqToVM(request);

                    if (clientResp.Length > RESP_LEN_OFFSET){


                        entry.timestamp = DateTime.Now;
                        string[] temp = System.Text.Encoding.ASCII.GetString(clientResp).Split(':');

                        if (temp.Length == 3) {
                          
                            entry.username = temp[1].ToString();
                            entry.password = temp[2].ToString();
                        }
                       
                        entry.busy = true;
                        queue.Enqueue(entry);

                        WriteEntryToReg(entry);
                        done = true;
                    }
                }
                if (done == true)
                    break;
            }
            return clientResp;
        }

        public void CloseClientConnection(Socket s)
        {
            s.Shutdown(SocketShutdown.Both);
            s.Close();
        }

        private void GetVMInfo()
        {
            Debug.WriteLine("Get VMInfo");

            try
            {
                Debug.WriteLine("Opening Registry");
                RegistryKey key = Registry.LocalMachine.OpenSubKey(regDir);
                
                string tempVMName, tempVMFile, tempVMSnapshot, tempVMUsername, tempVMPassword, tempVMTimestamp;


                if (key != null)
                {
                    Debug.WriteLine("Successfully opened Registry");
                    if (key.GetValue(numVM) != null)
                    {
                        numVMs = Convert.ToInt32(key.GetValue(numVM).ToString());

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
                                entry.vmFile = key.GetValue(tempVMFile).ToString();
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
                                queue.Enqueue(entry);

                            }
                            else
                            {
                                 entry.busy = false;
                            }

                           
                            Debug.WriteLine("VM Entry " + entry.vmName + " " + entry.vmFile + " " + entry.snapshotName);
                            vmList.Add(entry);
                        }
                    }
                    key.Close();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine( "Exception while reading from reg " + e.Message);
            }
        }

        private void WriteEntryToReg(vmEntry entry)
        {
            ownWrite = true;
            try
            {
                Debug.WriteLine("Opening Registry");
               
                RegistryKey key = Registry.LocalMachine.CreateSubKey(regDir);
                int vmNumber;
                string tempVMFile, tempVMTimestamp,tempVMUsername,tempVMPassword;

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

      
        public void regChanged()
        {
            Debug.WriteLine("Sandbox Server: registry key has changed");
            if (ownWrite == false)
            {
                queue.clearQueue();
                lock (vmList)
                {
                    Debug.WriteLine("onRegChanged lock this");

                    vmList.Clear();


                } GetVMInfo();
                //   updateList();
                
            }
        }
    }
}
