// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence
using System;
using System.Diagnostics;
using Internal.Runtime.CompilerServices;
using OS_Sharp;
using OS_Sharp.Misc;

/// <summary>
/// Nifanfa's Super Fast Memory Allocation Lib
/// </summary>
internal abstract unsafe class Allocator
{
    internal static unsafe void ZeroFill(IntPtr data, ulong size)
    {
        Native.Stosb((void*)data, 0, size);
    }

    public static sbyte CollectIf;
    public static bool AllowCollect;
    public static sbyte NotCollectIf;

    public static void Collect()
    {
        ulong addr;
        ulong memSaved = 0;
        ulong counter = 0;
        for (ulong i = 0; i < NumPages; i++)
        {
            if (_Info.Pages[i] == 0)
            {
                continue;
            }

            if (_Info.Pages[i] == PageSignature)
            {
                continue;
            }

            if (_Info.GCInfos[i] == NotCollectIf)
            {
                continue;
            }

            addr = (ulong)(_Info.Start + (i * PageSize));
            ulong* page = PageTable.GetPage(addr);
            if (BitHelpers.IsBitSet(*page, 5)) //Accessed bit
            {
                *page &= ~(1UL << 5);
                _Info.GCInfos[i]++;
            }
            else
            {
                _Info.GCInfos[i]--;
            }

            if (_Info.GCInfos[i] < CollectIf)
            {
                counter++;
                memSaved += Free((IntPtr)addr);
            }

            i += _Info.Pages[i];
        }
        if (memSaved != 0)
        {
            Debug.Write("GC Collected: ");
            Debug.Write(counter.ToString());
            Debug.Write(" Unused Handle(s) ");
            Debug.Write((memSaved / 1048576).ToString());
            Debug.WriteLine("MiB");
        }
    }

    /*
    public static void KeepAlive(object obj)
    {
        ulong p = GetPageIndexStart((IntPtr)Unsafe.AsPointer(ref obj));
        if (p == 0xFFFFFFFFFFFFFFFF)
        {
            return;
        }

        _Info.GCInfos[p] = NotCollectIf;
    }
    */

    private static ulong GetPageIndexStart(IntPtr ptr)
    {
        ulong p = (ulong)ptr;
        if (p < (ulong)_Info.Start)
        {
            return 0xFFFFFFFFFFFFFFFF;
        }

        p -= (ulong)_Info.Start;
        if ((p % PageSize) != 0)
        {
            return 0xFFFFFFFFFFFFFFFF;
        }
        /*
* This will get wrong if the size is larger than PageSize
* and however the allocated address should be aligned
*/
        //p &= ~PageSize; 
        p /= PageSize;
        return p;
    }

    internal static ulong Free(IntPtr intPtr)
    {
        ulong p = GetPageIndexStart(intPtr);
        if (p == 0xFFFFFFFFFFFFFFFF)
        {
            return 0;
        }

        ulong pages = _Info.Pages[p];
        if (pages != 0 && pages != PageSignature)
        {
            _Info.PageInUse -= pages;
            Native.Stosb((void*)intPtr, 0, pages * PageSize);
            for (ulong i = 0; i < pages; i++)
            {
                _Info.Pages[p + i] = 0;
                _Info.GCInfos[p + i] = 0;
            }

            _Info.Pages[p] = 0;
            _Info.GCInfos[p] = 0;
            return pages * PageSize;
        }
        return 0;
    }

    public static ulong MemoryInUse => _Info.PageInUse * PageSize;

    public const ulong PageSignature = 0x2E61666E6166696E;

    /*
     * NumPages = Memory Size / PageSize
     * This should be a const because there will be allocations during initializing modules ??_Info
     */
    public const int NumPages = 131072;
    public const ulong PageSize = 4096;

    public struct Info
    {
        public IntPtr Start;
        public ulong PageInUse;
        public fixed ulong Pages[NumPages]; //Max 512MiB
        public fixed sbyte GCInfos[NumPages]; //Max 512MiB
    }

    public static Info _Info;

    public static void Initialize(IntPtr Start)
    {
        fixed (Info* pInfo = &_Info)
        {
            Native.Stosb(pInfo, 0, (ulong)sizeof(Info));
        }

        CollectIf = -12;
        NotCollectIf = 127;
        _Info.Start = Start;
        _Info.PageInUse = 0;
        AllowCollect = false;
    }

    /// <summary>
    /// Returns a 4KB aligned address
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
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
            if (_Info.Pages[i] == 0)
            {
                found = true;
                for (ulong k = 0; k < pages; k++)
                {
                    if (_Info.Pages[i + k] != 0)
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    break;
                }
            }
            else if (_Info.Pages[i] != PageSignature)
            {
                i += _Info.Pages[i];
            }
        }
        if (!found)
        {
            Panic.Error("Memory leak");
            return IntPtr.Zero;
        }

        for (ulong k = 0; k < pages; k++)
        {
            _Info.Pages[i + k] = PageSignature;
            _Info.GCInfos[i + k] = AllowCollect ? (sbyte)0 : NotCollectIf;
        }
        _Info.Pages[i] = pages;
        _Info.GCInfos[i] = AllowCollect ? (sbyte)0 : NotCollectIf;
        _Info.PageInUse += pages;

        IntPtr ptr = _Info.Start + (i * PageSize);
        return ptr;
    }

    public static IntPtr Reallocate(IntPtr intPtr, ulong size)
    {
        if (intPtr == IntPtr.Zero)
        {
            return Allocate(size);
        }

        if (size == 0)
        {
            Free(intPtr);
            return IntPtr.Zero;
        }

        ulong p = GetPageIndexStart(intPtr);
        if (p == 0xFFFFFFFFFFFFFFFF)
        {
            return intPtr;
        }

        ulong pages = 1;

        if (size > PageSize)
        {
            pages = (size / PageSize) + ((size % 4096) != 0 ? 1UL : 0);
        }

        if (_Info.Pages[p] == pages)
        {
            return intPtr;
        }

        IntPtr newptr = Allocate(size);
        MemoryCopy(newptr, intPtr, size);
        Free(intPtr);
        return newptr;
    }

    internal static unsafe void MemoryCopy(IntPtr dst, IntPtr src, ulong size)
    {
        Native.Movsb((void*)dst, (void*)src, size);
    }
}
