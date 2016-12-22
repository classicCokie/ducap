using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;



namespace ducap
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Thread proxy = new Thread(startProxyServer);
            proxy.Start();
        }

        private void startProxyServer() {           
            ProxyServer proxy = new ProxyServer(80);
            proxy.StartServer();

            while (true)
            {
                proxy.AcceptConnection();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NetworkScanner scanner = new NetworkScanner();
            List<string> hosts = scanner.getHosts();
            listBox1.DataSource = hosts;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //get Selected Host and Parse into Array
            //try {
            //    string curItem = listBox1.SelectedItem.ToString();

            //    if (curItem != null)
            //    {
            //        string[] host = new string[2];
            //        int ipLength = curItem.Length - 26;
            //        host[0] = curItem.Substring(26, ipLength);
            //        host[1] = curItem.Substring(5, 17);

            //    }
            //}
            
         

            //Send the Arp Poison Packages
            ArpSpoofer arper = new ArpSpoofer();
            arper.sendArpSpoof();
        }
    }

  

    }


