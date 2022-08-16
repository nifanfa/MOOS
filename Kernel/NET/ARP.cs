using MOOS;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;

namespace MOOS.NET
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
            public MACAddress SourceMAC;
            public uint SourceIP;
            public MACAddress DestMAC;
            public uint DestIP;
        }

        public enum ARPOperation
        {
            Request = 1,
            Reply = 2
        }

        public class ARPEntry
        {
            public IPAddress IP;
            public MACAddress MAC;
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
                IPAddress IP = new IPAddress();
                IP.AddressV4 = hdr->SourceIP;
                MACAddress MAC = hdr->SourceMAC;
                ARPEntry entry = new ARPEntry() { IP = IP, MAC = MAC };
                if (!ARP.Exist(IP))
                {
                    ARPEntries.Add(entry);
                }
            }
            if (
                hdr->DestIP == Network.IP.AddressV4
                )
            {
                if (Ethernet.SwapLeftRight(hdr->Operation) == (ushort)ARPOperation.Request)
                {
                    ARPHeader* _hdr = (ARPHeader*)Allocator.Allocate((ulong)(sizeof(ARPHeader)));
                    Native.Movsb(_hdr, hdr, (ulong)(sizeof(ARPHeader)));

                    _hdr->Operation = Ethernet.SwapLeftRight((uint)ARPOperation.Reply);
                    _hdr->DestMAC = _hdr->SourceMAC;
                    _hdr->DestIP = _hdr->SourceIP;
                    _hdr->SourceMAC = Network.MAC;
                    _hdr->SourceIP = Network.IP.AddressV4;
                    MACAddress DestMAC = _hdr->DestMAC;
                    Ethernet.SendPacket(DestMAC, (ushort)Ethernet.EthernetType.ARP, _hdr, sizeof(ARPHeader));
                    Allocator.Free((IntPtr)_hdr);
                }
            }
        }

        public static bool Exist(IPAddress destIP) 
        {
            for (int i = 0; i < ARPEntries.Count; i++)
            {
                if (
                    ARPEntries[i].IP.AddressV4 == destIP.AddressV4
                    ) return true;
            }
            ARP.Require(destIP);
            return false;
        }

        internal static MACAddress Lookup(IPAddress destIP)
        {

            for (; ; )
            {
                for (int i = 0; i < ARPEntries.Count; i++)
                {
                    if (
                        ARPEntries[i].IP.AddressV4 == destIP.AddressV4
                        )
                    {
                        return ARPEntries[i].MAC;
                    }
                }
            }
        }

        internal static void Require(IPAddress IP)
        {
            ARPHeader* hdr = (ARPHeader*)Allocator.Allocate((ulong)(sizeof(ARPHeader)));
            hdr->SourceMAC = Network.MAC;
            hdr->DestMAC = Network.Boardcast;
            hdr->SourceIP = Network.IP.AddressV4;
            hdr->DestIP = IP.AddressV4;
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