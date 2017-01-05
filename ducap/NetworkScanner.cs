using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using PcapDotNet.Base;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Arp;
using PcapDotNet.Packets.Dns;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.Gre;
using PcapDotNet.Packets.Http;
using PcapDotNet.Packets.Icmp;
using PcapDotNet.Packets.Igmp;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.IpV6;
using PcapDotNet.Packets.Transport;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ducap
{
    class NetworkScanner
    {
        private int counter = 1;
        List<string> hostList = new List<string>(); 
       private void start_scanning()
        {
            List<Thread> _threadList = new List<Thread>();
            for (int i = 1; i < 255; i++)
            {
                Thread request = new Thread(() => scan_Network(counter++));
                _threadList.Add(request);
                request.Start();
            }

            foreach (Thread machineThread in _threadList)
            {
                machineThread.Join();
            }
           
        }

        public List<string> getHosts()
        {
            start_scanning();
            return hostList;
        }

        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(uint DestIP, uint SrcIP, byte[] pMacAddr, ref int PhyAddrLen);

       private void scan_Network(int counter)
        {
            Console.WriteLine(counter);
                string ipRange = "192.168.178.";
                IPAddress dst = IPAddress.Parse(String.Concat(ipRange, counter));
                uint uintAddress = BitConverter.ToUInt32(dst.GetAddressBytes(), 0);
                byte[] macAddr = new byte[6];
                int macAddrLen = macAddr.Length;
                int retValue = SendARP(uintAddress, 0, macAddr, ref macAddrLen);
                if (retValue == 0)
                {
                    string[] str = new string[(int)macAddrLen];
                    for (int i = 0; i < macAddrLen; i++)
                        str[i] = macAddr[i].ToString("x2");

                hostList.Add("MAC: " + string.Join(":", str) + " IP: " + String.Concat(ipRange, counter));
                }
        }
    }
}

