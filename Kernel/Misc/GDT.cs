using Internal.Runtime.CompilerServices;
using System.Runtime.InteropServices;


static class GDT
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct GDTEntry
    {
        public ushort LimitLow;
        public ushort BaseLow;
        public byte BaseMid;
        public byte Access;
        public byte LimitHigh_Flags;
        public byte BaseHigh;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GDTDescriptor
    {
        public ushort Limit;
        public ulong Base;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct TSSEntry
    {
        public ushort LimitLow;
        public ushort BaseLow;
        public byte BaseMidLow;
        public byte Access;
        public byte LimitHigh_Flags;
        public byte BaseMidHigh;
        public uint BaseHigh;
        public uint Reserved;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct TSS
    {
        public uint Reserved0;
        public uint Rsp0Low;
        public uint Rsp0High;
        public uint Rsp1Low;
        public uint Rsp1High;
        public uint Rsp2Low;
        public uint Rsp2High;
        public uint Reserved1;
        public uint Reserved2;
        public uint Ist1Low;
        public uint Ist1High;
        public uint Ist2Low;
        public uint Ist2High;
        public uint Ist3Low;
        public uint Ist3High;
        public uint Ist4Low;
        public uint Ist4High;
        public uint Ist5Low;
        public uint Ist5High;
        public uint Ist6Low;
        public uint Ist6High;
        public uint Ist7Low;
        public uint Ist7High;
        public ulong Reserved3;
        public ushort Reserved4;
        public ushort IOMapBase;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct GDTS
    {
        public GDTEntry Empty;
        public GDTEntry KernelCode;
        public GDTEntry KernelData;
        public TSSEntry TSS;
    }


    static TSS tss;
    static GDTS gdts;
    public static GDTDescriptor gdtr;


    public static void Initialise()
    {
        gdts.KernelCode.LimitLow = 0xFFFF;
        gdts.KernelCode.Access = 0x9A;
        gdts.KernelCode.LimitHigh_Flags = 0xAF;

        gdts.KernelData.LimitLow = 0xFFFF;
        gdts.KernelData.Access = 0x92;
        gdts.KernelData.LimitHigh_Flags = 0xCF;

        unsafe
        {
            fixed (TSS* _tss = &tss)
            {
                var addr = (ulong)_tss;
                gdts.TSS.LimitLow = (ushort)(Unsafe.SizeOf<TSS>() - 1);
                gdts.TSS.BaseLow = (ushort)(addr & 0xFFFF);
                gdts.TSS.BaseMidLow = (byte)((addr >> 16) & 0xFF);
                gdts.TSS.BaseMidHigh = (byte)((addr >> 24) & 0xFF);
                gdts.TSS.BaseHigh = (uint)(addr >> 32);
                gdts.TSS.Access = 0x89;
                gdts.TSS.LimitHigh_Flags = 0x80;
            }
        }

        unsafe
        {
            fixed (GDTS* _gdts = &gdts)
            {
                gdtr.Limit = (ushort)(Unsafe.SizeOf<GDTEntry>() * 3 - 1);
                gdtr.Base = (ulong)_gdts;
            }
        }

        Native.Load_GDT(ref gdtr);
    }
}