using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;

namespace VmManagerConfiguration
{
    public class Client
    {
        Socket vmSocket;
        IPAddress[] addr;

        int VM_RESP_HEADER_LEN = 32;
        int VM_PORT_NUM = 8401;
        int RESP_LEN_OFFSET = 4;

        public Client()
        {
            vmSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public bool ConnectToVM(string vmName)
        {
            bool connected = true;
            try
            {
                addr = Dns.GetHostAddresses(vmName);

                vmSocket.Blocking = true;

                IPEndPoint ipepServer = new IPEndPoint(addr[0], VM_PORT_NUM);
                vmSocket.Connect(ipepServer);
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
                connected = false;
            }
            catch (Exception eee)
            {
                Console.WriteLine("Socket Connect Error.\n\n" + eee.Message + "\nPossible Cause: Client already running. Check the tasklist for running processes", "Startup Error");
                connected = false;
            }
            return connected;
        }

        public byte[] sendReqToVM(byte[] request)
        {
            byte[] response = new byte[RESP_LEN_OFFSET]; ;

            try
            {
                Debug.WriteLine("Sending Request to VM" + request[0]);

                vmSocket.Send(request, request.Length, 0);
                int bytesReceived = 0;

                byte[] bb = new byte[VM_RESP_HEADER_LEN];

                // make sure the we have read 32 bytes of data.
                do
                {
                    bytesReceived += vmSocket.Receive(bb, bytesReceived, bb.Length - bytesReceived, 0);
                } while (bytesReceived < bb.Length);

                string rcv = "";
                Debug.WriteLine("Response from VM " + bytesReceived);

                for (int i = 0; i < bytesReceived; i++)
                {
                    if (bb[i] != 0)
                        rcv += Convert.ToChar(bb[i]);
                }

                int length = Int32.Parse(rcv);

                if (length > 0)
                {
                    int lengthToRead = length;
                    Debug.WriteLine("Response from VM Length > 0 " + length);

                    ASCIIEncoding asen = new ASCIIEncoding();
                    string ipAddress = addr[0].ToString();
                    ipAddress += ":";
                    Debug.WriteLine("IPAddress of VM " + ipAddress);

                    byte[] ipAddr = asen.GetBytes(ipAddress);
                    /* Add the length of IP address to the received Length */
                    length += ipAddr.Length;

                    Debug.WriteLine("New Length " + length);
                    rcv = Convert.ToString(length);

                    for (int i = rcv.Length; i < RESP_LEN_OFFSET; i++)
                    {
                        rcv = "0" + rcv;
                    }

                    Debug.WriteLine("Length in String Form " + rcv + " string len " + rcv.Length);

                    response = new byte[RESP_LEN_OFFSET + length];
                    bb = asen.GetBytes(rcv);

                    Debug.WriteLine("Length in Byte Form" + bb.Length);

                    for (int i = bb.Length - 1, j = 1; i >= 0; i--, j++)
                    {
                        response[RESP_LEN_OFFSET - j] = bb[i];
                    }
                    /* Copy IP Address */
                    Buffer.BlockCopy(bb, 0, response, 0, bb.Length);

                    /* Copy IP Address */
                    Buffer.BlockCopy(ipAddr, 0, response, RESP_LEN_OFFSET, ipAddr.Length);


                    bytesReceived = 0;
                    int offset = RESP_LEN_OFFSET + ipAddr.Length;
                    do
                    {
                        bytesReceived += vmSocket.Receive(response, offset + bytesReceived,
                                                        lengthToRead - bytesReceived, 0);
                    } while (bytesReceived < lengthToRead);
                    Debug.WriteLine("Data  " + response);
                }
                else
                    response = new byte[RESP_LEN_OFFSET];
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception while reading resposne from VM " + e.Message);
            }
            return response;
        }
    }
}