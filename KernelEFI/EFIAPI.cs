using System;
using System.Runtime.InteropServices;

public enum EfiEventType : uint
{
    EvtTimer = 0x80000000,
    EvtRuntime = 0x40000000,
    EvtRuntimeContext = 0x20000000,

    EvtNotifyWait = 0x00000100,
    EvtNotifySignal = 0x00000200,

    EvtSignalExitBootServices = 0x00000201,
    EvtSignalVirtualAddressChange = 0x60000202,

    EvtEfiSignalMask = 0x000000FF,
    EvtEfiSignalMax = 4,

    EfiEventTimer = EvtTimer,
    EfiEventRuntime = EvtRuntime,
    EfiEventRuntimeContext = EvtRuntimeContext,
    EfiEventNotifyWait = EvtNotifyWait,
    EfiEventNotifySignal = EvtNotifySignal,
    EfiEventSignalExitBootServices = EvtSignalExitBootServices,
    EfiEventSignalVirtualAddressChange = EvtSignalVirtualAddressChange,
    EfiEventEfiSignalMask = EvtEfiSignalMask,
    EfiEventEfiSignalMax = EvtEfiSignalMax
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiTimeCapabilities
{
    public uint Resolution; // 1e-6 parts per million
    public uint Accuracy; // hertz
    public byte SetsToZero; // Set clears sub-second time
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiHandle
{
    public UIntPtr Handle;
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiEvent
{
    public UIntPtr Handle;
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiVirtualAddress
{
    public UIntPtr Address;
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiPhysicalAddress
{
    public UIntPtr Address;
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiOpenProtocolInformationEntry
{
    public EfiHandle AgentHandle;
    public EfiHandle ControllerHandle;
    public uint Attributes;
    public uint OpenCount;
}

public enum EfiLocateSearchType
{
    AllHandles,
    ByRegisterNotify,
    ByProtocol
}

public enum EfiResetType
{
    EfiResetCold,
    EfiResetWarm,
    EfiResetShutdown
}

[StructLayout(LayoutKind.Explicit)]
public struct BlockContent
{
    [FieldOffset(0)] public EfiPhysicalAddress DataBlock;
    [FieldOffset(0)] public EfiPhysicalAddress ContinuationPointer;
}


[StructLayout(LayoutKind.Sequential)]
public struct EfiCapsuleBlockDescriptor
{
    public ulong Length;
    public BlockContent Union;
}


[StructLayout(LayoutKind.Sequential)]
public struct EfiCapsuleHeader
{
    public EfiGuid CapsuleGuid;
    public uint HeaderSize;
    public uint Flags;
    public uint CapsuleImageSize;
}

public enum EfiInterfaceType
{
    EfiNativeInterface,
    EfiPcodeInterface
}

public enum EfiTpl : ulong
{
    TplApplication = 4,
    TplCallback = 8,
    TplNotify = 16,
    TplHighLevel = 31,
    EfiTplApplication = TplApplication,
    EfiTplCallback = TplCallback,
    EfiTplNotify = TplNotify,
    EfiTplHighLevel = TplHighLevel
}

[Flags]
public enum EfiStatus : ulong
{
    EfiSuccess = 0,
    EfiLoadError = 0x8000000000000000 | 1,
    EfiInvalidParameter = 0x8000000000000000 | 2,
    EfiUnsupported = 0x8000000000000000 | 3,
    EfiBadBufferSize = 0x8000000000000000 | 4,
    EfiBufferTooSmall = 0x8000000000000000 | 5,
    EfiNotReady = 0x8000000000000000 | 6,
    EfiDeviceError = 0x8000000000000000 | 7,
    EfiWriteProtected = 0x8000000000000000 | 8,
    EfiOutOfResources = 0x8000000000000000 | 9,
    EfiVolumeCorrupted = 0x8000000000000000 | 10,
    EfiVolumeFull = 0x8000000000000000 | 11,
    EfiNoMedia = 0x8000000000000000 | 12,
    EfiMediaChanged = 0x8000000000000000 | 13,
    EfiNotFound = 0x8000000000000000 | 14,
    EfiAccessDenied = 0x8000000000000000 | 15,
    EfiNoResponse = 0x8000000000000000 | 16,
    EfiNoMapping = 0x8000000000000000 | 17,
    EfiTimeout = 0x8000000000000000 | 18,
    EfiNotStarted = 0x8000000000000000 | 19,
    EfiAlreadyStarted = 0x8000000000000000 | 20,
    EfiAborted = 0x8000000000000000 | 21,
    EfiIcmpError = 0x8000000000000000 | 22,
    EfiTftpError = 0x8000000000000000 | 23,
    EfiProtocolError = 0x8000000000000000 | 24,
    EfiIncompatibleVersion = 0x8000000000000000 | 25,
    EfiSecurityViolation = 0x8000000000000000 | 26,
    EfiCrcError = 0x8000000000000000 | 27,
    EfiEndOfMedia = 0x8000000000000000 | 28,
    EfiEndOfFile = 0x8000000000000000 | 31,
    EfiInvalidLanguage = 0x8000000000000000 | 32,
    EfiCompromisedData = 0x8000000000000000 | 33,
    EfiWarnUnknownGlyph = 1,
    EfiWarnDeleteFailure = 2,
    EfiWarnWriteFailure = 3,
    EfiWarnBufferTooSmall = 4
}


[StructLayout(LayoutKind.Sequential)]
public readonly struct EfiTableHeader
{
    public readonly ulong Signature;
    public readonly uint Revision;
    public readonly uint HeaderSize;
    public readonly uint CRC32;
    public readonly uint Reserved;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct EfiRuntimeServices
{
    public readonly EfiTableHeader Hdr;

    public readonly delegate* < EfiTime*, EfiTimeCapabilities*, EfiStatus> GetTime;
    public readonly delegate* <EfiTime*, EfiStatus> SetTime;
    public readonly delegate* <byte*, byte*, EfiTime*, EfiStatus> GetWakeupTime;
    public readonly delegate* <byte, EfiTime*, EfiStatus> SetWakeupTime;

    public readonly delegate* <ulong, ulong, uint, EfiMemoryDescriptor*, EfiStatus> SetVirtualAddressMap;
    public readonly delegate* <ulong, void**, EfiStatus> ConvertPointer;

    public readonly delegate* <char*, EfiGuid*, uint*, ulong*, void*, EfiStatus> GetVariable;
    public readonly delegate* <ulong*, char*, EfiGuid*, EfiStatus> GetNextVariableName;
    public readonly delegate* <char*, EfiGuid, uint, ulong, void*, EfiStatus> SetVariable;

    public readonly delegate* <uint*, EfiStatus> GetNextHighMonotonicCount;
    public readonly delegate* <EfiResetType, ulong, char*, EfiStatus> ResetSystem;

    public readonly delegate* <EfiCapsuleHeader**, ulong, EfiPhysicalAddress, EfiStatus> UpdateCapsule;

    public readonly delegate* <EfiCapsuleHeader**, ulong, ulong*, EfiResetType*, EfiStatus>
        QueryCapsuleCapabilities;

    public readonly delegate* <uint, ulong*, ulong*, ulong*, EfiStatus> QueryVariableInfo;
}


public enum EfiTimerDelay
{
    TimerCancel,
    TimerPeriodic,
    TimerRelative,
    TimerTypeMax
}

public enum EfiProtocolAttributes : uint
{
    EFI_OPEN_PROTOCOL_BY_HANDLE_PROTOCOL = 0x00000001,
    EFI_OPEN_PROTOCOL_GET_PROTOCOL = 0x00000002,
    EFI_OPEN_PROTOCOL_TEST_PROTOCOL = 0x00000004,
    EFI_OPEN_PROTOCOL_BY_CHILD_CONTROLLER = 0x00000008,
    EFI_OPEN_PROTOCOL_BY_DRIVER = 0x00000010,
    EFI_OPEN_PROTOCOL_EXCLUSIVE = 0x00000020
}

[StructLayout(LayoutKind.Sequential)]
public readonly unsafe struct EfiBootServices
{
    public readonly EfiTableHeader Hdr;

    public readonly delegate* <EfiTpl, EfiTpl> RaiseTPL;
    public readonly delegate* <EfiTpl, void> RestoreTPL;

    public readonly delegate* <EfiAllocateType, EfiMemoryType, ulong, EfiPhysicalAddress*, EfiStatus>
        AllocatePages;

    public readonly delegate* <EfiPhysicalAddress, ulong, EfiStatus> FreePages;
    public readonly delegate* <ulong*, EfiMemoryDescriptor*, ulong*, ulong*, uint*, EfiStatus> GetMemoryMap;
    public readonly delegate* <EfiMemoryType, ulong, out void*, EfiStatus> AllocatePool;
    public readonly delegate* <void*, EfiStatus> FreePool;

    public readonly delegate* <EfiEventType, EfiTpl,
        delegate* <EfiEvent, void*, void>,
        void*, EfiEvent*, EfiStatus> CreateEvent;

    public readonly delegate* <EfiEvent, EfiTimerDelay, ulong, EfiStatus> SetTimer;
    public readonly delegate* <ulong, EfiEvent*, ulong*, EfiStatus> WaitForEvent;
    public readonly delegate* <EfiEvent, EfiStatus> SignalEvent;
    public readonly delegate* <EfiEvent, EfiStatus> CloseEvent;
    public readonly delegate* <EfiEvent, EfiStatus> CheckEvent;

    public readonly delegate* <EfiHandle*, EfiGuid*, EfiInterfaceType, void*, EfiStatus>
        InstallProtocolInterface;

    public readonly delegate* <EfiHandle, EfiGuid*, void*, void*, EfiStatus> ReinstallProtocolInterface;
    public readonly delegate* <EfiHandle, EfiGuid*, void*, EfiStatus> UninstallProtocolInterface;
    public readonly delegate* <EfiHandle, EfiGuid*, void**, EfiStatus> HandleProtocol;
    public readonly delegate* <EfiHandle, EfiGuid*, void**, EfiStatus> PCHandleProtocol;
    public readonly delegate* <EfiGuid*, EfiEvent, void**> RegisterProtocolNotify;

    public readonly delegate* <EfiLocateSearchType, EfiGuid*, void*, ulong*, EfiHandle*, EfiStatus>
        LocateHandle;

    public readonly delegate* <EfiGuid*, EfiDevicePathProtocol**, EfiHandle*, EfiStatus>
        LocateDevicePath;

    public readonly delegate* <EfiGuid*, void*, EfiStatus> InstallConfigurationTable;


    public readonly delegate* <bool, EfiHandle, EfiDevicePathProtocol*, void*, ulong, EfiHandle*,
        EfiStatus> LoadImage;

    public readonly delegate* <EfiHandle, ulong*, char**, EfiStatus> StartImage;
    public readonly delegate* <EfiHandle, EfiStatus, ulong, char*, EfiStatus> Exit;
    public readonly delegate* <EfiHandle, EfiStatus> UnloadImage;
    public readonly delegate* <EfiHandle, ulong, EfiStatus> ExitBootServices;

    public readonly delegate* <ulong*, EfiStatus> GetNextMonotonicCount;
    public readonly delegate* <ulong, EfiStatus> Stall;
    public readonly delegate* <ulong, ulong, int, char*, EfiStatus> SetWatchdogTimer;


    public readonly delegate* <EfiHandle, EfiHandle*, EfiDevicePathProtocol*, bool, EfiStatus>
        ConnectController;

    public readonly delegate* <EfiHandle, EfiHandle, EfiHandle, EfiStatus> DisconnectController;

    public readonly delegate* <EfiHandle, EfiGuid*, void**, EfiHandle, EfiHandle, EfiProtocolAttributes,
        EfiStatus> OpenProtocol;

    public readonly delegate* <EfiHandle, EfiGuid*, EfiHandle, EfiHandle, EfiStatus> CloseProtocol;

    public readonly delegate* <EfiHandle, EfiGuid*, EfiOpenProtocolInformationEntry**,
        ulong*, EfiStatus> OpenProtocolInformation;

    public readonly delegate* <EfiHandle, EfiGuid***, ulong*, EfiStatus> ProtocolsPerHandle;

    public readonly delegate* <EfiLocateSearchType, EfiGuid*, void*, ulong*, EfiHandle**, EfiStatus>
        LocateHandleBuffer;

    public readonly delegate* <EfiGuid*, void*, void**, EfiStatus> LocateProtocol;
    public readonly void* InstallMultipleProtocolInterfaces;
    public readonly void* UninstallMultipleProtocolInterfaces;

    public readonly delegate* <void*, ulong, int*, EfiStatus> CalculateCrc32;

    public readonly delegate* <void*, void*, ulong, void> CopyMem;
    public readonly delegate* <void*, ulong, byte, void> SetMem;

    public readonly delegate* <uint, EfiTpl,
        delegate* <EfiEvent, void*, void>, void*, EfiGuid*,
        EfiEvent*, EfiStatus> CreateEventEx;
}


[StructLayout(LayoutKind.Sequential)]
public readonly unsafe struct EfiConfigurationTable
{
    private readonly EfiGuid VendorGuid;
    private readonly void* VendorTable;
}


[StructLayout(LayoutKind.Sequential)]
public readonly unsafe struct EfiSystemTable
{
    public readonly EfiTableHeader Hdr;

    public readonly char* FirmwareVendor;
    public readonly uint FirmwareRevision;

    public readonly EfiHandle ConsoleInHandle;
    public readonly SIMPLE_INPUT_INTERFACE* ConIn;

    public readonly EfiHandle ConsoleOutHandle;
    public readonly SimpleTextOutputInterface* ConOut;

    public readonly EfiHandle StandardErrorHandle;
    public readonly SimpleTextOutputInterface* StdErr;

    public readonly EfiRuntimeServices* RuntimeServices;
    public readonly EfiBootServices* BootServices;

    public readonly ulong NumberOfTableEntries;
    public readonly EfiConfigurationTable* ConfigurationTable;
}