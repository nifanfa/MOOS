using System;

abstract unsafe class Heap
{
    internal static unsafe void ZeroMemory(IntPtr data, ulong size)
    {
        Native.Stosb((void*)data, 0, size);
    }

    internal static void Free(IntPtr intPtr)
    {
        ulong p = (ulong)intPtr;
        if (p < (ulong)_Info.Start) return;
        p -= (ulong)_Info.Start;
        if ((p % PageSize) != 0) return;
        /*
         * This will get wrong if the size is larger than PageSize
         * and however the allocated address should be aligned
         */
        //p &= ~PageSize; 
        p /= PageSize;
        ulong pages = _Info.Pages[p] & ~(1UL << 63);
        if (pages != 0)
        {
            for (ulong i = 0; i < pages; i++)
                _Info.Pages[p + i] = 0;
        }
    }

    /*
     * NumPages = Memory Size / PageSize
     * This should be a const because there will be allocations during initializing modules 👇_Info
     */
    public const int NumPages = 131072;
    public const ulong PageSize = 4096;

    public struct Info
    {
        public IntPtr Start;
        public fixed ulong Pages[NumPages]; //Max 512MiB
    }

    public static Info _Info;

    public static void Initialize(IntPtr Start)
    {
        _Info.Start = Start;
    }

    //If bit63 is set so it means the page is using
    internal static unsafe IntPtr Allocate(ulong size)
    {
        ulong pages = 1;

        if (size > PageSize)
        {
            pages = (size / PageSize) + ((size % 4096) != 0 ? 1UL : 0);
        }

        ulong i = 0;
        bool found = false;

        for (i = 0; i < NumPages; i++)
        {
            if (!BitHelpers.IsBitSet(_Info.Pages[i], 63))
            {
                found = true;
                for (ulong k = 0; k < pages; k++)
                {
                    if (BitHelpers.IsBitSet(_Info.Pages[i + k], 63))
                    {
                        found = false;
                        break;
                    }
                }
                if (found) break;
            }
        }
        if (!found) return IntPtr.Zero;

        for (ulong k = 0; k < pages; k++)
        {
            _Info.Pages[i + k] |= (1UL << 63);
        }
        _Info.Pages[i] |= pages;

        IntPtr ptr = _Info.Start + (i * PageSize);
        return ptr;
    }

    internal static unsafe IntPtr AlignedAllocate(ulong size, ulong alignment)
    {
        ulong offset = alignment - 1 + (ulong)sizeof(void*);
        void* p1 = (void*)Allocate(size + offset);
        void** p2 = (void**)(((ulong)p1 + offset) & ~(alignment - 1));
        p2[-1] = p1;
        return (IntPtr)p2;
    }

    internal static unsafe void MemoryCopy(IntPtr dst, IntPtr src, ulong size)
    {
        Native.Movsb((void*)dst, (void*)src, size);
    }
}