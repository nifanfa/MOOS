using System.Runtime.InteropServices;

namespace MOOS
{
    /// <summary>
    /// How to get the size of multiboot module?
    /// </summary>
    [StructLayout(LayoutKind.Sequential,Pack = 1)]
    public unsafe struct VHDFooter
    {
        public fixed byte cookie[8];
        public uint features;
        public uint ffversion;
        public ulong dataoffset;
        public uint timestamp;
        public uint creatorapp;
        public uint creatorver;
        public uint creatorhos;
        public ulong origsize;
        public ulong currsize;
        public uint diskgeom;
        public uint disktype;
        public uint checksum;
        public fixed byte uniqueid[16];
        public byte savedst;
        public fixed byte reserved[427];
    }
}