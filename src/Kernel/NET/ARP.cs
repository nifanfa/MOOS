using Kernel;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Kernel.NET
{
    public static unsafe class ARP
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ARPHeader
        {
            public ushort HardwareType;
            public ushort Protocol;
            public byte HardwareAddrLen;
            public byte ProtocolAddrLen;
            public ushort Operation;
            public fixed byte SourceMAC[6];
            public fixed byte SourceIP[4];
            public fixed byte DestMAC[6];
            public fixed byte DestIP[4];
        }

        public enum ARPOperation
        {
            Request = 1,
            Reply = 2
        }

        public struct ARPEntry
        {
            public byte[] IP;
            public byte[] MAC;
        }

        public static List<ARPEntry> ARPEntries;

        public static void Initialise() 
        {
            ARPEntries = new List<ARPEntry>(32);
        }

        internal static void HandlePacket(byte* data, int length)
        {
            ARPHeader* hdr = (ARPHeader*)data;

            if (Ethernet.SwapLeftRight(hdr->Operation) == (ushort)ARPOperation.Reply)
            {
                byte[] IP = new byte[4];
                IP[0] = hdr->SourceIP[0];
                IP[1] = hdr->SourceIP[1];
                IP[2] = hdr->SourceIP[2];
                IP[3] = hdr->SourceIP[3];
                byte[] MAC = new byte[6];
                MAC[0] = hdr->SourceMAC[0];
                MAC[1] = hdr->SourceMAC[1];
                MAC[2] = hdr->SourceMAC[2];
                MAC[3] = hdr->SourceMAC[3];
                MAC[4] = hdr->SourceMAC[4];
                MAC[5] = hdr->SourceMAC[5];
                ARPEntry entry = new ARPEntry() { IP = IP, MAC = MAC };
                ARPEntries.Add(entry);
                Console.Write(((ulong)IP[0]).ToString());
                Console.Write(".");
                Console.Write(((ulong)IP[1]).ToString());
                Console.Write(".");
                Console.Write(((ulong)IP[2]).ToString());
                Console.Write(".");
                Console.Write(((ulong)IP[3]).ToString());
                Console.Write(" -> ");
                Console.Write(((ulong)MAC[0]).ToStringHex());
                Console.Write(":");
                Console.Write(((ulong)MAC[1]).ToStringHex());
                Console.Write(":");
                Console.Write(((ulong)MAC[2]).ToStringHex());
                Console.Write(":");
                Console.Write(((ulong)MAC[3]).ToStringHex());
                Console.Write(":");
                Console.Write(((ulong)MAC[4]).ToStringHex());
                Console.Write(":");
                Console.WriteLine(((ulong)MAC[5]).ToStringHex());
            }
            if (
                hdr->DestIP[0] == Network.IP[0] &&
                hdr->DestIP[1] == Network.IP[1] &&
                hdr->DestIP[2] == Network.IP[2] &&
                hdr->DestIP[3] == Network.IP[3]
                )
            {
                if (Ethernet.SwapLeftRight(hdr->Operation) == (ushort)ARPOperation.Request)
                {
                    ARPHeader* _hdr = (ARPHeader*)Allocator.Allocate((ulong)(sizeof(ARPHeader)));
                    Native.Movsb(_hdr, hdr, (ulong)(sizeof(ARPHeader)));

                    _hdr->Operation = Ethernet.SwapLeftRight((uint)ARPOperation.Reply);
                    _hdr->DestMAC[0] = _hdr->SourceMAC[0];
                    _hdr->DestMAC[1] = _hdr->SourceMAC[1];
                    _hdr->DestMAC[2] = _hdr->SourceMAC[2];
                    _hdr->DestMAC[3] = _hdr->SourceMAC[3];
                    _hdr->DestMAC[4] = _hdr->SourceMAC[4];
                    _hdr->DestMAC[5] = _hdr->SourceMAC[5];
                    _hdr->DestIP[0] = _hdr->SourceIP[0];
                    _hdr->DestIP[1] = _hdr->SourceIP[1];
                    _hdr->DestIP[2] = _hdr->SourceIP[2];
                    _hdr->DestIP[3] = _hdr->SourceIP[3];
                    _hdr->SourceMAC[0] = Network.MAC[0];
                    _hdr->SourceMAC[1] = Network.MAC[1];
                    _hdr->SourceMAC[2] = Network.MAC[2];
                    _hdr->SourceMAC[3] = Network.MAC[3];
                    _hdr->SourceMAC[4] = Network.MAC[4];
                    _hdr->SourceMAC[5] = Network.MAC[5];
                    _hdr->SourceIP[0] = Network.IP[0];
                    _hdr->SourceIP[1] = Network.IP[1];
                    _hdr->SourceIP[2] = Network.IP[2];
                    _hdr->SourceIP[3] = Network.IP[3];
                    byte[] DestMAC = new byte[6];
                    DestMAC[0] = _hdr->DestMAC[0];
                    DestMAC[1] = _hdr->DestMAC[1];
                    DestMAC[2] = _hdr->DestMAC[2];
                    DestMAC[3] = _hdr->DestMAC[3];
                    DestMAC[4] = _hdr->DestMAC[4];
                    DestMAC[5] = _hdr->DestMAC[5];
                    Ethernet.SendPacket(DestMAC, (ushort)Ethernet.EthernetType.ARP, _hdr, sizeof(ARPHeader));
                    DestMAC.Dispose();
                    Allocator.Free((IntPtr)_hdr);
                }
            }
        }

        internal static byte[] Lookup(byte[] destIP)
        {
            byte[] MAC = null;
            while(MAC == null) 
            {
                for (int i = 0; i < ARPEntries.Count; i++)
                {
                    if (
                        ARPEntries[i].IP[0] == destIP[0] &&
                        ARPEntries[i].IP[1] == destIP[1] &&
                        ARPEntries[i].IP[2] == destIP[2] &&
                        ARPEntries[i].IP[3] == destIP[3]
                        ) return ARPEntries[i].MAC;
                }
                Console.WriteLine("ARP entry not found! Making requests");
                ARP.Require(destIP);
            }
            return null;
        }

        internal static void Require(byte[] IP)
        {
            ARPHeader* hdr = (ARPHeader*)Allocator.Allocate((ulong)(sizeof(ARPHeader)));
            hdr->SourceMAC[0] = Network.MAC[0];
            hdr->SourceMAC[1] = Network.MAC[1];
            hdr->SourceMAC[2] = Network.MAC[2];
            hdr->SourceMAC[3] = Network.MAC[3];
            hdr->SourceMAC[4] = Network.MAC[4];
            hdr->SourceMAC[5] = Network.MAC[5];
            hdr->DestMAC[0] = Network.Boardcast[0];
            hdr->DestMAC[1] = Network.Boardcast[1];
            hdr->DestMAC[2] = Network.Boardcast[2];
            hdr->DestMAC[3] = Network.Boardcast[3];
            hdr->DestMAC[4] = Network.Boardcast[4];
            hdr->DestMAC[5] = Network.Boardcast[5];
            hdr->SourceIP[0] = Network.IP[0];
            hdr->SourceIP[1] = Network.IP[1];
            hdr->SourceIP[2] = Network.IP[2];
            hdr->SourceIP[3] = Network.IP[3];
            hdr->DestIP[0] = IP[0];
            hdr->DestIP[1] = IP[1];
            hdr->DestIP[2] = IP[2];
            hdr->DestIP[3] = IP[3];
            hdr->Operation = Ethernet.SwapLeftRight((uint)ARPOperation.Request);
            hdr->HardwareAddrLen = 6;
            hdr->ProtocolAddrLen = 4;
            hdr->HardwareType = Ethernet.SwapLeftRight(1);
            hdr->Protocol = Ethernet.SwapLeftRight((uint)Ethernet.EthernetType.IPv4);
            Ethernet.SendPacket(Network.Boardcast, (ushort)Ethernet.EthernetType.ARP, hdr, sizeof(ARPHeader));
            Allocator.Free((IntPtr)hdr);
        }
    }
}
