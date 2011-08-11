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
        string writeRegDir = @"SOFTWARE\RetherNetworksInc\DOFS-SandBox\CurrentVMSet";
        string numVM = "numVM";
        string vmName = "VMName";
        string vmFile = "VMFile";
        string vmSnapshot = "VMSnapshot";

        string vmPath = "VMPath";
        string vmTimestamp = "VMTimestamp";

        int numVMs;
        List<vmEntry> vmList;        

        public Server(VMActivityQueue q)
        {
            queue = q;
            vmList = new List<vmEntry>();
            GetVMInfo();
            CheckActiveVMSet();
           
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
/*
                        string address = "192.168.20.174:TestUser1:abcdefghij";
                        byte[] ipAddr = System.Text.ASCIIEncoding.ASCII.GetBytes(address);
                        Debug.WriteLine("Sending Response to  Client Byte " + ipAddr.Length);

                        if (clientResp.Length > RESP_LEN_OFFSET)
                            sock.Send(ipAddr, ipAddr.Length, 0);
*/
                        if(clientResp.Length > RESP_LEN_OFFSET)
                            sock.Send(clientResp, RESP_LEN_OFFSET, clientResp.Length - RESP_LEN_OFFSET, 0);
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

                    if (clientResp.Length > RESP_LEN_OFFSET)
                    {
/*                        vmEntry entry = new vmEntry(DateTime.Now,
                                                    "\"C:\\Documents and Settings\\dofsadmin\\My Documents\\My Virtual Machines\\Windows XP Professional\\Windows XP Professional.vmx\"",
                                                    "WinXP-CompNameChange",
                                                    "192.168.20.174");
  */
                        entry.timestamp = DateTime.Now;
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
            try
            {
                Debug.WriteLine("Opening Registry");
                RegistryKey key = Registry.LocalMachine.OpenSubKey(regDir);
                string tempVMName, tempVMFile, tempVMSnapshot;

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
                            
                            Debug.WriteLine("Temp VM String " + tempVMName + " " + tempVMFile + " " + tempVMSnapshot);

                            vmEntry entry = new vmEntry();
                            if (key.GetValue(tempVMName) != null)
                            {
                                entry.vmName = key.GetValue(tempVMName).ToString();
                            }
                            if (key.GetValue(tempVMFile) != null)
                            {
                                entry.vmPath = key.GetValue(tempVMFile).ToString();
                            }
                            if (key.GetValue(tempVMSnapshot) != null)
                            {
                                entry.snapshotName = key.GetValue(tempVMSnapshot).ToString();
                            }
                            Debug.WriteLine("VM Entry " + entry.vmName + " " + entry.vmPath + " " + entry.snapshotName);
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
            try
            {
                Debug.WriteLine("Opening Registry");
                RegistryKey key = Registry.LocalMachine.CreateSubKey(writeRegDir);
                int vmNumber;
                string tempVMPath, tempVMTimestamp;

                if (key != null)
                {
                    if (key.GetValue(numVM) != null)
                    {
                        vmNumber = Convert.ToInt32(key.GetValue(numVM).ToString());
                        vmNumber++;

                        tempVMPath = vmPath + vmNumber;
                        tempVMTimestamp = vmTimestamp + vmNumber;

                        key.SetValue(numVM, vmNumber);
                        key.SetValue(tempVMPath, entry.vmPath);
                        key.SetValue(tempVMTimestamp, entry.timestamp.ToString());

                        entry.indexNum = vmNumber;
                    }
                    key.Close();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception while adding entry to Reg " + e.Message);
            }
        }

        private void CheckActiveVMSet()
        {
            try
            {
                Debug.WriteLine("Opening Registry");
                RegistryKey key = Registry.LocalMachine.CreateSubKey(writeRegDir);
                int vmNumber;
                string tempVMPath, tempVMTimestamp;
                string vmPathValue;
                DateTime vmTimestampValue;

                if (key != null)
                {
                    if (key.GetValue(numVM) != null)
                    {   
                        vmNumber = Convert.ToInt32(key.GetValue(numVM).ToString());

                        for (int i = 1; i <= vmNumber; i++)
                        {
                            tempVMPath = vmPath + i;
                            tempVMTimestamp = vmTimestamp + i;

                            if (key.GetValue(tempVMPath) != null)
                            {
                                vmPathValue = key.GetValue(tempVMPath).ToString();

                                Debug.WriteLine("Value in VmPathValue " + vmPathValue + " Num values " + vmNumber);
                                foreach (vmEntry entry in vmList)
                                {
                                    if (vmPathValue.Equals(entry.vmPath))
                                    {
                                        Debug.WriteLine("Found a match " + entry.vmPath);
                                        if (key.GetValue(tempVMTimestamp) != null)
                                        {
                                            vmTimestampValue = Convert.ToDateTime(key.GetValue(tempVMTimestamp).ToString());

                                            entry.timestamp = vmTimestampValue;
                                            entry.busy = true;
                                            entry.indexNum = i;

                                            queue.Enqueue(entry);
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        key.SetValue(numVM, 0);
                    }
                    key.Close();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception while checking Reg entries " + e.Message);
            }
        }
    }
}
