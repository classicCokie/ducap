using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace ducap
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NetworkScanner scanner = new NetworkScanner();
            List<string> hosts = scanner.getHosts();
            listBox1.DataSource = hosts;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            

            ProxyServer proxy =  new ProxyServer(80);
            proxy.StartServer();

            while(true)
            {
                proxy.AcceptConnection();
            }

           



        }
    }

    }


