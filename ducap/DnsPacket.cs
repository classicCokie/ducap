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

namespace ducap
{
    class DnsPacket
    {
        //DNS PACKET BUILDER 
        public  Packet BuildDnsPacket(Packet origPacket)
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

            DnsLayer dnsLayer =
                new DnsLayer
                {
                    Id = origPacket.Ethernet.IpV4.Udp.Dns.Id,
                    IsResponse = origPacket.Ethernet.IpV4.Udp.Dns.IsResponse,
                    OpCode = origPacket.Ethernet.IpV4.Udp.Dns.OpCode,
                    IsAuthoritativeAnswer = origPacket.Ethernet.IpV4.Udp.Dns.IsAuthoritativeAnswer,
                    IsTruncated = origPacket.Ethernet.IpV4.Udp.Dns.IsTruncated,
                    IsRecursionDesired = origPacket.Ethernet.IpV4.Udp.Dns.IsRecursionDesired,
                    IsRecursionAvailable = origPacket.Ethernet.IpV4.Udp.Dns.IsRecursionAvailable,
                    FutureUse = origPacket.Ethernet.IpV4.Udp.Dns.FutureUse,
                    IsAuthenticData = origPacket.Ethernet.IpV4.Udp.Dns.IsAuthenticData,
                    IsCheckingDisabled = origPacket.Ethernet.IpV4.Udp.Dns.IsCheckingDisabled,
                    ResponseCode = origPacket.Ethernet.IpV4.Udp.Dns.ResponseCode,
                    Queries = origPacket.Ethernet.IpV4.Udp.Dns.Queries,
                    Answers = origPacket.Ethernet.IpV4.Udp.Dns.Answers,
                    Authorities = origPacket.Ethernet.IpV4.Udp.Dns.Authorities,
                    Additionals = origPacket.Ethernet.IpV4.Udp.Dns.Additionals,
                    DomainNameCompressionMode = DnsDomainNameCompressionMode.All,
                };

            PacketBuilder builder = new PacketBuilder(ethernetLayer, ipV4Layer, udpLayer, dnsLayer);

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
