/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
//Reference: https://www.intel.com/content/dam/doc/manual/pci-pci-x-family-gbe-controllers-software-dev-manual.pdf

using MOOS.Misc;
using MOOS.NET;
using System.Runtime.InteropServices;
using static MOOS.Misc.MMIO;

namespace MOOS.Driver
{
    public unsafe class Intel8254X
    {
        public static uint BAR0;
        public static uint RXDescs;
        public static uint TXDescs;

        public static byte[] MAC;
        public static int IRQ;

        public static bool FullDuplex
        {
            get
            {
                return (ReadRegister(8) & (1 << 0)) != 0;
            }
        }
        public static int Speed
        {
            get
            {
                if ((ReadRegister(8) & (3 << 6)) == 0)
                {
                    return 10;
                }
                if ((ReadRegister(8) & (2 << 6)) != 0)
                {
                    return 1000;
                }
                if ((ReadRegister(8) & (1 << 6)) != 0)
                {
                    return 100;
                }
                return 0;
            }
        }

        public static void Initialize()
        {
            PCIDevice device = null;

            for (int i = 0; i < PCI.Devices.Count; i++)
            {
                if (
                    PCI.Devices[i] != null &&
                    PCI.Devices[i].VendorID == 0x8086 &&
                        (
                            PCI.Devices[i].DeviceID == 0x100e ||
                            PCI.Devices[i].DeviceID == 0x1004 ||
                            PCI.Devices[i].DeviceID == 0x100f ||
                            PCI.Devices[i].DeviceID == 0x10ea ||
                            PCI.Devices[i].DeviceID == 0x10d3
                        )
                    )
                {
                    device = PCI.Devices[i];
                }
            }

            if (device == null) return;

            Console.WriteLine("Intel 8254X Series Gigabit Ethernet Controller Found");
            device.WriteRegister(0x04, 0x04 | 0x02 | 0x01);

            BAR0 = (uint)(device.Bar0 & (~0xF));
            Console.Write("BAR0: 0x");
            Console.WriteLine(((ulong)BAR0).ToStringHex());

            WriteRegister(0x14, 0x1);
            bool HasEEPROM = false;
            for (int i = 0; i < 1024; i++)
            {
                if ((ReadRegister(0x14) & 0x10) != 0)
                {
                    HasEEPROM = true;
                    break;
                }
            }

            MAC = new byte[6];

            //Must be set
            if (!HasEEPROM)
            {
                MAC[0] = In8((byte*)(BAR0 + 0x5400));
                MAC[1] = In8((byte*)(BAR0 + 0x5401));
                MAC[2] = In8((byte*)(BAR0 + 0x5402));
                MAC[3] = In8((byte*)(BAR0 + 0x5403));
                MAC[4] = In8((byte*)(BAR0 + 0x5404));
                MAC[5] = In8((byte*)(BAR0 + 0x5405));
                Console.WriteLine("This controller has no EEPROM");
            }
            else
            {
                MAC[0] = (byte)(ReadROM(0) & 0xFF);
                MAC[1] = (byte)(ReadROM(0) >> 8);
                MAC[2] = (byte)(ReadROM(1) & 0xFF);
                MAC[3] = (byte)(ReadROM(1) >> 8);
                MAC[4] = (byte)(ReadROM(2) & 0xFF);
                MAC[5] = (byte)(ReadROM(2) >> 8);
                Console.WriteLine("EEPROM on this controller");
            }

            Console.Write("MAC:");
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

            Linkup();
            for (int i = 0; i < 0x80; i++)
                WriteRegister((ushort)(0x5200 + i * 4), 0);

            Console.Write("IRQ: ");
            Console.WriteLine(((ulong)device.IRQ).ToString("x2"));

            RXInitialize();
            TXInitialize();

            WriteRegister(0x00D0, 0x1F6DC);
            WriteRegister(0x00D0, 0xFF & ~4);
            ReadRegister(0xC0);

            Console.Write("Speed: ");
            Console.Write(((ulong)Speed).ToString());
            Console.Write(' ');
            Console.Write("FullDuplex: ");
            Console.WriteLine(FullDuplex?"Yes":"No");
            Console.WriteLine("Configuration Done");

            Network.MAC = MAC;
            Interrupts.EnableInterrupt(device.IRQ);
            IRQ = device.IRQ;
        }

        private static void TXInitialize()
        {
            TXDescs = (uint)Allocator.Allocate(8 * 16);

            for (int i = 0; i < 8; i++)
            {
                TXDesc* desc = (TXDesc*)(TXDescs + (i * 16));
                desc->addr = 0;
                desc->cmd = 0;
            }

            WriteRegister(0x3800, TXDescs);
            WriteRegister(0x3804, 0);
            WriteRegister(0x3808, 8 * 16);
            WriteRegister(0x3810, 0);
            WriteRegister(0x3818, 0);

            WriteRegister(0x0400, (1 << 1) | (1 << 3));
        }

        public static uint RXCurr = 0;
        public static uint TXCurr = 0;

        private static void RXInitialize()
        {
            RXDescs = (uint)Allocator.Allocate(32 * 16);

            for (uint i = 0; i < 32; i++)
            {
                RXDesc* desc = (RXDesc*)(RXDescs + (i * 16));
                desc->addr = (ulong)(void*)Allocator.Allocate(2048 + 16);
                desc->status = 0;
            }

            WriteRegister(0x2800, RXDescs);
            WriteRegister(0x2804, 0);

            WriteRegister(0x2808, 32 * 16);
            WriteRegister(0x2810, 0);
            WriteRegister(0x2818, 32 - 1);

            WriteRegister(0x0100,
                     (1 << 1) |
                     (1 << 2) |
                     (1 << 3) |
                     (1 << 4) |
                     (0 << 6) |
                     (0 << 8) |
                    (1 << 15) |
                    (1 << 26) |
                    (0 << 16)
                );
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RXDesc
        {
            public ulong addr;
            public ushort length;
            public ushort checksum;
            public byte status;
            public byte errors;
            public ushort special;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TXDesc
        {
            public ulong addr;
            public ushort length;
            public byte cso;
            public byte cmd;
            public byte status;
            public byte css;
            public ushort special;
        }

        public static void WriteRegister(ushort Reg, uint Val)
        {
            Out32((uint*)(BAR0 + Reg), Val);
        }

        public static uint ReadRegister(ushort Reg)
        {
            return In32((uint*)(BAR0 + Reg));
        }

        public static ushort ReadROM(uint Addr)
        {
            uint Temp;
            WriteRegister(0x14, 1 | (Addr << 8));
            while (((Temp = ReadRegister(0x14)) & 0x10) == 0) ;
            return ((ushort)((Temp >> 16) & 0xFFFF));
        }

        public static void OnInterrupt()
        {
            uint Status = ReadRegister(0xC0);

            if ((Status & 0x04) != 0)
            {
                //Console.WriteLine("Linking Up");
                Linkup();
            }
            if ((Status & 0x10) != 0)
            {
                //Console.WriteLine("Good Threshold");
            }

            if ((Status & 0x80) != 0)
            {
                //Console.WriteLine("Packet Received");
                uint _RXCurr = RXCurr;
                RXDesc* desc = (RXDesc*)(RXDescs + (RXCurr * 16));
                while ((desc->status & 0x1) != 0)
                {
                    Ethernet.HandlePacket((byte*)desc->addr, desc->length);
                    //desc->addr;
                    desc->status = 0;
                    RXCurr = (RXCurr + 1) % 32;
                    WriteRegister(0x2818, _RXCurr);
                }
            }
        }

        private static void Linkup()
        {
            WriteRegister(0, ReadRegister(0) | 0x40);
        }

        public static void Send(byte* Buffer, int Length)
        {
            TXDesc* desc = (TXDesc*)(TXDescs + (TXCurr * 16));
            desc->addr = (ulong)Buffer;
            desc->length = (ushort)Length;
            desc->cmd = (1 << 0) | (1 << 1) | (1 << 3);
            desc->status = 0;

            byte _TXCurr = (byte)TXCurr;
            TXCurr = (TXCurr + 1) % 8;
            WriteRegister(0x3818, TXCurr);
            while ((desc->status & 0xff) == 0) ;
        }
    }
}