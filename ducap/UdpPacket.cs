using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ducap
{
    class UdpPacket
    {

        public UdpPacket() {

        }
        //UDP Packt Builder
        public  Packet BuildUdpPacket(Packet origPacket)
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

            PayloadLayer payloadLayer =
                new PayloadLayer
                {
                    Data = origPacket.Ethernet.IpV4.Udp.Payload,
                };

            PacketBuilder builder = new PacketBuilder(ethernetLayer, ipV4Layer, udpLayer, payloadLayer);

            return builder.Build(DateTime.Now);
        }


        private static MacAddress lookUpMacAdress(String dest)
        {
            MacAddress mac;

            mac = new MacAddress("9c:80:df:84:9b:9c");

            if (dest == "192.168.2.176")
            {
                mac = new MacAddress("84:4b:f5:1a:81:f6");
            }

            return mac;
        }


    }

  
}
