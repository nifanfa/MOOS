// Copywrite (C) 2021 Contributors of nifanfa/Solution1. Licensed under the  MIT licence
namespace Kernel.FS
{
    public abstract class Disk
    {
        public abstract bool Read(ulong sector, uint count, byte[] data);
        public abstract bool Write(ulong sector, uint count, byte[] data);
    }
}
