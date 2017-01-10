using PcapDotNet.Packets.Ethernet;
using System;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Http;
using PcapDotNet.Packets.Dns;
using PcapDotNet.Packets.Icmp;

namespace ducap
{
    class IcmpPacket
    {
        public  Packet BuildIcmpPacket(Packet origPacket)
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
            
            IcmpEchoLayer icmpLayer =
                new IcmpEchoLayer
                {
                    Checksum = origPacket.Ethernet.IpV4.Icmp.Checksum, // Will be filled automatically.
                    Identifier = 456,
                    SequenceNumber = 800,
                };

            PacketBuilder builder = new PacketBuilder(ethernetLayer, ipV4Layer, icmpLayer);

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
