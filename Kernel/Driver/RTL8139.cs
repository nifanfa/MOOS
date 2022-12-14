using MOOS;
using MOOS.Driver;
using MOOS.Misc;
using System.Diagnostics;
using static Native;

namespace MOOS.NET
{
    public unsafe class RTL8139 : NIC
    {
        public static ushort IOBase = 0;
        public static uint RX = 0;
        public static byte IRQ = 0;
        public static MACAddress MAC;
        public static byte[] TSAD;
        public static byte[] TSD;
        private static ushort CurrentPointer = 0;
        private static int TXIndex = 0;

        public static void Initialise()
        {
            TSAD = new byte[] { 0x20, 0x24, 0x28, 0x2C };
            TSD = new byte[] { 0x10, 0x14, 0x18, 0x1C };

            PCIDevice dev = PCI.GetDevice(0x10EC, 0x8139);
            if (dev == null)
            {
                return;
            }
            Console.WriteLine("[RTL8139] RTL8139 Found!");
            IOBase = (ushort)(dev.Bar0 & ~0x3);
            dev.WriteRegister(0x04, 0x04 | 0x02 | 0x01);

            Out8((ushort)(IOBase + 0x52), 0x00);
            Out8((ushort)(IOBase + 0x37), 0x10);
            while ((In8((ushort)(IOBase + 0x37)) & 0x10) != 0) ;
            Console.WriteLine("[RTL8139] Soft-Reset Done");

            uint p = (uint)Allocator.Allocate(8192 + 16 + 1500 + 0xF);
            p = p & ~0xFU;
            Console.Write("[RTL8139] RX is at:");
            Console.WriteLine(((ulong)p).ToStringHex());
            RX = p;

            Out32((ushort)(IOBase + 0x30), (uint)p);

            Out16((ushort)(IOBase + 0x3C), 0x5);
            Out32((ushort)(IOBase + 0x44), 0xF | (1 << 7));
            Out8((ushort)(IOBase + 0x37), 0xC);
            Console.WriteLine("[RTL8139] RTL8139 Configuration D0ne");
            Console.Write("[RTL8139] IRQ:");
            Console.WriteLine(((ulong)dev.IRQ).ToStringHex());

            MAC = new MACAddress(
                In8((ushort)(IOBase + 0)),
                In8((ushort)(IOBase + 1)),
                In8((ushort)(IOBase + 2)),
                In8((ushort)(IOBase + 3)),
                In8((ushort)(IOBase + 4)),
                In8((ushort)(IOBase + 5))
            );
            Console.WriteLine($"[RTL8139] MAC: {MAC}");

            Network.MAC = MAC;
            IRQ = dev.IRQ;
            Interrupts.EnableInterrupt(dev.IRQ,&OnInterrupt);

            //Literally instance
            Network.Controller = new RTL8139();
        }

        public static void OnInterrupt()
        {
            ushort Status = In16((ushort)(IOBase + 0x3E));
            if ((Status & (1 << 2)) != 0)
            {
                //Debug.WriteLine("[RTL8139] Transmit OK");
            }
            if ((Status & (1 << 0)) != 0)
            {
                //Debug.WriteLine("[RTL8139] Receive OK");

                ushort* t = (ushort*)(RX + CurrentPointer);
                ushort length = *(t + 1);
                t += 2;
                length -= 4;

                Ethernet.HandlePacket((byte*)t, length);

                CurrentPointer = (ushort)((CurrentPointer + length + 4 + 3) & ~3);
                Out16((ushort)(IOBase + 0x38), (ushort)(CurrentPointer - 0x10));
            }
            Out16((ushort)(IOBase + 0x3E), 0x05);
        }

        public override void Send(byte* Data, int Length)
        {
            Out32((ushort)(IOBase + TSAD[TXIndex]), (uint)Data);
            Out32((ushort)(IOBase + TSD[TXIndex++]), (uint)Length);
            if (TXIndex > 3) TXIndex = 0;
        }
    }
}