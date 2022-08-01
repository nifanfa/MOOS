using System.Runtime.InteropServices;

namespace System.Reflection.PortableExecutable
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct OptionalHeaders64
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
}