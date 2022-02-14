using System;
using System.Runtime;
using System.Runtime.InteropServices;

abstract unsafe class Allocator
{
    internal static unsafe void ZeroFill(IntPtr data, ulong size)
    {
        Native.Stosb((void*)data, 0, size);
    }


    unsafe struct BitMap
    {
        public fixed byte Data[BITMAP_LENGTH];
    }

    const int BITMAP_LENGTH = 1024 * 16; // 16kb can hold enough bits to represent 131,072 pages, allowing up to 512mb to be allocated (assuming a page size of 4kb)

    internal static IntPtr Reallocate(IntPtr ptr, ulong size)
    {
        return krealloc(ptr, size);
    }

    static BitMap bitMap;
    static int totalPages = 0;
    static uint allocations = 0;


    public static unsafe bool AddFreePages(IntPtr address, uint pages)
    {
        var startPage = (int)((ulong)address / 4096);

        for (int i = 0; i < pages; i++)
        {
            if (totalPages < startPage + i)
                totalPages = startPage + i;

            var index = (startPage + i) / 8;
            var bit = (startPage + i) % 8;

            bitMap.Data[index] |= (byte)(1 << bit);
        }

        return true;
    }

    [RuntimeExport("liballoc_lock")]
    private static int Lock()
    {
        if (IDT.Initialised)
            IDT.Disable();

        return 0;
    }

    [RuntimeExport("liballoc_unlock")]
    private static int Unlock()
    {
        if (IDT.Initialised)
            IDT.Enable();

        return 0;
    }

    [RuntimeExport("liballoc_alloc")]
    private static unsafe IntPtr Alloc(ulong pages)
    {
        var count = 0;
        var index = 0;

        for (; ; )
        {
            if (index >= totalPages)
                return IntPtr.Zero;

            if (count == (int)pages)
            {
                for (int i = 0; i < (int)pages; i++)
                {
                    var _off = (index + i) / 8;
                    var _bit = (index + i) % 8;
                    bitMap.Data[_off] &= (byte)~(1 << _bit);
                }

                allocations++;

                return (IntPtr)(index * 4096);
            }

            if (count == 0)
            {
                for (; ; )
                {
                    var _off = index / 8;
                    var _bit = index % 8;

                    if ((bitMap.Data[_off] & (1 << _bit)) == (1 << _bit))
                        break;

                    index++;
                }
            }

            var off = (index + count) / 8;
            var bit = (index + count) % 8;

            if ((bitMap.Data[off] & (1 << bit)) != (1 << bit))
                count = 0;

            count++;
        }
    }

    [RuntimeExport("liballoc_free")]
    private static unsafe int Free(IntPtr ptr, ulong pages)
    {
        AddFreePages(ptr, (uint)pages);
        allocations--;

        return 0;
    }

    public static ulong TotalMemory
        => (ulong)totalPages * 4096;

    public static unsafe ulong UsedMemory
    {
        get
        {
            var r = 0UL;

            for (int i = 0; i < totalPages / 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((bitMap.Data[i] & (1 << j)) != (1 << j))
                        r++;
                }
            }

            return r * 4096;
        }
    }

    public static ulong FreeMemory
        => TotalMemory - UsedMemory;

    public static ulong Allocations
        => (ulong)allocations;

    internal static unsafe void ZeroMemory(IntPtr data, ulong size)
    {
        Native.Stosb((void*)data, 0, size);
    }

    internal static void Free(IntPtr intPtr)
    {
        kfree(intPtr);
    }

    internal static unsafe IntPtr Allocate(ulong size)
    {
        return Allocate_Aligned(size,0x1000);
    }

    internal static unsafe IntPtr Allocate_Aligned(ulong size, ulong alignment)
    {
        ulong offset = alignment - 1 + (ulong)sizeof(void*);
        void* p1 = (void*)kmalloc(size + offset);
        void** p2 = (void**)(((ulong)p1 + offset) & ~(alignment - 1));
        p2[-1] = p1;
        return (IntPtr)p2;
    }

    internal static unsafe void CopyMemory(IntPtr dst, IntPtr src, ulong size)
    {
        Native.Movsb((void*)dst, (void*)src, size);
    }

    [DllImport("*")]
    private static extern IntPtr kmalloc(ulong size);

    [DllImport("*")]
    private static extern IntPtr krealloc(IntPtr ptr, ulong size);

    [DllImport("*")]
    private static extern void kfree(IntPtr ptr);

    internal static unsafe void MemoryCopy(IntPtr dst, IntPtr src, ulong size)
    {
        Native.Movsb((void*)dst, (void*)src, size);
    }
}