/*
//TO-DO may reimplement FAT32?
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
RootDirectorySector = (uint)(lba + DBR->BPB_RsvdSecCnt + DBR->BPB_FATSz32 * DBR->BPB_NumFATs + (DBR->BPB_RootClus - 2u) * DBR->BPB_SecPerClus);
DBR->BPB_SecPerClus

FileOffset(Sector) = RootDirectorySector + ((Cluster - 2) * DBR->BPB_SecPerClus);
*/