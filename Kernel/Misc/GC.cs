#if HasGC
using System;
using System.Diagnostics;

namespace Kernel.Misc
{
    public unsafe static class GC
    {
        [Flags]
        public enum Flags : byte 
        {
            Fixed = 0b1000_0000,
            NeedsToBeCollected = 0b0100_0000,
        }

        public static bool AllowCollect;

        public static void Collect()
        {
            lock (null)
            {
                ulong memSaved = 0;
                ulong counter = 0;
                for (ulong i = 0; i < Allocator.NumPages; i++)
                {
                    if (Allocator._Info.Pages[i] == 0) continue;
                    if (Allocator._Info.Pages[i] == Allocator.PageSignature) continue;
                    if (((Flags)Allocator._Info.GCInfos[i]).HasFlag(Flags.Fixed)) continue;

                    ulong addr = (ulong)(Allocator._Info.Start + (i * Allocator.PageSize));
                    ulong* page = PageTable.GetPage(addr);
                    if (BitHelpers.IsBitSet(*page, 5)) //Accessed bit
                    {
                        *page &= ~(1UL << 5);
                        Allocator._Info.GCInfos[i] = 0;
                    }
                    else
                    {
                        Allocator._Info.GCInfos[i]++;
                    }

                    ulong pages = Allocator._Info.Pages[i];

                    if (((Flags)Allocator._Info.GCInfos[i]).HasFlag(Flags.NeedsToBeCollected))
                    {
                        counter++;
                        memSaved += Allocator.Free((IntPtr)addr);
                    }

                    i += pages;
                }
                if (memSaved != 0)
                {
                    Console.Write("GC Collected: ");
                    Console.Write(counter.ToString());
                    Console.Write(" Unused Handle(s) ");
                    Console.Write((memSaved / 1048576).ToString());
                    Console.WriteLine("MiB");
                }
            }
        }
    }
}
#endif