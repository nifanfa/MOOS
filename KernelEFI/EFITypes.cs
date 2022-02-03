using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct EfiGuid
{
    public uint Data1;
    public ushort Data2;
    public ushort Data3;
    public fixed byte Data4[8];
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiTime
{
    public ushort Year; // 1998 - 20XX
    public byte Month; // 1 - 12
    public byte Day; // 1 - 31
    public byte Hour; // 0 - 23
    public byte Minute; // 0 - 59
    public byte Second; // 0 - 59
    public byte Pad1;
    public uint Nanosecond; // 0 - 999,999,999
    public short TimeZone; // -1440 to 1440 or 2047
    public byte Daylight;
    public byte Pad2;
}


[StructLayout(LayoutKind.Sequential)]
public unsafe struct EfiIPv4Address
{
    public fixed byte Addr[4];
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct EfiIPv6Address
{
    public fixed byte Addr[16];
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct EfiMacAddress
{
    public fixed byte Addr[32];
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiManagedNetworkConfigData
{
    public uint ReceivedQueueTimeoutValue;
    public uint TransmitQueueTimeoutValue;
    public ushort ProtocolTypeFilter;
    public bool EnableUnicastReceive;
    public bool EnableMulticastReceive;
    public bool EnableBroadcastReceive;
    public bool EnablePromiscuousReceive;
    public bool FlushQueuesOnReset;
    public bool EnableReceiveTimestamps;
    public bool DisableBackgroundPolling;
}


public enum EfiAllocateType
{
    AllocateAnyPages,
    AllocateMaxAddress,
    AllocateAddress,
    MaxAllocateType
}


public enum EfiMemoryType
{
    EfiReservedMemoryType,
    EfiLoaderCode,
    EfiLoaderData,
    EfiBootServicesCode,
    EfiBootServicesData,
    EfiRuntimeServicesCode,
    EfiRuntimeServicesData,
    EfiConventionalMemory,
    EfiUnusableMemory,
    EfiACPIReclaimMemory,
    EfiACPIMemoryNVS,
    EfiMemoryMappedIO,
    EfiMemoryMappedIOPortSpace,
    EfiPalCode,
    EfiMaxMemoryType
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiMemoryDescriptor
{
    public uint Type; // Field size is 32 bits followed by 32 bit pad
    public uint Pad;
    public EfiPhysicalAddress PhysicalStart; // Field size is 64 bits
    public EfiVirtualAddress VirtualStart; // Field size is 64 bits
    public ulong NumberOfPages; // Field size is 64 bits
    public ulong Attribute; // Field size is 64 bits
}