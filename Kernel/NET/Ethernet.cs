using System.Runtime.InteropServices;

namespace MOOS.NET
{
    public static unsafe class Ethernet
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct EthernetHeader
        {
            public MACAddress DestMAC;
            public MACAddress SrcMAC;
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

        public static void SwapLeftRight(ref ushort val)
        {
            uint prev = val;
            val = ((ushort)((((prev) & 0xff) << 8) | (((prev) & 0xff00) >> 8)));
        }

        public static void HandlePacket(byte* Data, int Length)
        {
            Native.Cli();
            EthernetHeader* hdr = (EthernetHeader*)Data;
            Data += sizeof(EthernetHeader);
            Length -= sizeof(EthernetHeader);
            if (SwapLeftRight(hdr->EthernetType) == (ushort)EthernetType.ARP) ARP.HandlePacket(Data, Length);
            if (SwapLeftRight(hdr->EthernetType) == (ushort)EthernetType.IPv4) IPv4.HandlePacket(Data, Length);
            Native.Sti();
        }

        internal static void SendPacket(MACAddress DestMAC, ushort Type, void* Data, int Length)
        {
            ulong p = (ulong)Allocator.Allocate((ulong)(Length + sizeof(EthernetHeader)));
            EthernetHeader* hdr = (EthernetHeader*)p;
            hdr->DestMAC = DestMAC;
            hdr->SrcMAC = Network.MAC;
            hdr->EthernetType = SwapLeftRight(Type);
            Native.Movsb((void*)(p + (ulong)sizeof(EthernetHeader)), Data, (ulong)Length);
            //TODO Disposing
            Network.Controller.Send((byte*)p, Length + sizeof(EthernetHeader));
        }

        public static uint SwapLeftRight32(uint x)
        {
            return ((x & 0x000000ff) << 24) +
              ((x & 0x0000ff00) << 8) +
              ((x & 0x00ff0000) >> 8) +
              ((x & 0xff000000) >> 24);
        }

        public static void SwapLeftRight32(ref uint val)
        {
            uint prev = val;
            val = ((prev & 0x000000ff) << 24) +
              ((prev & 0x0000ff00) << 8) +
              ((prev & 0x00ff0000) >> 8) +
              ((prev & 0xff000000) >> 24);
        }
    }
}