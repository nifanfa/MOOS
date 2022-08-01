using MOOS.FS;
using MOOS.Misc;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MOOS
{
    public static unsafe class SATA
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct HBA
        {
            public uint HostCapability;
            public uint GlobalHostControl;
            public uint InterruptStatus;
            public uint PortsImplemented;
            public uint Version;
            public uint CCCControl;
            public uint CCCPorts;
            public uint EnclosureManagementLocation;
            public uint EnclosureManagementControl;
            public uint HostCapabilitiesExtended;
            public uint BIOSHandoffControlStatus;
            public fixed byte Reserved0[0x74];
            public fixed byte Vendor[0x60];
            public HBAPort Ports;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct HBAPort
        {
            public ulong CommandListBase;
            public ulong FISBaseAddress;
            public uint InterruptStatus;
            public uint InterruptEnable;
            public uint CommandStatus;
            public uint Reserved0;
            public uint TaskFileData;
            public uint Signature;
            public uint SataStatus;
            public uint SataControl;
            public uint SataError;
            public uint SataActive;
            public uint CommandIssue;
            public uint SataNotification;
            public uint FISSwitchControl;
            public fixed uint Reserved1[11];
            public fixed uint Vendor[4];
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct HBACommandHeader
        {
            public byte P1;
            public byte P2;
            public ushort PRDTLength;
            public uint PRDBCount;
            public ulong CommandTableBaseAddress;
            public fixed uint Reserved1[4];

            public byte CommandFISLength
            {
                get
                {
                    return (byte)(P1 & 0x1F);
                }

                set
                {
                    unchecked { P1 &= (byte)~0x1F; }
                    P1 |= (byte)(value & 0x1F);
                }
            }

            public bool ATAPI
            {
                get
                {
                    return BitHelpers.IsBitSet(P1, 5);
                }
            }

            public bool Write
            {
                get
                {
                    return BitHelpers.IsBitSet(P1, 6);
                }

                set
                {
                    if (value)
                    {
                        P1 |= 1 << 6;
                    }
                    else
                    {
                        unchecked { P1 &= (byte)~(1 << 6); }
                    }
                }
            }

            public bool Prefetchable
            {
                get
                {
                    return BitHelpers.IsBitSet(P1, 7);
                }
            }

            public bool Reset
            {
                get
                {
                    return BitHelpers.IsBitSet(P2, 0);
                }
            }

            public bool BIST
            {
                get
                {
                    return BitHelpers.IsBitSet(P2, 1);
                }
            }

            public bool ClearBusy
            {
                get
                {
                    return BitHelpers.IsBitSet(P2, 2);
                }

                set
                {
                    if (value)
                    {
                        P2 |= 1 << 2;
                    }
                    else
                    {
                        unchecked { P2 &= (byte)~(1 << 2); }
                    }
                }
            }

            public bool Reserved0
            {
                get
                {
                    return BitHelpers.IsBitSet(P2, 3);
                }
            }

            public byte PortMultiplier
            {
                get
                {
                    return (byte)((P2 & 0xF0) >> 4);
                }
            }
        };

        public enum SATAPortType
        {
            NONE = 0,
            SATA = 1,
            SEMB = 2,
            PM = 3,
            ATAPI = 4
        }

        public static HBA* Controller = null;
        public static List<SATADevice> Ports = null;

        public static void Initialize()
        {
            Ports = new List<SATADevice>();
            #region FindDev
            for (int i = 0; i < PCI.Devices.Count; i++)
            {
                if (
                    PCI.Devices[i] != null &&
                    PCI.Devices[i].ClassID == 0x01 &&
                    PCI.Devices[i].SubClassID == 0x06
                    )
                {
                    var dev = PCI.Devices[i];

                    if (dev == null) return;
                    ushort reg = dev.ReadRegister(0x04);
                    dev.WriteRegister(0x04, 0x04 | 0x02 | 0x01);
                    Controller = (HBA*)dev.Bar5;
                    for (int k = 0; k < 32; k++)
                    {
                        if ((Controller->PortsImplemented & (1 << k)) != 0)
                        {
                            SATAPortType type = CheckPortType(&(&Controller->Ports)[k]);
                            if (type == SATAPortType.SATA || type == SATAPortType.ATAPI)
                            {
                                SATADevice sata = new SATADevice();
                                sata.PortType = type;
                                sata.HBAPort = &(&Controller->Ports)[k];
                                Ports.Add(sata);
                            }
                        }
                    }
                    if (Ports.Count != 0) goto End;
                    dev.WriteRegister(0x04, reg);
                }
            }
            #endregion
        End:
            for (int i = 0; i < Ports.Count; i++)
            {
                Ports[i].Configure();

                Console.Write("[SATA] SATA Port ");
                Console.Write(((ulong)i).ToString());
                Console.WriteLine(" Configured");
            }
        }

        public static SATAPortType CheckPortType(HBAPort* port)
        {
            uint statStat = port->SataStatus;
            byte intpowman = (byte)((statStat >> 8) & 0b111);
            byte devdetect = (byte)(statStat & 0b111);

            //Not Present
            if (devdetect != 0x3) return SATAPortType.NONE;
            //Not Active
            if (intpowman != 0x1) return SATAPortType.NONE;

            switch (port->Signature)
            {
                case 0x00000101:
                    return SATAPortType.SATA;
                case 0xEB140101:
                    return SATAPortType.ATAPI;
                case 0x96690101:
                    return SATAPortType.PM;
                case 0xC33C0101:
                    return SATAPortType.SEMB;
                default:
                    return SATAPortType.NONE;
            }
        }

        public class SATADevice : Disk
        {
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            struct FIS_REG_H2D
            {
                public byte FISType;
                byte PortMultiplier_Reserved0_CommandControl;
                public byte Command;
                public byte FeatureLow;
                public byte LBA0;
                public byte LBA1;
                public byte LBA2;
                public byte DeviceRegister;
                public byte LBA3;
                public byte LBA4;
                public byte LBA5;
                public byte FeatureHigh;
                public ushort Count;
                public byte ISOCommandCompletion;
                public byte Control;
                public fixed byte Reserved1[4];

                public byte PortMultiplier
                {
                    get
                    {
                        return (byte)(PortMultiplier_Reserved0_CommandControl & 0xF);
                    }
                }

                public bool CommandControl
                {
                    get
                    {
                        return BitHelpers.IsBitSet(PortMultiplier_Reserved0_CommandControl, 7);
                    }

                    set
                    {
                        if (value)
                        {
                            PortMultiplier_Reserved0_CommandControl |= 1 << 7;
                        }
                        else
                        {
                            unchecked { PortMultiplier_Reserved0_CommandControl &= (byte)~(1 << 7); }
                        }
                    }
                }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            struct HBACommandTable
            {
                public fixed byte CommandFIS[64];
                public fixed byte ATAPICommand[16];
                public fixed byte Reserved[48];
                public HBAPRDTEntry PRDTEntry;
            };

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            struct HBAPRDTEntry
            {
                public ulong DataBaseAddress;
                public uint Reserved0;
                public uint ByteCount_Reserved1_InterruptOnCompletion;

                public bool InterruptOnCompletion
                {
                    get
                    {
                        return BitHelpers.IsBitSet(ByteCount_Reserved1_InterruptOnCompletion, 31);
                    }

                    set
                    {
                        if (value)
                        {
                            ByteCount_Reserved1_InterruptOnCompletion |= 1U << 31;
                        }
                        else
                        {
                            unchecked { ByteCount_Reserved1_InterruptOnCompletion &= ~(1U << 31); }
                        }
                    }
                }

                public uint ByteCount
                {
                    get
                    {
                        return ByteCount_Reserved1_InterruptOnCompletion & 0x3FFFFF;
                    }

                    set
                    {
                        ByteCount_Reserved1_InterruptOnCompletion &= ~0x3FFFFFU;
                        ByteCount_Reserved1_InterruptOnCompletion |= value & 0x3FFFFFU;
                    }
                }
            };

            public SATAPortType PortType;
            public volatile HBAPort* HBAPort;

            public void Configure()
            {
                StopCMD();

                ulong newBase = (ulong)Allocator.Allocate(4096);
                HBAPort->CommandListBase = newBase;

                ulong fisBase = (ulong)Allocator.Allocate(4096);
                HBAPort->FISBaseAddress = fisBase;

                HBACommandHeader* cmdhdr = (HBACommandHeader*)(HBAPort->CommandListBase);

                for (int i = 0; i < 32; i++)
                {
                    cmdhdr[i].PRDTLength = 8;
                    var p = Allocator.Allocate(4096);
                    Allocator.ZeroFill(p, 4096);
                    cmdhdr[i].CommandTableBaseAddress = (ulong)p;
                }

                StartCMD();
            }

            public void StartCMD()
            {
                while ((HBAPort->CommandStatus & 0x8000) != 0) ;
                HBAPort->CommandStatus |= 0x0010;
                HBAPort->CommandStatus |= 0x0001;
            }

            public void StopCMD()
            {
                HBAPort->CommandStatus &= ~0x0001U;
                HBAPort->CommandStatus &= ~0x0010U;

                while (true)
                {
                    if ((HBAPort->CommandStatus & 0x4000) != 0) continue;
                    if ((HBAPort->CommandStatus & 0x8000) != 0) continue;
                    break;
                }
            }

            public const int SectorSize = 512;

            public override bool Read(ulong sector, uint count, byte* p)
            {
                return ReadOrWrite(sector, (ushort)count, p, false);
            }

            public override bool Write(ulong sector, uint count, byte* p)
            {
                return ReadOrWrite(sector, (ushort)count, p, true);
            }

            private bool ReadOrWrite(ulong Sector,ushort Count, byte* Buffer, bool Write)
            {
                if (Count >= 512) Panic.Error("[SATA] Too many sectors to read!");

                if (PortType == SATAPortType.ATAPI && Write) return false;
                unchecked { HBAPort->InterruptStatus = (uint)-1; }
                int Slot = FindSlot();
                if (Slot == -1) return false;

                HBACommandHeader* hdr = ((HBACommandHeader*)(HBAPort->CommandListBase));
                hdr += Slot;
                hdr->CommandFISLength = (byte)(sizeof(FIS_REG_H2D) / sizeof(uint));
                hdr->Write = Write;
                hdr->ClearBusy = true;
                hdr->PRDTLength = (ushort)(((Count - 1) >> 4) + 1);

                HBACommandTable* table = ((HBACommandTable*)(hdr->CommandTableBaseAddress));

                Native.Stosb(table, 0, (ulong)(sizeof(HBACommandTable) + (hdr->PRDTLength - 1) * sizeof(HBAPRDTEntry)));

                int i = 0;
                for(i = 0; i < hdr->PRDTLength - 1; i++) 
                {
                    (&table->PRDTEntry)[i].DataBaseAddress = (ulong)Buffer;
                    (&table->PRDTEntry)[i].ByteCount = 8 * 1024 - 1;
                    (&table->PRDTEntry)[i].InterruptOnCompletion = true;
                    Buffer += 4 * 1024;
                    Count -= 16;
                }

                (&table->PRDTEntry)[i].DataBaseAddress = (ulong)Buffer;
                (&table->PRDTEntry)[i].ByteCount = (uint)((Count << 9) - 1);
                (&table->PRDTEntry)[i].InterruptOnCompletion = true;

                FIS_REG_H2D* FIS = (FIS_REG_H2D*)(table->CommandFIS);
                FIS->FISType = 0x27;
                FIS->CommandControl = true;
                FIS->Command = (byte)(Write ? 0x35 : 0x25);

                FIS->LBA0 = (byte)((Sector) & 0xFF);
                FIS->LBA1 = (byte)((Sector >> 8) & 0xFF);
                FIS->LBA2 = (byte)((Sector >> 16) & 0xFF);
                FIS->LBA3 = (byte)((Sector >> 24) & 0xFF);
                FIS->LBA4 = (byte)((Sector >> 32) & 0xFF);
                FIS->LBA5 = (byte)((Sector >> 40) & 0xFF);

                FIS->DeviceRegister = 1 << 6;

                FIS->Count = Count;

                while ((HBAPort->TaskFileData & (0x80 | 0x08)) != 0) ;

                HBAPort->CommandIssue = (uint)(1 << Slot);

                while (true)
                {
                    if ((HBAPort->CommandIssue & (1 << Slot)) == 0) break;
                    if ((HBAPort->InterruptStatus & ((1 << 30))) != 0) 
                    {
                        return false;
                    }
                }

                if ((HBAPort->InterruptStatus & (1 << 30)) != 0)
                {
                    return false;
                }

                while (HBAPort->CommandIssue != 0);
                return true;
            }

            public int FindSlot()
            {
                uint Slots = HBAPort->SataActive | HBAPort->CommandIssue;
                for (int i = 0; i < 32; i++)
                {
                    if ((Slots & (1 << i)) == 0) return i;
                }
                return -1;
            }
        }
    }
}