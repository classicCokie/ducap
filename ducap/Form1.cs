using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;


namespace ducap
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        public  void updateList(String item) {
            listBox2.Text = item;
            
        }

        private void startProxyServer() {
            tcpProxy tcpP = new tcpProxy();
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
            //try
            //{
            //    string curItem = listBox1.SelectedItem.ToString();
            //    //string[] host = new string[2];
            //    if (curItem != null)
            //    {            
            //        int ipLength = curItem.Length - 26;
            //        host[0] = curItem.Substring(26, ipLength);
            //        host[1] = curItem.Substring(5, 17);
            //    }

            //    //Send the Arp Poison Packages
            //    //ArpSpoofer arper = new ArpSpoofer(host);
            //    //arper.sendArpSpoof();
            //} catch  {

            //    Console.WriteLine("No Host selected!!!");
            //}

            //Send the Arp Poison Packages
            string[] host = new string[2];
            ArpSpoofer arper = new ArpSpoofer(host);
            arper.sendArpSpoof();
            Thread proxy = new Thread(startProxyServer);
            proxy.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String item = listBox1.SelectedItem.ToString();

            if (!listBox2.Items.Contains(item) && listBox2.Items.Count <= 1 || listBox2.Items.Count == 0) {
                listBox2.Items.Add(item);
            }
        }
    }

  

    }


