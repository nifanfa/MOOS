#if HasGC
using System;
using System.Diagnostics;

namespace Kernel.Misc
{
    public unsafe static class GC
    {
        public static sbyte CollectIf;
        public static bool AllowCollect;
        public static sbyte NotCollectIf;

        public static void Collect()
        {
            ulong addr;
            ulong memSaved = 0;
            ulong counter = 0;
            for (ulong i = 0; i < Allocator.NumPages; i++)
            {
                if (Allocator._Info.Pages[i] == 0) continue;
                if (Allocator._Info.Pages[i] == Allocator.PageSignature) continue;
                if (Allocator._Info.GCInfos[i] == NotCollectIf) continue;

                addr = (ulong)(Allocator._Info.Start + (i * Allocator.PageSize));
                ulong* page = PageTable.GetPage(addr);
                if (BitHelpers.IsBitSet(*page, 5)) //Accessed bit
                {
                    *page &= ~(1UL << 5);
                    Allocator._Info.GCInfos[i]++;
                }
                else
                {
                    Allocator._Info.GCInfos[i]--;
                }

                if (Allocator._Info.GCInfos[i] < CollectIf)
                {
                    counter++;
                    memSaved += Allocator.Free((IntPtr)addr);
                }

                i += Allocator._Info.Pages[i];
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
    }
}
#endif