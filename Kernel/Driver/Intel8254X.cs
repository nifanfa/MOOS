//Reference: https://www.intel.com/content/dam/doc/manual/pci-pci-x-family-gbe-controllers-software-dev-manual.pdf

using MOOS.Misc;
using MOOS.NET;
using System;
using System.Runtime.InteropServices;
using static MOOS.Misc.MMIO;

namespace MOOS.Driver
{
    public unsafe class Intel8254X : NIC
    {
        public static uint BAR0;
        public static uint RXDescs;
        public static uint TXDescs;
        public static MACAddress MAC;
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
                        PCI.Devices[i].DeviceID == 0x1000 || //Intel82542
                        PCI.Devices[i].DeviceID == 0x1001 || //Intel82543GC
                        PCI.Devices[i].DeviceID == 0x1004 || //Intel82543GC
                        PCI.Devices[i].DeviceID == 0x1008 || //Intel82544EI
                        PCI.Devices[i].DeviceID == 0x1009 || //Intel82544EI
                        PCI.Devices[i].DeviceID == 0x100C || //Intel82543EI
                        PCI.Devices[i].DeviceID == 0x100D || //Intel82544GC
                        PCI.Devices[i].DeviceID == 0x100E || //Intel82540EM
                        PCI.Devices[i].DeviceID == 0x100F || //Intel82545EM
                        PCI.Devices[i].DeviceID == 0x1010 || //Intel82546EB
                        PCI.Devices[i].DeviceID == 0x1011 || //Intel82545EM
                        PCI.Devices[i].DeviceID == 0x1012 || //Intel82546EB
                        PCI.Devices[i].DeviceID == 0x1013 || //Intel82541EI
                        PCI.Devices[i].DeviceID == 0x1014 || //Intel82541ER
                        PCI.Devices[i].DeviceID == 0x1015 || //Intel82540EM
                        PCI.Devices[i].DeviceID == 0x1016 || //Intel82540EP
                        PCI.Devices[i].DeviceID == 0x1017 || //Intel82540EP
                        PCI.Devices[i].DeviceID == 0x1018 || //Intel82541EI
                        PCI.Devices[i].DeviceID == 0x1019 || //Intel82547EI
                        PCI.Devices[i].DeviceID == 0x101A || //Intel82547EI
                        PCI.Devices[i].DeviceID == 0x101D || //Intel82546EB
                        PCI.Devices[i].DeviceID == 0x101E || //Intel82540EP
                        PCI.Devices[i].DeviceID == 0x1026 || //Intel82545GM
                        PCI.Devices[i].DeviceID == 0x1027 || //Intel82545GM
                        PCI.Devices[i].DeviceID == 0x1028 || //Intel82545GM
                        PCI.Devices[i].DeviceID == 0x1049 || //Intel82566MM_ICH8
                        PCI.Devices[i].DeviceID == 0x104A || //Intel82566DM_ICH8
                        PCI.Devices[i].DeviceID == 0x104B || //Intel82566DC_ICH8
                        PCI.Devices[i].DeviceID == 0x104C || //Intel82562V_ICH8
                        PCI.Devices[i].DeviceID == 0x104D || //Intel82566MC_ICH8
                        PCI.Devices[i].DeviceID == 0x105E || //Intel82571EB
                        PCI.Devices[i].DeviceID == 0x105F || //Intel82571EB
                        PCI.Devices[i].DeviceID == 0x1060 || //Intel82571EB
                        PCI.Devices[i].DeviceID == 0x1075 || //Intel82547EI
                        PCI.Devices[i].DeviceID == 0x1076 || //Intel82541GI
                        PCI.Devices[i].DeviceID == 0x1077 || //Intel82547EI
                        PCI.Devices[i].DeviceID == 0x1078 || //Intel82541ER
                        PCI.Devices[i].DeviceID == 0x1079 || //Intel82546EB
                        PCI.Devices[i].DeviceID == 0x107A || //Intel82546EB
                        PCI.Devices[i].DeviceID == 0x107B || //Intel82546EB
                        PCI.Devices[i].DeviceID == 0x107C || //Intel82541PI
                        PCI.Devices[i].DeviceID == 0x107D || //Intel82572EI
                        PCI.Devices[i].DeviceID == 0x107E || //Intel82572EI
                        PCI.Devices[i].DeviceID == 0x107F || //Intel82572EI
                        PCI.Devices[i].DeviceID == 0x108A || //Intel82546GB
                        PCI.Devices[i].DeviceID == 0x108B || //Intel82573E
                        PCI.Devices[i].DeviceID == 0x108C || //Intel82573E
                        PCI.Devices[i].DeviceID == 0x1096 || //Intel80003ES2LAN
                        PCI.Devices[i].DeviceID == 0x1098 || //Intel80003ES2LAN
                        PCI.Devices[i].DeviceID == 0x1099 || //Intel82546GB
                        PCI.Devices[i].DeviceID == 0x109A || //Intel82573L
                        PCI.Devices[i].DeviceID == 0x10A4 || //Intel82571EB
                        PCI.Devices[i].DeviceID == 0x10A7 || //Intel82575
                        PCI.Devices[i].DeviceID == 0x10A9 || //Intel82575_serdes
                        PCI.Devices[i].DeviceID == 0x10B5 || //Intel82546GB
                        PCI.Devices[i].DeviceID == 0x10B9 || //Intel82572EI
                        PCI.Devices[i].DeviceID == 0x10BA || //Intel80003ES2LAN
                        PCI.Devices[i].DeviceID == 0x10BB || //Intel80003ES2LAN
                        PCI.Devices[i].DeviceID == 0x10BC || //Intel82571EB
                        PCI.Devices[i].DeviceID == 0x10BD || //Intel82566DM_ICH9
                        PCI.Devices[i].DeviceID == 0x10C4 || //Intel82562GT_ICH8
                        PCI.Devices[i].DeviceID == 0x10C5 || //Intel82562G_ICH8
                        PCI.Devices[i].DeviceID == 0x10C9 || //Intel82576
                        PCI.Devices[i].DeviceID == 0x10D3 || //Intel82574L
                        PCI.Devices[i].DeviceID == 0x10A9 || //Intel82575_quadcopper
                        PCI.Devices[i].DeviceID == 0x10CB || //Intel82567V_ICH9
                        PCI.Devices[i].DeviceID == 0x10E5 || //Intel82567LM_4_ICH9
                        PCI.Devices[i].DeviceID == 0x10EA || //Intel82577LM
                        PCI.Devices[i].DeviceID == 0x10EB || //Intel82577LC
                        PCI.Devices[i].DeviceID == 0x10EF || //Intel82578DM
                        PCI.Devices[i].DeviceID == 0x10F0 || //Intel82578DC
                        PCI.Devices[i].DeviceID == 0x10F5 || //Intel82567LM_ICH9_egDellE6400Notebook
                        PCI.Devices[i].DeviceID == 0x1502 || //Intel82579LM
                        PCI.Devices[i].DeviceID == 0x1503 || //Intel82579V
                        PCI.Devices[i].DeviceID == 0x150A || //Intel82576NS
                        PCI.Devices[i].DeviceID == 0x150E || //Intel82580
                        PCI.Devices[i].DeviceID == 0x1521 || //IntelI350
                        PCI.Devices[i].DeviceID == 0x1533 || //IntelI210
                        PCI.Devices[i].DeviceID == 0x157B || //IntelI210
                        PCI.Devices[i].DeviceID == 0x153A || //IntelI217LM
                        PCI.Devices[i].DeviceID == 0x153B || //IntelI217VA
                        PCI.Devices[i].DeviceID == 0x1559 || //IntelI218V
                        PCI.Devices[i].DeviceID == 0x155A || //IntelI218LM
                        PCI.Devices[i].DeviceID == 0x15A0 || //IntelI218LM2
                        PCI.Devices[i].DeviceID == 0x15A1 || //IntelI218V
                        PCI.Devices[i].DeviceID == 0x15A2 || //IntelI218LM3
                        PCI.Devices[i].DeviceID == 0x15A3 || //IntelI218V3
                        PCI.Devices[i].DeviceID == 0x156F || //IntelI219LM
                        PCI.Devices[i].DeviceID == 0x1570 || //IntelI219V
                        PCI.Devices[i].DeviceID == 0x15B7 || //IntelI219LM2
                        PCI.Devices[i].DeviceID == 0x15B8 || //IntelI219V2
                        PCI.Devices[i].DeviceID == 0x15BB || //IntelI219LM3
                        PCI.Devices[i].DeviceID == 0x15D7 || //IntelI219LM
                        PCI.Devices[i].DeviceID == 0x15E3    //IntelI219LM
                        )
                    )
                {
                    device = PCI.Devices[i];
                }
            }

            if (device == null) return;

            Console.WriteLine("[Intel8254X] Intel 8254X Series Gigabit Ethernet Controller Found");
            device.WriteRegister(0x04, 0x04 | 0x02 | 0x01);

            BAR0 = (uint)(device.Bar0 & (~0xF));
            Console.Write("[Intel8254X] BAR0: 0x");
            Console.WriteLine(((ulong)BAR0).ToStringHex());

            Reset();

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

            //Must be set
            if (!HasEEPROM)
            {
                MAC = new MACAddress(
                    In8((byte*)(BAR0 + 0x5400)),
                    In8((byte*)(BAR0 + 0x5401)),
                    In8((byte*)(BAR0 + 0x5402)),
                    In8((byte*)(BAR0 + 0x5403)),
                    In8((byte*)(BAR0 + 0x5404)),
                    In8((byte*)(BAR0 + 0x5405))
                    );
                Console.WriteLine("[Intel8254X] This controller has no EEPROM");
            }
            else
            {
                MAC = new MACAddress(
                    (byte)(ReadROM(0) & 0xFF),
                    (byte)(ReadROM(0) >> 8),
                    (byte)(ReadROM(1) & 0xFF),
                    (byte)(ReadROM(1) >> 8),
                    (byte)(ReadROM(2) & 0xFF),
                    (byte)(ReadROM(2) >> 8)
                );
                Console.WriteLine("[Intel8254X] EEPROM on this controller");
            }

            Console.WriteLine($"[Intel8254X] MAC: {MAC}");

            Linkup();
            for (int i = 0; i < 0x80; i++)
                WriteRegister((ushort)(0x5200 + i * 4), 0);

            Console.Write("[Intel8254X] IRQ: ");
            Console.WriteLine(((ulong)device.IRQ).ToString("x2"));

            RXInitialize();
            TXInitialize();

            WriteRegister(0x00D0, 0x1F6DC);
            ReadRegister(0xC0);

            Console.Write("[Intel8254X] Speed: ");
            Console.Write(((ulong)Speed).ToString());
            Console.Write(' ');
            Console.Write("FullDuplex: ");
            Console.WriteLine(FullDuplex?"Yes":"No");
            Console.WriteLine("[Intel8254X] Configuration Done");

            Network.MAC = MAC;
            //This may not work on vmware
            //Interrupts.EnableInterrupt(device.IRQ, &OnInterrupt);
            Interrupts.EnableInterrupt(0x20, &OnInterrupt);
            IRQ = device.IRQ;

            //Literally instance
            Network.Controller = new Intel8254X();
        }

        public static void Reset()
        {
            Console.Write("[Intel8254X] Reseting controller...");

            WriteRegister(0, 1 << 26);
            while (BitHelpers.IsBitSet(ReadRegister(0), 26)) ;
        }

        private static void TXInitialize()
        {
            TXDescs = (uint)Allocator.Allocate(8 * 16);

            for (int i = 0; i < 8; i++)
            {
                TXDesc* desc = (TXDesc*)(TXDescs + (i * 16));
                desc->addr = (ulong)Allocator.Allocate(65536);
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
                //Console.WriteLine("[Intel8254X] Linking Up");
                Linkup();
            }
            if ((Status & 0x10) != 0)
            {
                //Console.WriteLine("[Intel8254X] Good Threshold");
            }

            if ((Status & 0x80) != 0)
            {
                //Console.WriteLine("[Intel8254X] Packet Received");
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

            Console.Write("[Intel8254X] Waiting for network connection ");
            Console.Wait((uint*)(BAR0 + 0x08), 1);
            Console.WriteLine();
        }

        public override void Send(byte* Buffer, int Length)
        {
            TXDesc* desc = (TXDesc*)(TXDescs + (TXCurr * 16));
            Native.Movsb((void*)desc->addr, Buffer, (ulong)Length);
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