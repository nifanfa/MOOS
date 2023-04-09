//Realtek 8168/8169/8111 series NIC driver
//https://people.freebsd.org/~wpaul/RealTek/RTL8111B_8168B_Registers_DataSheet_1.0.pdf
//TO-DO: how to power on the controller?

using MOOS.Driver;
using MOOS.Misc;
using System;
using System.Runtime.InteropServices;
using static MOOS.NETv4;

namespace MOOS
{
    internal unsafe class RTL8111
    {
        static uint IOBase;

        const int NumRX = 2048;
        const int NumTX = 32;

        const int RXBufferSize = 2048 + 16;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct DMADescriptor
        {
            public uint command;
            public uint vlan;
            public ulong buffer;
        };

        static DMADescriptor* RXDescs;
        static DMADescriptor* TXDescs;

        public static void Initialize()
        {
            PCIDevice dev = null;

            for (int i = 0; i < PCI.Devices.Count; i++)
            {
                if (
                     PCI.Devices[i] != null &&
                     PCI.Devices[i].VendorID == 0x10EC &&
                     (
                        PCI.Devices[i].DeviceID == 0x8161 ||
                        PCI.Devices[i].DeviceID == 0x8168 ||
                        PCI.Devices[i].DeviceID == 0x8169
                      )
                    )
                {
                    dev = PCI.Devices[i];
                }
            }

            if (dev == null) return;
            Console.Write("[RTL8111] device found!\n");

            dev.WriteRegister(0x04, 0x04 | 0x02 | 0x01);

            IOBase = dev.Bar0 & ~1u;

            WriteRegister8(0x52, 0);

            WriteRegister8(0x37, 0x10);
            while ((ReadRegister8(0x37) & 0x10) != 0) ;

            NETv4.MAC = new NETv4.MACAddress
                (
                ReadRegister8(0),
                ReadRegister8(1),
                ReadRegister8(2),
                ReadRegister8(3),
                ReadRegister8(4),
                ReadRegister8(5)
                );

            RXDescs = Allocator.ClearAllocate<DMADescriptor>(NumRX);
            TXDescs = Allocator.ClearAllocate<DMADescriptor>(NumTX);

            for (int i = 0; i < NumRX; i++)
            {
                ResetRXDesc(i);
                RXDescs[i].buffer = (ulong)Allocator.Allocate(RXBufferSize);
            }

            WriteRegister8(0x50, 0xC0);
            WriteRegister32(0x44, 0x0000E73F);
            WriteRegister8(0x37, 0x04);
            WriteRegister32(0x40, 0x03000700);
            WriteRegister16(0xDA, 0x1FFF);
            WriteRegister8(0xEC, 0x3B);
            WriteRegister32(0x20, (uint)(ulong)TXDescs);
            WriteRegister32(0x24, (uint)(((ulong)TXDescs) >> 32));
            WriteRegister32(0xE4, (uint)(ulong)RXDescs);
            WriteRegister32(0xE8, (uint)(((ulong)RXDescs) >> 32));
            //Note: this setting is wrong becasue it only enabled
            //ROK(receive ok) interrupt but all interrupts 
            //containing TOK and so on works fine.
            //Other value except 0 and 1 makes system freeze idk why.
            WriteRegister16(0x3C, 1);
            WriteRegister8(0x37, 0x0C);
            WriteRegister8(0x50, 0x00);

            Interrupts.EnableInterrupt(dev.IRQ, &OnInterrupt);

            Console.Write("[RTL8111] device initialized\n");

            NETv4.Sender = &Send;
        }

        private static void ResetRXDesc(int i)
        {
            RXDescs[i].command = (1u << 31) | (RXBufferSize & 0x3FFF);
            if (i == (NumRX - 1))
            {
                RXDescs[i].command |= (1u << 30);
            }
        }

        public static void Send(byte* buffer, int length)
        {
            DMADescriptor* desc = &TXDescs[0];
            desc->buffer = (ulong)buffer;
            desc->command = (uint)((1 << 31) | (1 << 30) | (1 << 28) | (length & 0x3FFF));
            WriteRegister8(0x38, 0x40);
            for (; ; )
            {
                if ((ReadRegister8(0x38) & 0x40) == 0) break;
                ACPITimer.Sleep(1);
            }
        }

        public static void OnInterrupt()
        {
            ushort status = ReadRegister16(0x3E);
            if ((status & 0x20) != 0)
            {
            }
            if ((status & 0x01) != 0)
            {
                for (int i = NumRX - 1; i >= 0; i--)
                {
                    if (!BitHelpers.IsBitSet(RXDescs[i].command, 31))
                    {
                        NETv4.OnData((byte*)RXDescs[i].buffer);
                        ResetRXDesc(i);
                    }
                }
            }
            if ((status & 0x04) != 0)
            {
            }
            WriteRegister16(0x3E, status);
        }

        public static byte ReadRegister8(uint reg) => Native.In8((ushort)(IOBase + reg));

        public static ushort ReadRegister16(uint reg) => Native.In16((ushort)(IOBase + reg));

        public static uint ReadRegister32(uint reg) => Native.In32((ushort)(IOBase + reg));

        public static void WriteRegister8(uint reg, byte value) => Native.Out8((ushort)(IOBase + reg), value);

        public static void WriteRegister16(uint reg, ushort value) => Native.Out16((ushort)(IOBase + reg), value);

        public static void WriteRegister32(uint reg, uint value) => Native.Out32((ushort)(IOBase + reg), value);
    }
}
