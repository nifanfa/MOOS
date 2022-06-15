using System.Runtime.InteropServices;

namespace MOOS
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MultibootInfo
    {
        public uint Flags;
        public uint MemLow;
        public uint MemHigh;
        public uint BootDev;
        public uint CMDLine;
        public uint ModCount;
        public uint ModAddr;
        public uint Syms1;
        public uint Syms2;
        public uint Syms3;
        public uint Syms4;
        public uint MMapLen;
        public uint MMapAddr;
        public uint DrvLen;
        public uint DrvAddr;
        public uint CfgTable;
        public uint LdrName;
        public uint ApmTable;
        public uint VBECtrlInfo;
        public uint VBEInfo;
        public uint VBEMode;
        public uint VBEInterfaceSeg;
        public uint VBEInterfaceOff;
        public uint VBEInterfaceLen;

        public uint* Mods 
        {
            get 
            {
                return (uint*)ModAddr;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VBEInfo
    {
        public ushort Attributes;
        public byte WindowA;
        public byte WindowB;
        public ushort Granularity;
        public ushort WindowSize;
        public ushort SegmentA;
        public ushort SegmentB;
        public uint WinFuncPtr;
        public ushort Pitch;
        public ushort ScreenWidth;
        public ushort ScreenHeight;
        public byte WChar;
        public byte YChar;
        public byte Planes;
        public byte BitsPerPixel;
        public byte Banks;
        public byte MemoryModel;
        public byte BankSize;
        public byte ImagePages;
        public byte Reserved0;
        public byte RedMask;
        public byte RedPosition;
        public byte GreenMask;
        public byte GreenPosition;
        public byte BlueMask;
        public byte BluePosition;
        public byte ReservedMask;
        public byte ReservedPosition;
        public byte DirectColorAttributes;
        public uint PhysBase;
        public uint OffScreenMemoryOff;
        public ushort OffScreenMemorySize;
    }
}