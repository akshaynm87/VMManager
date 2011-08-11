using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace OSAccountsManager
{
    class Server
    {
        Socket listeningSocket;
        Socket connectedSocket;

        Thread listeningThread;

        int CLIENT_REQ_LEN = 1;
        int CLIENT_PORT_NUM = 8401;
        int MAX_CLIENT_CONN = 1000;
        private byte CREATE_NEW_USER = 1;

        public Server()
        {
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

                Debug.WriteLine("Listening for Clients");

                while (true)
                {
                    connectedSocket = listeningSocket.Accept();
                    if (connectedSocket.Connected)
                    {
                        HandleClientRequest(connectedSocket);
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

        public void HandleClientRequest(Socket sock)
        {
            try
            {
                Debug.WriteLine("Client Connected");
                bool clientClosed = false;

                byte[] clientData = new byte[CLIENT_REQ_LEN];
                byte[] clientResp = null;
                int bytesRead = 0;
                int bytesReceived = 0;

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

                Debug.WriteLine("Data received from Client "+ bytesReceived);

                if (clientClosed == false)
                {
                    ASCIIEncoding asen = new ASCIIEncoding();

                    /* Process the request in clientData */
                    byte request = clientData[0];

                    Debug.WriteLine("Processing request " + request);
                    if (request == CREATE_NEW_USER)
                    {
                        Debug.WriteLine("Creating User account" + request);

                        UserAccountManager accManager = new UserAccountManager();
                        string accDetails = accManager.AddUserAccount();
                        if(accDetails != null)
                        {
                            clientResp = asen.GetBytes(accDetails);
                        }
                    }
                    /* Send the length of the data first */
                    int headerLength;
                    if (clientResp != null)
                        headerLength = clientResp.Length;
                    else
                        headerLength = 0;

                    byte[] header = new byte[32];
                    byte[] msg = asen.GetBytes("" + headerLength);
                    Buffer.BlockCopy(msg, 0, header, 0, msg.Length);
                    sock.Send(header, header.Length, 0);

                    /* Send the data */
                    if(headerLength > 0)
                        sock.Send(clientResp, clientResp.Length, 0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }
            finally
            {
                CloseClientConnection(sock);
                //                CLogger.WriteLog(ELogLevel.INFO, "Closing connection of Client: " + newclient.Address);
                Console.WriteLine("Closing connection of Client: ");
            }
        }

        public void CloseClientConnection(Socket s)
        {
            s.Shutdown(SocketShutdown.Both);
            s.Close();
        }
    }
}
