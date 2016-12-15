using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;


namespace ducap
{
    public class ProxyServer
    {

        private int listenPort;
        private TcpListener listener;

        public ProxyServer(int port)
        {
            this.listenPort = port;
            this.listener = new TcpListener(IPAddress.Any, this.listenPort);
            Console.WriteLine(this.listener);
        }

        public void StartServer()
        {
            Console.WriteLine("Started Server");
            this.listener.Start();
        }

        public void AcceptConnection()
        {
            Socket newClient = this.listener.AcceptSocket();
            ProxyClient client = new ProxyClient(newClient);
            Console.WriteLine("Start listening for connections");
            client.StartHandling();
        }
    }
}
