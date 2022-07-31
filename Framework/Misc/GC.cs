#if HasGC
using System;

namespace MOOS.Misc
{
	public static unsafe class GC
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
					if (Allocator._Info.Pages[i] == 0)
					{
						continue;
					}

					if (Allocator._Info.Pages[i] == Allocator.PageSignature)
					{
						continue;
					}

					if (((Flags)Allocator._Info.GCInfos[i]).HasFlag(Flags.Fixed))
					{
						continue;
					}

					ulong addr = (ulong)(Allocator._Info.Start + (i * Allocator.PageSize));
					ulong* page = PageTable.GetPage(addr);
					if (BitHelpers.IsBitSet(*page, 5)) //Accessed bit
					{
						*page &= ~(1UL << 5);
						Allocator._Info.GCInfos[i] = 0;
					} else
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
			}
		}
	}
}
#endif