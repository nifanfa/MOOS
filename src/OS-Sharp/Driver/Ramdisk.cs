// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the  MIT licence
using System;

namespace Kernel.FileSystem
{
    public unsafe class Ramdisk : Disk
    {
        byte* ptr;

        public Ramdisk(IntPtr _ptr)
        {
            ptr = (byte*)_ptr;
        }

        public override bool Read(ulong sector, uint count, byte[] data) 
        {
            fixed(byte* p = data) 
            {
                Native.Movsb(p, ptr + (sector * 512), 512 * count);
            }
            return true;
        }

        public override bool Write(ulong sector, uint count, byte[] data)
        {
            fixed (byte* p = data)
            {
                Native.Movsb(ptr + (sector * 512), p, 512 * count);
            }
            return true;
        }
    }
}
