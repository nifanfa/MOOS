using System.Runtime.InteropServices;

namespace Kernel.NET
{
    public static unsafe class Ethernet
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct EthernetHeader
        {
            public fixed byte DestMAC[6];
            public fixed byte SrcMAC[6];
            public ushort EthernetType;
        }

        public enum EthernetType
        {
            IPv4 = 0x0800,
            IPv6 = 0x86DD,
            ARP = 0x0806,
        }

        public static ushort SwapLeftRight(uint Value)
        {
            return ((ushort)((((Value) & 0xff) << 8) | (((Value) & 0xff00) >> 8)));
        }

        public static void HandlePacket(byte* Data, int Length)
        {
            EthernetHeader* hdr = (EthernetHeader*)Data;
            Data += sizeof(EthernetHeader);
            Length -= sizeof(EthernetHeader);
            if (SwapLeftRight(hdr->EthernetType) == (ushort)EthernetType.ARP) ARP.HandlePacket(Data, Length);
            if (SwapLeftRight(hdr->EthernetType) == (ushort)EthernetType.IPv4) IPv4.HandlePacket(Data, Length);
        }

        internal static void SendPacket(byte[] DestMAC, ushort Type, void* Data, int Length)
        {
            ulong p = (ulong)Memory.Allocate((ulong)(Length + sizeof(EthernetHeader)));
            EthernetHeader* hdr = (EthernetHeader*)p;
            hdr->DestMAC[0] = DestMAC[0];
            hdr->DestMAC[1] = DestMAC[1];
            hdr->DestMAC[2] = DestMAC[2];
            hdr->DestMAC[3] = DestMAC[3];
            hdr->DestMAC[4] = DestMAC[4];
            hdr->DestMAC[5] = DestMAC[5];
            hdr->SrcMAC[0] = Network.MAC[0];
            hdr->SrcMAC[1] = Network.MAC[1];
            hdr->SrcMAC[2] = Network.MAC[2];
            hdr->SrcMAC[3] = Network.MAC[3];
            hdr->SrcMAC[4] = Network.MAC[4];
            hdr->SrcMAC[5] = Network.MAC[5];
            hdr->EthernetType = SwapLeftRight(Type);
            Native.Movsb((void*)(p + (ulong)sizeof(EthernetHeader)), Data, (ulong)Length);
            //TODO Disposing
            RTL8139.Send((byte*)p, Length + sizeof(EthernetHeader));
        }

        public static uint SwapLeftRight32(uint x)
        {
            return ((x & 0x000000ff) << 24) +
              ((x & 0x0000ff00) << 8) +
              ((x & 0x00ff0000) >> 8) +
              ((x & 0xff000000) >> 24);
        }
    }
}
