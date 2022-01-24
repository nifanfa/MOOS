//https://www.dmtf.org/sites/default/files/standards/documents/DSP0134_3.5.0.pdf

using System;
using System.Runtime.InteropServices;

namespace Kernel.Driver
{
    public static unsafe class SMBIOS
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct SMBIOSEntryPoint
        {
            public fixed byte EntryPointString[4];
            public byte Checksum;
            public byte Length;
            public byte MajorVersion;
            public byte MinorVersion;
            public ushort MaxStructureSize;
            public byte EntryPointRevision;
            public fixed byte FormattedArea[5];
            public fixed byte EntryPointString2[5];
            public byte Checksum2;
            public ushort TableLength;
            public uint TableAddress;
            public ushort NumberOfStructures;
            public byte BCDRevision;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct SMBIOSHeader
        {
            public HeaderTypes Type;
            public byte Length;
            public ushort Handle;
        };

        public enum HeaderTypes : byte
        {
            BIOSInformation = 0,
            SystemInformation = 1,
            MainboardInformation = 2,
            EnclosureChasisInformation = 3,
            ProcessorInformation = 4,
            CacheInformation = 7,
            SystemSlotsInformation = 9,
            PhysicalMemoryArray = 16,
            MemoryDeviceInformation = 17,
            MemoryArrayMappedAddress = 19,
            MemoryDeviceMappedAddress = 20,
            SystemBootInformation = 32
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ProcessorInformation
        {
            public byte SocketDesignation;
            public byte ProcessorType;
            public byte ProcessorFamily;
            public byte ProcessorManufacturer;
            public ulong ProcessorID;
            public byte ProcessorVersion;
            public byte Voltage;
            public ushort ExternalClock;
            public ushort MaxSpeed;
            public ushort CurrentSpeed;
            public byte Status;
            public byte ProcessorUpgrade;

            public ushort L1CacheHandle;
            public ushort L2CacheHandle;
            public ushort L3CacheHandle;
            public byte SerialNumber;
            public byte AssetTag;
            public byte PartNumber;
            public byte CoreCount;
            public byte CoreEnabled;
            public byte ThreadCount;
            public ushort ProcessorCharacteristics;
            public ushort ProcessorFamily2;

            public ushort CoreCount2;
            public ushort CoreEnabled2;
            public ushort ThreadCount2;
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PhysicalMemoryArray
        {
            public byte Location;
            public byte Use;
            public ErrorCorrectionTypes MemoryErrorCorrection;
            public uint MaximumCapacity;
            public ushort MemoryErrorInformationHandle;
            public ushort NumberofMemoryDevices;
            public ulong ExtendedMaximumCapacity;
        }

        public enum ErrorCorrectionTypes : byte
        {
            Other = 0x01,
            Unknown = 0x02,
            None = 0x03,
            Parity = 0x04,
            SinglebitECC = 0x05,
            MultibitECC = 0x06,
            CRC = 0x07,
        }


        public static void Initialise()
        {
            byte* p = (byte*)0xF0000;
            while (*(uint*)p != 0x5F4D535F) p++;
            Console.WriteLine("SMBIOS Entry Found");

            SMBIOSEntryPoint* entry = (SMBIOSEntryPoint*)p;
            InfoEntries(entry);
        }

        private static byte* InfoEntries(SMBIOSEntryPoint* entry)
        {
            byte* p = (byte*)entry->TableAddress;
            while ((uint)p < (entry->TableAddress + entry->TableLength))
            {
                SMBIOSHeader* hdr = (SMBIOSHeader*)p;
                p += hdr->Length;

                switch (hdr->Type)
                {
                    case HeaderTypes.ProcessorInformation:
                        ProcessorInformation* pinfo = (ProcessorInformation*)((byte*)hdr + sizeof(SMBIOSHeader));
                        PrintIndex(hdr, pinfo->PartNumber);
                        PrintIndex(hdr, pinfo->SocketDesignation);
                        PrintIndex(hdr, pinfo->ProcessorManufacturer);
                        Console.Write(" Speed: ");
                        Console.Write(((uint)pinfo->CurrentSpeed).ToString());
                        Console.Write("Mhz ");
                        Console.WriteLine();
                        break;
                    case HeaderTypes.PhysicalMemoryArray:
                        PhysicalMemoryArray* minfo = (PhysicalMemoryArray*)((byte*)hdr + sizeof(SMBIOSHeader));
                        Console.Write("Installed RAM: ");
                        Console.Write((minfo->MaximumCapacity + minfo->ExtendedMaximumCapacity).ToString());
                        Console.WriteLine("MiB");
                        Console.Write("Number of Memory Devices: ");
                        Console.WriteLine(((uint)minfo->NumberofMemoryDevices).ToString());
                        Console.Write("Memory Correction: ");
                        switch (minfo->MemoryErrorCorrection)
                        {
                            case ErrorCorrectionTypes.Other: Console.WriteLine("Other"); break;
                            case ErrorCorrectionTypes.Unknown: Console.WriteLine("Unknown"); break;
                            case ErrorCorrectionTypes.None: Console.WriteLine("None"); break;
                            case ErrorCorrectionTypes.Parity: Console.WriteLine("Parity"); break;
                            case ErrorCorrectionTypes.SinglebitECC: Console.WriteLine("SinglebitECC"); break;
                            case ErrorCorrectionTypes.MultibitECC: Console.WriteLine("MultibitECC"); break;
                            case ErrorCorrectionTypes.CRC: Console.WriteLine("CRC"); break;
                        }
                        break;
                }

                while (*(ushort*)p != 0) p++;
                p += 0x02;
            }

            return p;
        }

        private static void PrintIndex(SMBIOSHeader* hdr, byte index, bool ignoreSpace = false)
        {
            byte* p = (byte*)hdr;
            p += hdr->Length;
            int count = 0;
            while (count != index)
            {
                while (*p++ != 0) ;
                count++;
            }
            while (*p != 0)
            {
                char c;
                c = (char)*p++;
                if (ignoreSpace && c == ' ') continue;
                Console.Write(c);
            }
            Console.Write(' ');
        }
    }
}