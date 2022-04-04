/*
 * Copyright(c) 2022 nifanfa, This code is part of the Solution1 licensed under the MIT licence.
 */
namespace Kernel.FS
{
    public abstract class Disk
    {
        public abstract bool Read(ulong sector, uint count, byte[] data);
        public abstract bool Write(ulong sector, uint count, byte[] data);
    }
}
