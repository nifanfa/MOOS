using Internal.Runtime.CompilerServices;
using Kernel;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EFI
{
    public enum Status : ulong
    {
        Error = 0x8000000000000000,

        Success = 0,

        BufferTooSmall = 5 | Error,
    }

    public enum AllocateType : uint
    {
        AnyPages,
        MaxAddress,
        Address
    }

    public enum MemoryType : uint
    {
        ReservedMemoryType,
        LoaderCode,
        LoaderData,
        BootServicesCode,
        BootServicesData,
        RuntimeServicesCode,
        RuntimeServicesData,
        ConventionalMemory,
        UnusableMemory,
        ACPIReclaimMemory,
        ACPIMemoryNVS,
        MemoryMappedIO,
        MemoryMappedIOPortSpace,
        PalCode
    }

    [Flags]
    public enum FileMode : ulong
    {
        Read = 1,
        Write = 2,
        Create = 0x8000000000000000
    }

    [Flags]
    public enum FileAttribute : ulong
    {
        ReadOnly = 1,
        Hidden = 2,
        System = 4,
        Reserved = 8,
        Directory = 16,
        Archive = 32,

        ValidAttr = ReadOnly | Hidden | System | Directory | Archive
    }

    public enum GraphicsPixelFormat
    {
        RedGreenBlueReserved8BitPerColor,
        BlueGreenRedReserved8BitPerColor,
        BitMask,
        BltOnly
    }

    public enum LocateSearchType
    {
        AllHandles,
        ByRegisterNotify,
        ByProtocol
    }


    public readonly struct Handle
    {
        readonly IntPtr _pointer;

        public Handle(IntPtr ptr) { _pointer = ptr; }

        public static readonly Handle Zero = new Handle(IntPtr.Zero);
    }

    public readonly struct Event
    {
        readonly IntPtr _value;
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct TableHeader
    {
        public readonly ulong Signature;
        public readonly uint Revision;
        public readonly uint HeaderSize;
        public readonly uint Crc32;
        public readonly uint Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct SystemTable
    {
        public readonly TableHeader Hdr;
        public readonly char* FirmwareVendor;
        public readonly uint FirmwareRevision;
        public readonly Handle ConsoleInHandle;
        public readonly SimpleTextInputProtocol* ConIn;
        public readonly Handle ConsoleOutHandle;
        public readonly SimpleTextOutputProtocol* ConOut;
        public readonly Handle StandardErrorHandle;
        public readonly SimpleTextOutputProtocol* StdErr;
        public readonly RuntimeServices* RuntimeServices;
        public readonly BootServices* BootServices;
        public readonly ulong NumberOfTableEntries;
        public readonly IntPtr ConfigurationTable;
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct RuntimeServices
    {
        public readonly TableHeader Hdr;

        readonly IntPtr _GetTime;
        readonly IntPtr _SetTime;
        readonly IntPtr _GetWakeupTime;
        readonly IntPtr _SetWakeupTime;
        readonly IntPtr _SetVirtualAddressMap;
        readonly IntPtr _ConvertPointer;
        readonly IntPtr _GetVariable;
        readonly IntPtr _GetNextVariableName;
        readonly IntPtr _SetVariable;
        readonly IntPtr _GetNextHighMonotonicCount;
        readonly IntPtr _ResetSystem;
        readonly IntPtr _UpdateCapsule;
        readonly IntPtr _QueryCapsuleCapabilities;
        readonly IntPtr _QueryVariableInfo;


        public unsafe ulong GetTime(out Time time, out TimeCapabilities capabilities)
        {
            fixed (Time* timeAddress = &time)
            fixed (TimeCapabilities* capabilitiesAddress = &capabilities)
                return RawCalliHelper.StdCall(_GetTime, timeAddress, capabilitiesAddress);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Time
    {
        public ushort Year;
        public byte Month;
        public byte Day;
        public byte Hour;
        public byte Minute;
        public byte Second;
        public byte Pad1;
        public uint Nanosecond;
        public short TimeZone;
        public byte Daylight;
        public byte PAD2;
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct SimpleTextOutputMode
    {
        public readonly int MaxMode;
        public readonly int Mode;
        public readonly int Attribute;
        public readonly int CursorColumn;
        public readonly int CursorRow;
        public readonly bool CursorVisible;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TimeCapabilities
    {
        public uint Resolution;
        public uint Accuracy;
        public bool SetsToZero;
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct InputKey
    {
        public readonly ushort ScanCode;
        public readonly ushort UnicodeChar;
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct SimpleTextInputProtocol
    {
        readonly IntPtr _Reset;
        readonly IntPtr _readKeyStroke;

        public readonly Event WaitForKey;


        public unsafe void Reset(bool ExtendedVerification)
        {
            fixed (SimpleTextInputProtocol* _this = &this)
                RawCalliHelper.StdCall(_Reset, _this, ExtendedVerification);
        }

        public unsafe Status ReadKeyStroke(out InputKey Key)
        {
            fixed (SimpleTextInputProtocol* _this = &this)
            fixed (InputKey* _key = &Key)
                return (Status)RawCalliHelper.StdCall(_readKeyStroke, _this, _key);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct BootServices
    {
        public readonly TableHeader Hdr;

        readonly IntPtr _RaiseTPL;
        readonly IntPtr _RestoreTPL;
        readonly IntPtr _AllocatePages;
        readonly IntPtr _FreePages;
        readonly IntPtr _GetMemoryMap;
        readonly IntPtr _AllocatePool;
        readonly IntPtr _FreePool;
        readonly IntPtr _CreateEvent;
        readonly IntPtr _SetTimer;
        readonly IntPtr _WaitForEvent;
        readonly IntPtr _SignalEvent;
        readonly IntPtr _CloseEvent;
        readonly IntPtr _CheckEvent;
        readonly IntPtr _InstallProtocolInterface;
        readonly IntPtr _ReinstallProtocolInterface;
        readonly IntPtr _UninstallProtocolInterface;
        readonly IntPtr _HandleProtocol;
        readonly IntPtr _Reserved;
        readonly IntPtr _RegisterProtocolNotify;
        readonly IntPtr _LocateHandle;
        readonly IntPtr _LocateDevicePath;
        readonly IntPtr _InstallConfigurationTable;
        readonly IntPtr _LoadImage;
        readonly IntPtr _StartImage;
        readonly IntPtr _Exit;
        readonly IntPtr _UnloadImage;
        readonly IntPtr _ExitBootServices;
        readonly IntPtr _GetNextMonotonicCount;
        readonly IntPtr _Stall;
        readonly IntPtr _SetWatchdogTimer;
        readonly IntPtr _ConnectController;
        readonly IntPtr _DisconnectController;
        readonly IntPtr _OpenProtocol;
        readonly IntPtr _CloseProtocol;
        readonly IntPtr _OpenProtocolInformation;
        readonly IntPtr _ProtocolsPerHandle;
        readonly IntPtr _LocateHandleBuffer;
        readonly IntPtr _LocateProtocol;
        readonly IntPtr _InstallMultipleProtocolInterfaces;
        readonly IntPtr _UninstallMultipleProtocolInterfaces;
        readonly IntPtr _CalculateCrc32;
        readonly IntPtr _CopyMem;
        readonly IntPtr _SetMem;
        readonly IntPtr _CreateEventEx;


        public unsafe Status WaitForEvent(ulong count, Event[] events, out ulong index)
        {
            fixed (Event* _events = events)
            fixed (ulong* _index = &index)
                return (Status)RawCalliHelper.StdCall(_WaitForEvent, count, _events, _index);
        }

        public unsafe Status WaitForSingleEvent(Event @event)
        {
            uint i = 0;

            return (Status)RawCalliHelper.StdCall(_WaitForEvent, 1, &@event, &i);
        }

        public Status Stall(ulong Microseconds)
            => (Status)RawCalliHelper.StdCall(_Stall, Microseconds);

#if DEBUG
        public static int AllocateCount = 0;
#endif

        // TODO: Figure out how to get rid of this pointer
        public unsafe Status AllocatePool(MemoryType type, ulong size, IntPtr* buf)
        {
#if DEBUG
            AllocateCount++;
#endif
            return (Status)RawCalliHelper.StdCall(_AllocatePool, type, size, buf);
        }

        public unsafe Status AllocatePages(AllocateType type, MemoryType memType, ulong pages, ref IntPtr addr)
        {
            fixed (IntPtr* _addr = &addr)
                return (Status)RawCalliHelper.StdCall(_AllocatePages, type, memType, pages, _addr);
        }

        public Status FreePool(IntPtr buf)
        {
#if DEBUG
            AllocateCount--;
#endif
            return (Status)RawCalliHelper.StdCall(_FreePool, buf);
        }

        public Status FreePool<T>(T[] arr)
        {
#if DEBUG
            AllocateCount--;
#endif
            return (Status)RawCalliHelper.StdCall(_FreePool, Unsafe.As<T[], IntPtr>(ref arr));
        }

        public void CopyMem(IntPtr dst, IntPtr src, ulong length)
            => RawCalliHelper.StdCall(_CopyMem, dst, src, length);

        public void SetMem(IntPtr buf, ulong size, byte val)
            => RawCalliHelper.StdCall(_SetMem, buf, size, val);

        public Status SetWatchdogTimer(ulong timeout, ulong code, ulong dataSize, IntPtr data)
            => (Status)RawCalliHelper.StdCall(_SetWatchdogTimer, timeout, code, dataSize, data);

        public unsafe Status OpenProtocol<T>(Handle handle, ref Guid protocol, out T* iface, Handle agent, Handle controller, uint attr) where T : unmanaged
        {
            fixed (Guid* _protocol = &protocol)
            fixed (T** _iface = &iface)
                return (Status)RawCalliHelper.StdCall(_OpenProtocol, handle, _protocol, (Handle*)_iface, agent, controller, attr);
        }

        public unsafe Status GetMemoryMap(ref ulong memMapSize, IntPtr memMap, out ulong mapKey, out ulong descSize, out uint descVer)
        {
            fixed (ulong* _memMapSize = &memMapSize)
            fixed (ulong* _mapKey = &mapKey)
            fixed (ulong* _descSize = &descSize)
            fixed (uint* _descVer = &descVer)
                return (Status)RawCalliHelper.StdCall(_GetMemoryMap, _memMapSize, memMap, _mapKey, _descSize, _descVer);
        }

        public Status ExitBootServices(Handle imageHandle, ulong mapKey)
            => (Status)RawCalliHelper.StdCall(_ExitBootServices, imageHandle, mapKey);

        // TODO: Get rid of the out Handle* and use an out Handle[] or out NativeArray<Handle> instead
        public unsafe Status LocateHandleBuffer(LocateSearchType searchType, ref Guid protocol, IntPtr searchKey, ref ulong numHandles, out Handle* buffer)
        {
            fixed (Guid* _protocol = &protocol)
            fixed (ulong* _numHandles = &numHandles)
            fixed (Handle** _buffer = &buffer)
                return (Status)RawCalliHelper.StdCall(_LocateHandleBuffer, searchType, _protocol, searchKey, _numHandles, (Handle*)_buffer);
        }

        public unsafe Status HandleProtocol(Handle handle, ref Guid protocol, out IntPtr iface)
        {
            fixed (Guid* _protocol = &protocol)
            fixed (IntPtr* _iface = &iface)
                return (Status)RawCalliHelper.StdCall(_HandleProtocol, handle, _protocol, _iface);
        }

        public unsafe Status CloseProtocol(Handle handle, ref Guid protocol, Handle agent, Handle controller)
        {
            fixed (Guid* pProt = &protocol)
                return (Status)RawCalliHelper.StdCall(_CloseProtocol, handle, pProt, agent, controller);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct SimpleTextOutputProtocol
    {
        readonly IntPtr _Reset;
        readonly IntPtr _OutputString;
        readonly IntPtr _TestString;
        readonly IntPtr _QueryMode;
        readonly IntPtr _SetMode;
        readonly IntPtr _SetAttribute;
        readonly IntPtr _ClearScreen;
        readonly IntPtr _SetCursorPosition;
        readonly IntPtr _EnableCursor;

        public readonly SimpleTextOutputMode* Mode;


        public unsafe Status Reset(bool ExtendedVerification)
        {
            fixed (SimpleTextOutputProtocol* _this = &this)
                return (Status)RawCalliHelper.StdCall(_Reset, _this, &ExtendedVerification);
        }

        public unsafe Status OutputString(char* str)
        {
            fixed (SimpleTextOutputProtocol* _this = &this)
                return (Status)RawCalliHelper.StdCall(_OutputString, _this, str);
        }

        public unsafe Status TestString(char* str)
        {
            fixed (SimpleTextOutputProtocol* _this = &this)
                return (Status)RawCalliHelper.StdCall(_TestString, _this, str);
        }

        public unsafe Status QueryMode(ulong mode, out ulong columns, out ulong rows)
        {
            fixed (SimpleTextOutputProtocol* _this = &this)
            fixed (ulong* _columns = &columns, _rows = &rows)
                return (Status)RawCalliHelper.StdCall(_QueryMode, _this, mode, _columns, _rows);
        }

        public unsafe Status SetMode(ulong mode)
        {
            fixed (SimpleTextOutputProtocol* _this = &this)
                return (Status)RawCalliHelper.StdCall(_SetMode, _this, mode);
        }

        public unsafe Status SetAttribute(ulong attribute)
        {
            fixed (SimpleTextOutputProtocol* _this = &this)
                return (Status)RawCalliHelper.StdCall(_SetAttribute, _this, attribute);
        }

        public unsafe Status ClearScreen()
        {
            fixed (SimpleTextOutputProtocol* _this = &this)
                return (Status)RawCalliHelper.StdCall(_ClearScreen, _this);
        }

        public unsafe Status SetCursorPosition(ulong column, ulong row)
        {
            fixed (SimpleTextOutputProtocol* _this = &this)
                return (Status)RawCalliHelper.StdCall(_SetCursorPosition, _this, column, row);
        }

        public unsafe Status EnableCursor(bool visible)
        {
            fixed (SimpleTextOutputProtocol* _this = &this)
                return (Status)RawCalliHelper.StdCall(_EnableCursor, _this, visible);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Guid
    {
        public uint Data1;
        public ushort Data2;
        public ushort Data3;
        public fixed byte Data4[8];

        public Guid(uint d1, ushort d2, ushort d3, byte[] d4)
        {
            Data1 = d1;
            Data2 = d2;
            Data3 = d3;

            fixed (byte* dst = Data4)
            fixed (byte* src = d4)
                Allocator.MemoryCopy((IntPtr)dst, (IntPtr)src, 8);

            d4.Dispose();
        }

        public static Guid LoadedImageProtocol;
        public static Guid SimpleFileSystemProtocol;
        public static Guid FileInfo;
        public static Guid GraphicsOutputProtocol;
        public static Guid ComponentName2Protocol;

        internal static void Initialise()
        {
            LoadedImageProtocol = new Guid(0x5B1B31A1, 0x9562, 0x11d2, new byte[] { 0x8E, 0x3F, 0x00, 0xA0, 0xC9, 0x69, 0x72, 0x3B });
            SimpleFileSystemProtocol = new Guid(0x964e5b22, 0x6459, 0x11d2, new byte[] { 0x8e, 0x39, 0x0, 0xa0, 0xc9, 0x69, 0x72, 0x3b });
            FileInfo = new Guid(0x09576e92, 0x6d3f, 0x11d2, new byte[] { 0x8e, 0x39, 0x00, 0xa0, 0xc9, 0x69, 0x72, 0x3b });
            GraphicsOutputProtocol = new Guid(0x9042a9de, 0x23dc, 0x4a38, new byte[] { 0x96, 0xfb, 0x7a, 0xde, 0xd0, 0x80, 0x51, 0x6a });
            ComponentName2Protocol = new Guid(0x6a7a5cff, 0xe8d9, 0x4f70, new byte[] { 0xba, 0xda, 0x75, 0xab, 0x30, 0x25, 0xce, 0x14 });
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct FileProtocol
    {
        public readonly ulong Revision;

        readonly IntPtr _Open;
        readonly IntPtr _Close;
        readonly IntPtr _Delete;
        readonly IntPtr _Read;
        readonly IntPtr _Write;
        readonly IntPtr _GetPosition;
        readonly IntPtr _SetPosition;
        readonly IntPtr _GetInfo;
        readonly IntPtr _SetInfo;
        readonly IntPtr _Flush;
        readonly IntPtr _OpenEx;
        readonly IntPtr _ReadEx;
        readonly IntPtr _WriteEx;
        readonly IntPtr _FlushEx;

        public unsafe Status Open(out FileProtocol* newHandle, string filename, FileMode mode, FileAttribute attr)
        {
            fixed (FileProtocol* _this = &this)
            fixed (FileProtocol** _newHandle = &newHandle)
            fixed (char* f = &filename._firstChar)
                return (Status)RawCalliHelper.StdCall(_Open, _this, (Handle*)_newHandle, f, mode, attr);
        }

        public unsafe Status Close()
        {
            fixed (FileProtocol* _this = &this)
                return (Status)RawCalliHelper.StdCall(_Close, _this);
        }

        public unsafe Status GetInfo(ref Guid type, ref ulong bufSize, out FileInfo buf)
        {
            fixed (FileProtocol* _this = &this)
            fixed (Guid* _type = &type)
            fixed (ulong* _bufSize = &bufSize)
            fixed (FileInfo* _buf = &buf)
                return (Status)RawCalliHelper.StdCall(_GetInfo, _this, _type, _bufSize, _buf);
        }

        public unsafe Status Read(ref ulong bufSize, IntPtr buf)
        {
            fixed (FileProtocol* _this = &this)
            fixed (ulong* _bufSize = &bufSize)
                return (Status)RawCalliHelper.StdCall(_Read, _this, _bufSize, buf);
        }

        public unsafe Status Read<T>(out T obj) where T : unmanaged
        {
            var len = (ulong)Unsafe.SizeOf<T>();
            obj = default;

            fixed (T* _obj = &obj)
                return Read(ref len, (IntPtr)_obj);
        }

        public unsafe Status Read<T>(out T[] arr, int count) where T : unmanaged
        {
            var len = (ulong)count * (ulong)Unsafe.SizeOf<T>();
            arr = new T[count];

            fixed (T* _arr = arr)
                return Read(ref len, (IntPtr)_arr);
        }

        public unsafe Status SetPosition(ulong pos)
        {
            fixed (FileProtocol* _this = &this)
                return (Status)RawCalliHelper.StdCall(_SetPosition, _this, pos);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct DevicePathProtocol
    {
        public readonly byte Type;
        public readonly byte SubType;
        public readonly ushort Length;
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct LoadedImageProtocol
    {
        public readonly uint Revision;
        public readonly Handle ParentHandle;
        public readonly SystemTable* SystemTable;
        public readonly Handle DeviceHandle;
        public readonly DevicePathProtocol* FilePath;
        public readonly IntPtr Reserved;
        public readonly uint LoadOptionsSize;
        public readonly IntPtr LoadOptions;
        public readonly IntPtr ImageBase;
        public readonly ulong ImageSize;
        public readonly MemoryType ImageCodeType;
        public readonly MemoryType ImageDataType;

        public readonly IntPtr _Unload;
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct SimpleFileSystemProtocol
    {
        public readonly ulong Revision;

        readonly IntPtr _OpenVolume;


        public unsafe Status OpenVolume(out FileProtocol* root)
        {
            fixed (SimpleFileSystemProtocol* _this = &this)
            fixed (FileProtocol** _root = &root)
                return (Status)RawCalliHelper.StdCall(_OpenVolume, _this, (Handle*)_root);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FileInfo
    {
        public ulong Size;
        public ulong FileSize;
        public ulong PhysicalSize;
        public Time CreateTime;
        public Time LastAccessTime;
        public Time ModificationTime;
        public ulong Attribute;
        public fixed char FileName[128];
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct MemoryDescriptor
    {
        public readonly uint Type;
        public readonly ulong PhysicalStart;
        public readonly ulong VirtualStart;
        public readonly ulong NumberOfPages;
        public readonly ulong Attribute;
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct PixelBitMask
    {
        public readonly uint RedMask;
        public readonly uint GreenMask;
        public readonly uint BlueMask;
        public readonly uint ReservedMask;
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct GraphicsOutputModeInformation
    {
        public readonly uint Version;
        public readonly uint HorizontalResolution;
        public readonly uint VerticalResolution;
        public readonly GraphicsPixelFormat PixelFormat;
        public readonly PixelBitMask PixelInformation;
        public readonly uint PixelsPerScanLine;
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct GraphicsOutputProtocolMode
    {
        public readonly uint MaxMode;
        public readonly uint Mode;
        public readonly GraphicsOutputModeInformation* Info;
        public readonly ulong SizeOfInfo;
        public readonly ulong FrameBufferBase;
        public readonly ulong FrameBufferSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct GraphicsOutputProtocol
    {
        readonly IntPtr _QueryMode;
        readonly IntPtr _SetMode;
        readonly IntPtr _Blt;

        public readonly GraphicsOutputProtocolMode* Mode;


        public unsafe Status QueryMode(uint modeNumber, out ulong sizeOfInfo, out GraphicsOutputModeInformation* info)
        {
            fixed (GraphicsOutputProtocol* _this = &this)
            fixed (ulong* _sizeOfInfo = &sizeOfInfo)
            fixed (GraphicsOutputModeInformation** _info = &info)
                return (Status)RawCalliHelper.StdCall(_QueryMode, _this, modeNumber, _sizeOfInfo, (Handle*)_info);
        }

        public unsafe Status SetMode(uint mode)
        {
            fixed (GraphicsOutputProtocol* _this = &this)
                return (Status)RawCalliHelper.StdCall(_SetMode, _this, mode);
        }
    }

    public unsafe static class EFI
    {
        public const uint OPEN_PROTOCOL_GET_PROTOCOL = 0x00000002;

        static IntPtr pST;

        public static SystemTable* ST
        {
            get
            {
                return (SystemTable*)pST;
            }
            set
            {
                pST = (IntPtr)value;
            }
        }


        public static void Initialise(SystemTable* systemTable)
        {
            ST = systemTable;
            Guid.Initialise();
        }
    }
}