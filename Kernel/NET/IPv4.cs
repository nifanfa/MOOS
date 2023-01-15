using MOOS;
using System;
using System.Net;
using System.Runtime.InteropServices;

namespace MOOS.NET
{
    public static unsafe class IPv4
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct IPv4Header
        {
            public byte VersionAndIHL;
            public byte DSCPAndECN;
            public ushort TotalLength;
            public ushort ID;
            public ushort FlagAndFragmentOffset;
            public byte TimeToLive;
            public byte Protocol;
            public ushort HeaderChecksum;
            public uint SourceIP;
            public uint DestIP;
        }

        public enum IPv4Protocol
        {
            ICMP = 1,
            TCP = 6,
            UDP = 17,
        }

        internal static unsafe void HandlePacket(byte* data, int length)
        {
            IPv4Header* hdr = (IPv4Header*)data;
            data += sizeof(IPv4Header);
            length -= sizeof(IPv4Header);

            Ethernet.SwapLeftRight(ref hdr->TotalLength);

            if (
                hdr->DestIP == Network.IP.AddressV4
                )
            {
                if (hdr->Protocol == (byte)IPv4Protocol.ICMP)
                {
                    if (data[0] == 8)
                    {
                        byte* p = (byte*)Allocator.Allocate((ulong)length);
                        Native.Movsb(p, data, (ulong)length);
                        p[0] = 0;
                        *(ushort*)(p + 2) = 0;
                        *(ushort*)(p + 2) = CalculateChecksum(p, length);

                        IPAddress srcIP = new IPAddress();
                        srcIP.AddressV4 = hdr->SourceIP;
                        SendPacket(srcIP, 1, p, length);
                        srcIP.Dispose();
                        Allocator.Free((IntPtr)p);
                    }
                }
                else if (hdr->Protocol == (byte)IPv4Protocol.UDP)
                {
                    UDP.HandlePacket(data, length);
                }
                else if (hdr->Protocol == (byte)IPv4Protocol.TCP)
                {
                    TCP.HandlePacket(data, hdr);
                }
            }
        }

        public static bool IsSameSubnet(IPAddress ip1, IPAddress ip2)
        {
            if ((ip1.AddressV4 & Network.Mask.AddressV4) != (ip2.AddressV4 & Network.Mask.AddressV4)) return false;

            return true;
        }

        public static void SendPacket(IPAddress DestIP, byte Protocol, byte* Data, int Length)
        {
            IPv4Header* hdr = (IPv4Header*)Allocator.Allocate((ulong)(sizeof(IPv4Header) + Length));
            hdr->VersionAndIHL = 0x45;
            hdr->TotalLength = Ethernet.SwapLeftRight((uint)(sizeof(IPv4Header) + Length));
            hdr->TimeToLive = 255;
            hdr->Protocol = Protocol;
            hdr->SourceIP = Network.IP.AddressV4;

            hdr->DestIP = DestIP.AddressV4;
            hdr->HeaderChecksum = CalculateChecksum((byte*)hdr, sizeof(IPv4Header));
            Native.Movsb(((byte*)hdr) + sizeof(IPv4Header), Data, (ulong)Length);
            MACAddress MAC = ARP.Lookup(IsSameSubnet(DestIP, Network.IP) ? DestIP : Network.Gateway);
            Ethernet.SendPacket(MAC, (ushort)Ethernet.EthernetType.IPv4, hdr, sizeof(IPv4Header) + Length);
            Allocator.Free((IntPtr)hdr);
        }

        public static ushort CalculateChecksum(byte* addr, int count)
        {
            uint sum = 0;
            ushort* ptr = (ushort*)addr;

            while (count > 1)
            {
                sum += *ptr++;
                count -= 2;
            }

            if (count > 0)
            {
                sum += *(byte*)ptr;
            }

            while ((sum >> 16) != 0)
            {
                sum = (sum & 0xffff) + (sum >> 16);
            }

            return (ushort)~sum;
        }
    }
}