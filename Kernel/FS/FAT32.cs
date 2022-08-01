/*
 * Obsoloted
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Kernel.FS
{
    public unsafe class FAT32 : FileSystem
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct FAT32_DBR
        {
            public fixed byte BS_jmpBoot[3];
            public fixed byte BS_OEMName[8];
            public ushort BPB_BytesPerSec;
            public byte BPB_SecPerClus;
            public ushort BPB_RsvdSecCnt;
            public byte BPB_NumFATs;
            public ushort BPB_RootEntCnt;
            public fixed byte BPB_TotSec16[2];
            public byte BPB_Media;
            public fixed byte BPB_FATSz16[2];
            public fixed byte BPB_SecPerTrk[2];
            public fixed byte BPB_NumHeads[2];
            public fixed byte BPB_HiddSec[4];
            public fixed byte BPB_TotSec32[4];
            public uint BPB_FATSz32;
            public fixed byte BPB_ExtFlags[2];
            public fixed byte BPB_FSVer[2];
            public uint BPB_RootClus;
            public fixed byte FSInfo[2];
            public fixed byte BPB_BkBootSec[2];
            public fixed byte BPB_Reserved[12];
            public byte BS_DrvNum;
            public byte BS_Reserved1;
            public byte BS_BootSig;
            public fixed byte BS_VolID[4];
            public fixed byte BS_FilSysType[11];
            public fixed byte BS_FilSysType1[8];    //"FAT32 " offset:82
        };


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Entry
        {
            public fixed byte Name[11];
            public byte Attribute;
            public byte Reserved;
            public byte CreateTimeTenMS;
            public ushort CreateTime;
            public ushort CreateDate;
            public ushort LastAccessDate;
            public ushort ClusterHigh;
            public ushort LastModifyTime;
            public ushort LastModifyDate;
            public ushort ClusterLow;
            public uint Size;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct EntryLong
        {
            public byte Attrbute;
            public fixed char Name1[5];
            public byte Signature;
            public byte Rsvd;
            public byte Checksum;
            public fixed char Name2[6];
            public ushort Cluster;
            public fixed char Name3[2];
        }

        public class AEntry
        {
            public string Name;
            public string Parent;
            public Entry* Item;

            public uint FountAtSec;
            public uint FountAtOffset;

            public uint Cluster
            {
                get
                {
                    return (uint)(Item->ClusterHigh << 16 | Item->ClusterLow);
                }
            }
            public uint Size
            {
                get
                {
                    return Item->Size;
                }
            }

            public void Free()
            {
                Allocator.Free((System.IntPtr)Item);
                this.Dispose();
            }
        }

        public struct Attributes
        {
            public const byte ReadWrite = 0x00;
            public const byte ReadOnly = 0x01;
            public const byte Hidden = 0x02;
            public const byte System = 0x04;
            public const byte VolumeLabel = 0x08;
            public const byte SubDirectory = 0x10;
            public const byte Archive = 0x20;
        }

        public struct Status
        {
            public const byte Empty = 0x00;
            public const byte Deleted = 0xE5;
            public const byte SpecialFile = 0x2E;
        }

        FAT32_DBR* DBR;

        public const ushort SectorSize = 512;

        public List<AEntry> Items;

        public Disk Disk;

        public FAT32(Disk disk, ulong lba)
        {
            Disk = disk;
            Items = new List<AEntry>();

            byte[] Buffer = new byte[512];
            disk.Read(lba, 1, Buffer);
            fixed (byte* P = Buffer) DBR = (FAT32_DBR*)P;

            if (DBR->BS_FilSysType1[3] != '3' && DBR->BS_FilSysType1[4] != '2') { Console.WriteLine("This is not a fat32 partition!"); return; }

            RootDirectorySector = (uint)(lba + DBR->BPB_RsvdSecCnt + DBR->BPB_FATSz32 * DBR->BPB_NumFATs + (DBR->BPB_RootClus - 2u) * DBR->BPB_SecPerClus);

            LongNameCache = null;

            ReadList(RootDirectorySector, "/");
        }

        public override string[] GetFiles()
        {
            string[] str = new string[Items.Count];
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = Items[i].Name;
            }
            return str;
        }

        private uint RootDirectorySector;

        private uint GetSectorOffset(uint Cluster)
        {
            return RootDirectorySector + ((Cluster - 2) * DBR->BPB_SecPerClus);
        }

        public override byte[] ReadAllBytes(string FileName)
        {
            if (FileName[0] != '/') FileName = "/" + FileName;

            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Parent + Items[i].Name == FileName)
                {
                    uint count = GetSectorsWillUse(Items[i].Size);
                    byte[] buffer = new byte[count * SectorSize];
                    Disk.Read(GetSectorOffset(Items[i].Cluster), count, buffer);

                    byte[] result = new byte[Items[i].Size];
                    for (int k = 0; k < result.Length; k++)
                    {
                        result[k] = buffer[k];
                    }

                    buffer.Dispose();

                    return result;
                }
            }

            return null;
        }

        private uint GetSectorsWillUse(uint size)
        {
            uint result = 1;
            if (size > SectorSize)
            {
                result = size / SectorSize;
                if (size % SectorSize != 0)
                {
                    result++;
                }
            }
            return result;
        }

        public string LongNameCache;

        private void ReadList(uint sector, string parent)
        {
            if (parent == "/")
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    Items[i].Free();
                }
                Items.Clear();
            }

            uint Index = 0;
            do
            {
                byte[] Buffer = new byte[512];
                Disk.Read(sector + Index, 1, Buffer);
                fixed (byte* P = Buffer)
                {
                    bool LastIsEmpty = false;
                    for (uint i = 0; i < 512; i += 32)
                    {
                        switch (*(P + i))
                        {
                            case Status.Empty:
                                LastIsEmpty = true;
                                continue;
                            case Status.SpecialFile:
                                LastIsEmpty = false;
                                continue;
                            case Status.Deleted:
                                LastIsEmpty = false;
                                continue;
                        }
                        LastIsEmpty = false;
                        Entry* item = (Entry*)Allocator.Allocate((uint)sizeof(Entry));
                        Allocator.MemoryCopy((System.IntPtr)item, (System.IntPtr)(P + i), 32);
                        EntryLong* itemlong = (EntryLong*)item;

                        if (itemlong->Signature == 0x0F)
                        {
                            if (LongNameCache == null) LongNameCache = string.Empty;

                            int p = 0;
                            for (p = 0; p < 5; p++)
                            {
                                if (itemlong->Name1[p] == 0 || itemlong->Name1[p] == 0xFFFF)
                                {
                                    break;
                                }
                            }
                            string Cache0 = new string(itemlong->Name1, 0, p);

                            for (p = 0; p < 6; p++)
                            {
                                if (itemlong->Name2[p] == 0 || itemlong->Name2[p] == 0xFFFF)
                                {
                                    break;
                                }
                            }
                            string Cache1 = new string(itemlong->Name2, 0, p);

                            for (p = 0; p < 2; p++)
                            {
                                if (itemlong->Name3[p] == 0 || itemlong->Name3[p] == 0xFFFF)
                                {
                                    break;
                                }
                            }
                            string Cache2 = new string(itemlong->Name3, 0, p);

                            string prev = LongNameCache;
                            LongNameCache = (Cache0 + Cache1 + Cache2) + prev;

                            prev.Dispose();
                            Cache0.Dispose();
                            Cache1.Dispose();
                            Cache2.Dispose();
                            continue;
                        }

                        AEntry aDirectoryItem = new AEntry()
                        {
                            Item = item,
                            Parent = parent,

                            FountAtSec = sector + Index,
                            FountAtOffset = i
                        };
                        if (item->Attribute == Attributes.SubDirectory)
                        {
                            if (LongNameCache != null)
                            {
                                aDirectoryItem.Name = LongNameCache;
                            }
                            else
                            {
                                aDirectoryItem.Name = string.FromASCII((System.IntPtr)item->Name, 8, 0x20) + string.FromASCII((System.IntPtr)(item->Name + 8), 3, 0x20);
                            }
                        }
                        else
                        {
                            if (LongNameCache != null)
                            {
                                aDirectoryItem.Name = LongNameCache;
                            }
                            else
                            {
                                aDirectoryItem.Name = string.FromASCII((System.IntPtr)item->Name, 8, 0x20) + "." + string.FromASCII((System.IntPtr)(item->Name + 8), 3, 0x20);
                            }
                        }
                        Items.Add(aDirectoryItem);

                        if (LongNameCache != null) LongNameCache = string.Empty;
                        LongNameCache = null;

                        if (item->Attribute == Attributes.SubDirectory)
                        {
                            ReadList(GetSectorOffset(Items[Items.Count - 1].Cluster), parent + string.FromASCII((System.IntPtr)item->Name, 8, 0x20) + "/");
                        }
                    }
                    Buffer.Dispose();
                    if (LastIsEmpty) return;
                }
            } while (Index++ != -1);
        }
    }
}
*/