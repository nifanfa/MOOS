using Kernel.NET;
using System;
using System.Runtime;
using System.Runtime.InteropServices;

abstract class Memory
{
    internal static unsafe void Zero(IntPtr data, ulong size)
    {
        Native.Stosb((void*)data, 0, size);
    }

    internal static void Free(IntPtr intPtr)
    {
        //kfree(intPtr);
    }

    struct Info 
    {
        public IntPtr Current;
    }

    static Info _Info;

    public static void Initialize(IntPtr Start) 
    {
        _Info.Current = Start;
    }

    internal static unsafe IntPtr Allocate(ulong size)
    {
        IntPtr ptr = _Info.Current;
        _Info.Current += size;
        return ptr;
        //return kmalloc(size);
    }

    internal static unsafe IntPtr AlignedAllocate(ulong size, ulong alignment)
    {
        ulong offset = alignment - 1 + (ulong)sizeof(void*);
        void* p1 = (void*)Allocate(size + offset);
        void** p2 = (void**)(((ulong)p1 + offset) & ~(alignment - 1));
        p2[-1] = p1;
        return (IntPtr)p2;
    }

    internal static unsafe void Copy(IntPtr dst, IntPtr src, ulong size)
    {
        Native.Movsb((void*)dst, (void*)src, size);
    }
}