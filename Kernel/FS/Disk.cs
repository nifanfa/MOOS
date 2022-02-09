namespace Kernel.FS
{
    public abstract class Disk
    {
        public abstract bool Read(ulong sector, uint count, byte[] data);
        public abstract bool Write(ulong sector, uint count, byte[] data);
    }
}
