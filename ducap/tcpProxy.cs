using System;
using System.Collections.Generic;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Transport;
using PcapDotNet.Packets.IpV4;

namespace ducap
{
    class tcpProxy
    {
        public  PacketCommunicator communicator;
        public static PacketCommunicator communicator2;
        public tcpProxy()
        {
            // Retrieve the device list from the local machine
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            PacketDevice selectedDevice = allDevices[0];
            communicator = selectedDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000);
            // start the capture
            // Check the link layer. We support only Ethernet for simplicity.
            if (communicator.DataLink.Kind != DataLinkKind.Ethernet)
            {
                Console.WriteLine("This program works only on Ethernet networks.");
                return;
            }

            //communicator.SetFilter("udp");
            Console.WriteLine("Listening on " + selectedDevice.Description + "...");

            communicator2 = communicator;
            // start the capture
            communicator.ReceivePackets(0, PacketHandler);
        }

        // Callback function invoked by Pcap.Net for every incoming packet
        private static void PacketHandler(Packet packet)
        {
           

            IpV4Datagram ip = packet.Ethernet.IpV4;
            UdpDatagram udp = ip.Udp;

            Form1 myForm = new Form1();
                
            myForm.updateList(Convert.ToString(ip.Source));


            if (Convert.ToString(ip.Source) == "192.168.178.44" || Convert.ToString(ip.Source) == "192.168.178.1") {

                if (Convert.ToString(packet.Ethernet.IpV4.Protocol) == "Tcp")
                {
                    TcpPacket tcp = new ducap.TcpPacket();
                    communicator2.SendPacket(tcp.BuildTcpPacket(packet));
                }
                if (Convert.ToString(packet.Ethernet.IpV4.Protocol) == "Udp")
                {
                    UdpPacket ufo = new UdpPacket();
                    communicator2.SendPacket(ufo.BuildUdpPacket(packet));
                }
                if (Convert.ToString(packet.Ethernet.IpV4.Protocol) == "Dns")
                {
                    DnsPacket dns = new DnsPacket();
                    communicator2.SendPacket(dns.BuildDnsPacket(packet));
                }
                if (Convert.ToString(packet.Ethernet.IpV4.Protocol) == "Icmp")
                {
                    IcmpPacket dns = new IcmpPacket();
                    communicator2.SendPacket(dns.BuildIcmpPacket(packet));
                }


            }          
        }

       

        



      

        
       


    }




}
