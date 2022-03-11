// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence
using System.Runtime.InteropServices;

namespace OS_Sharp
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct DOSHeader
    {
        public ushort e_magic;              // Magic number
        public ushort e_cblp;               // Bytes on last page of file
        public ushort e_cp;                 // Pages in file
        public ushort e_crlc;               // Relocations
        public ushort e_cparhdr;            // Size of header in paragraphs
        public ushort e_minalloc;           // Minimum extra paragraphs needed
        public ushort e_maxalloc;           // Maximum extra paragraphs needed
        public ushort e_ss;                 // Initial (relative) SS value
        public ushort e_sp;                 // Initial SP value
        public ushort e_csum;               // Checksum
        public ushort e_ip;                 // Initial IP value
        public ushort e_cs;                 // Initial (relative) CS value
        public ushort e_lfarlc;             // File address of relocation table
        public ushort e_ovno;               // Overlay number
        public fixed ushort e_res1[4];      // Reserved words
        public ushort e_oemid;              // OEM identifier (for e_oeminfo)
        public ushort e_oeminfo;            // OEM information; e_oemid specific
        public fixed ushort e_res2[10];     // Reserved words
        public int e_lfanew;                // File address of new exe header
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct FileHeader
    {
        public ushort Machine;
        public ushort NumberOfSections;
        public uint TimeDateStamp;
        public uint PointerToSymbolTable;
        public uint NumberOfSymbols;
        public ushort SizeOfOptionalHeader;
        public ushort Characteristics;
    }

    internal enum SubSystemType : ushort
    {
        Unknown = 0,
        Native = 1,
        WindowsGUI = 2,
        WindowsCUI = 3,
        PosixCUI = 7,
        WindowsCEGui = 9,
        EfiApplication = 10,
        EfiBootServiceDriver = 11,
        EfiRuntimeDriver = 12,
        EfiRom = 13,
        Xbox = 14
    }

    internal enum DllCharacteristicsType : ushort
    {
        RES_0 = 0x0001,
        RES_1 = 0x0002,
        RES_2 = 0x0004,
        RES_3 = 0x0008,
        DynamicBase = 0x0040,
        ForceIntegrity = 0x0080,
        NxCompat = 0x0100,
        NoIsolation = 0x0200,
        NoSEH = 0x0400,
        NoBind = 0x0800,
        RES_4 = 0x1000,
        WDMDriver = 0x2000,
        TerminalServerName = 0x8000
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct DataDirectory
    {
        public uint VirtualAddress;
        public uint Size;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct OptionalHeaders64
    {
        public ushort Magic;
        public byte MajorLinkerVersion;
        public byte MinorLinkerVersion;
        public uint SizeOfCode;
        public uint SizeOfInitializedData;
        public uint SizeOfUninitializedData;
        public uint AddressOfEntryPoint;
        public uint BaseOfCode;
        public ulong ImageBase;
        public uint SectionAlignment;
        public uint FileAlignment;
        public ushort MajorOperatingSystemVersion;
        public ushort MinorOperatingSystemVersion;
        public ushort MajorImageVersion;
        public ushort MinorImageVersion;
        public ushort MajorSubsystemVersion;
        public ushort MinorSubsystemVersion;
        public uint Win32VersionValue;
        public uint SizeOfImage;
        public uint SizeOfHeaders;
        public uint CheckSum;
        public SubSystemType Subsystem;
        public DllCharacteristicsType DllCharacteristics;
        public ulong SizeOfStackReserve;
        public ulong SizeOfStackCommit;
        public ulong SizeOfHeapReserve;
        public ulong SizeOfHeapCommit;
        public uint LoaderFlags;
        public uint NumberOfRvaAndSizes;
        public DataDirectory ExportTable;
        public DataDirectory ImportTable;
        public DataDirectory ResourceTable;
        public DataDirectory ExceptionTable;
        public DataDirectory CertificateTable;
        public DataDirectory BaseRelocationTable;
        public DataDirectory Debug;
        public DataDirectory Architecture;
        public DataDirectory GlobalPtr;
        public DataDirectory TLSTable;
        public DataDirectory LoadConfigTable;
        public DataDirectory BoundImport;
        public DataDirectory IAT;
        public DataDirectory DelayImportDescriptor;
        public DataDirectory CLRRuntimeHeader;
        public DataDirectory Reserved;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct NtHeaders64
    {
        public uint Signature;
        public FileHeader FileHeader;
        public OptionalHeaders64 OptionalHeader;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct SectionHeader
    {
        public fixed byte Name[8];
        public uint PhysicalAddress_VirtualSize;
        public uint VirtualAddress;
        public uint SizeOfRawData;
        public uint PointerToRawData;
        public uint PointerToRelocations;
        public uint PointerToLineNumbers;
        public ushort NumberOfRelocations;
        public ushort NumberOfLineNumbers;
        public uint Characteristics;
    }
}
