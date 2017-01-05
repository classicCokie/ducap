using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PcapDotNet.Base;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Arp;
using PcapDotNet.Packets.Ethernet;
using System.Threading;
using System.Globalization;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Icmp;
using PcapDotNet.Packets.Transport;
using System.Net;
using PcapDotNet.Packets.Dns;
using PcapDotNet.Packets.Gre;
using PcapDotNet.Packets.Http;
using PcapDotNet.Packets.Igmp;
using PcapDotNet.Packets.IpV6;


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

            // Compile the filter
            //using (BerkeleyPacketFilter filter = communicator.CreateFilter("tcp"))
            //{
            //    // Set the filter
            //    communicator.SetFilter(filter);
            //}
            // Compile the filter
            using (BerkeleyPacketFilter filter = communicator.CreateFilter("tcp"))
            {
                // Set the filter
                communicator.SetFilter(filter);
            }

            Console.WriteLine("Listening on " + selectedDevice.Description + "...");

            communicator2 = communicator;
            // start the capture
            communicator.ReceivePackets(0, PacketHandler);
        }

        // Callback function invoked by Pcap.Net for every incoming packet
        private static void PacketHandler(Packet packet)
        {          
            // print timestamp and length of the packet
            Console.WriteLine(packet.Timestamp.ToString("yyyy-MM-dd hh:mm:ss.fff") + " length:" + packet.Length);
            IpV4Datagram ip = packet.Ethernet.IpV4;
            UdpDatagram udp = ip.Udp;
            if (Convert.ToString(ip.Source) == "192.168.178.44" || Convert.ToString(ip.Source) == "192.168.178.1") {
                communicator2.SendPacket(BuildTcpPacket(packet));
                // print ip addresses and udp ports
                Console.WriteLine(ip.Source + ":" + udp.SourcePort + " -> " + ip.Destination + ":" + udp.DestinationPort);
            }          
        }

        private static MacAddress lookUpMacAdress(String dest) {
            MacAddress mac;
            
            mac = new MacAddress("c0:25:06:3c:ac:78");
            
            if (dest == "192.168.178.44")
            {
                mac = new MacAddress("84:4b:f5:1a:81:f6");
            }

            return mac;
        }

        //UDP Packt Builder
        private static Packet BuildUdpPacket(Packet origPacket)
        {
            EthernetLayer ethernetLayer =
                new EthernetLayer
                {
                    Source = origPacket.Ethernet.Source,
                    Destination = lookUpMacAdress(Convert.ToString(origPacket.Ethernet.IpV4.Destination)),
                    EtherType = EthernetType.None, // Will be filled automatically.
                };
           
            IpV4Layer ipV4Layer =
                new IpV4Layer
                {
                    Source = origPacket.Ethernet.IpV4.Source,
                    CurrentDestination = origPacket.Ethernet.IpV4.Destination,
                    Fragmentation = IpV4Fragmentation.None,
                    HeaderChecksum = null, // Will be filled automatically.
                    Identification = origPacket.Ethernet.IpV4.Identification,
                    Options = IpV4Options.None,
                    Protocol = null, // Will be filled automatically.
                    Ttl = origPacket.Ethernet.IpV4.Ttl,
                    TypeOfService = origPacket.Ethernet.IpV4.TypeOfService,
                };
            
            UdpLayer udpLayer =
                new UdpLayer
                {
                    SourcePort = origPacket.Ethernet.IpV4.Udp.SourcePort,
                    DestinationPort = origPacket.Ethernet.IpV4.Udp.DestinationPort,
                    Checksum = null, // Will be filled automatically.
                    CalculateChecksumValue = true,
                };

           ,
            
            PayloadLayer payloadLayer =
                new PayloadLayer
                {
                    Data = new Datagram(Encoding.ASCII.GetBytes("hello world")),
                };

            PacketBuilder builder = new PacketBuilder(ethernetLayer, ipV4Layer, udpLayer, payloadLayer);

            return builder.Build(DateTime.Now);
        }

       



        //TCP PACKET BUILDER
        private static Packet BuildTcpPacket(Packet origPacket)
        {
          EthernetLayer ethernetLayer =
                new EthernetLayer
                {
                    Source = origPacket.Ethernet.Source,
                    Destination = lookUpMacAdress(Convert.ToString(origPacket.Ethernet.IpV4.Destination)),
                    EtherType = EthernetType.None, // Will be filled automatically.
                };
            
            IpV4Layer ipV4Layer =
                new IpV4Layer
                {
                    Source = origPacket.Ethernet.IpV4.Source,
                    CurrentDestination = origPacket.Ethernet.IpV4.Destination,
                    Fragmentation = IpV4Fragmentation.None,
                    HeaderChecksum = null, // Will be filled automatically.
                    Identification = origPacket.Ethernet.IpV4.Identification,
                    Options = IpV4Options.None,
                    Protocol = null, // Will be filled automatically.
                    Ttl = origPacket.Ethernet.IpV4.Ttl,
                    TypeOfService = origPacket.Ethernet.IpV4.TypeOfService,
                };
            

            TcpLayer tcpLayer =
                new TcpLayer
                {
                    SourcePort = origPacket.Ethernet.IpV4.Tcp.SourcePort,
                    DestinationPort = origPacket.Ethernet.IpV4.Tcp.DestinationPort,
                    Checksum = null, // Will be filled automatically.
                    SequenceNumber = origPacket.Ethernet.IpV4.Tcp.SequenceNumber,
                    AcknowledgmentNumber = origPacket.Ethernet.IpV4.Tcp.AcknowledgmentNumber,
                    ControlBits = TcpControlBits.Acknowledgment,
                    Window = origPacket.Ethernet.IpV4.Tcp.Window,
                    UrgentPointer = origPacket.Ethernet.IpV4.Tcp.UrgentPointer,              
                    Options = origPacket.Ethernet.IpV4.Tcp.Options,
                };

            
            PayloadLayer payloadLayer =
                new PayloadLayer
                {
                    Data = new Datagram(Encoding.ASCII.GetBytes("hello world")),
                };

            PacketBuilder builder = new PacketBuilder(ethernetLayer, ipV4Layer, tcpLayer, payloadLayer);

            return builder.Build(DateTime.Now);
        }

        //HTTP PACKET BUILDER
        private static Packet BuildHttpPacket(Packet origPacket)
        {
            EthernetLayer ethernetLayer =
                new EthernetLayer
                {
                    Source = origPacket.Ethernet.Source,
                    Destination = lookUpMacAdress(Convert.ToString(origPacket.Ethernet.IpV4.Destination)),
                    EtherType = EthernetType.None, // Will be filled automatically.
                };

            IpV4Layer ipV4Layer =
                new IpV4Layer
                {
                    Source = origPacket.Ethernet.IpV4.Source,
                    CurrentDestination = origPacket.Ethernet.IpV4.Destination,
                    Fragmentation = IpV4Fragmentation.None,
                    HeaderChecksum = null, // Will be filled automatically.
                    Identification = origPacket.Ethernet.IpV4.Identification,
                    Options = IpV4Options.None,
                    Protocol = null, // Will be filled automatically.
                    Ttl = origPacket.Ethernet.IpV4.Ttl,
                    TypeOfService = origPacket.Ethernet.IpV4.TypeOfService,
                };


            TcpLayer tcpLayer =
                new TcpLayer
                {
                    SourcePort = origPacket.Ethernet.IpV4.Tcp.SourcePort,
                    DestinationPort = origPacket.Ethernet.IpV4.Tcp.DestinationPort,
                    Checksum = null, // Will be filled automatically.
                    SequenceNumber = origPacket.Ethernet.IpV4.Tcp.SequenceNumber,
                    AcknowledgmentNumber = origPacket.Ethernet.IpV4.Tcp.AcknowledgmentNumber,
                    ControlBits = TcpControlBits.Acknowledgment,
                    Window = origPacket.Ethernet.IpV4.Tcp.Window,
                    UrgentPointer = origPacket.Ethernet.IpV4.Tcp.UrgentPointer,
                    Options = origPacket.Ethernet.IpV4.Tcp.Options,
                };

            HttpRequestLayer httpLayer =
                new HttpRequestLayer
                {
                    Version = HttpVersion.Version11,
                    Header = new HttpHeader(new HttpContentLengthField(11)),
                    Body = new Datagram(Encoding.ASCII.GetBytes("hello world")),
                    Method = new HttpRequestMethod(HttpRequestKnownMethod.Get),
                    Uri = @"http://pcapdot.net/",
                };

            PacketBuilder builder = new PacketBuilder(ethernetLayer, ipV4Layer, tcpLayer, httpLayer);

            return builder.Build(DateTime.Now);
        }


    }
        
        
    
}
