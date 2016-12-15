using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ducap
{
    public class TcpForwarderSlim
    {
        public void SocketProcessor(object SocketObject)
        {


            // ex("Socket communication processing ...");
            Socket ClientSocket = (Socket)SocketObject;
            String ServerHost = null;
            Socket ServerSocket = new Socket(ClientSocket.AddressFamily, ClientSocket.SocketType, ClientSocket.ProtocolType);

            byte[] SendBuffer = new byte[8 * 1024];
            int SendSize = 0;

            byte[] ReceiveBuffer = new byte[8 * 1024];
            int ReceiveSize = 0;

            // CROSS-SOCKET TRAFFIC EXCHANGE
            try
            {

                while (true)
                {

                    if (!ClientSocket.Connected) break;

                    // PROCESS CLIENT REQUEST STREAM
                    if (ClientSocket.Available > 0)
                    {
                        SendSize = ClientSocket.Receive(SendBuffer);

                        // TRY TO GET HOST
                        String Method = Encoding.ASCII.GetString(SendBuffer, 0, 3);
                        if (Method == "GET")
                        {
                            String RequestString = Encoding.ASCII.GetString(SendBuffer);
                            int P1 = RequestString.IndexOf(' ') + 1;
                            int P2 = RequestString.IndexOf(' ', P1);
                            String URL = RequestString.Substring(P1, P2 - P1);
                            Uri URI = new System.Uri(URL);
                            String NewHost = URI.Host;
                            Console.WriteLine("Request: " + URL);

                            // (RE)CONNECT TO SERVER
                            if (NewHost != ServerHost)
                            {
                                if (ServerSocket.Connected) ServerSocket.Close();
                                ServerSocket = new Socket(ClientSocket.AddressFamily, ClientSocket.SocketType, ClientSocket.ProtocolType);
                                ServerHost = NewHost;
                                ServerSocket.Connect(ServerHost, 80);
                                Console.WriteLine("Connected to " + ServerHost);
                            }
                        }

                        if (ServerSocket.Connected)
                        {
                            if (SendSize > 0)
                                ServerSocket.Send(SendBuffer, SendSize, SocketFlags.None);
                        }
                    }

                    // REDIRECT REPLY FROM SERVER TO HOST
                    if (ServerSocket.Connected)
                    {
                        if (ServerSocket.Available > 0)
                        {
                            ReceiveSize = ServerSocket.Receive(ReceiveBuffer);
                            if (ReceiveSize > 0)
                                ClientSocket.Send(ReceiveBuffer, ReceiveSize, SocketFlags.None);
                        }
                    }
                    else break;

                    if (ClientSocket.Available == 0 && ServerSocket.Available == 0)
                        Thread.Sleep(100);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION: " + e.Message);
            }

            if (ServerSocket.Connected) ServerSocket.Close();
            if (ClientSocket.Connected) ClientSocket.Close();


        }
    }
}
