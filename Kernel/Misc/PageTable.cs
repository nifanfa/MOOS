using MOOS.Misc;

namespace MOOS
{
    public static unsafe class PageTable
    {
        public enum PageSize
        {
            Typical = 4096,
            //Huge = 0x200000
        }

        public static ulong* PML4;

        internal static void Initialise()
        {
            PML4 = (ulong*)SMP.SharedPageTable;

            Native.Stosb(PML4, 0, 0x1000);

            ulong i = 0;
            //Map the first 4KiB-4GiB
            //Reserve 4KiB for null reference exception
            for (i = (ulong)PageSize.Typical; i < 1024 * 1024 * 1024 * 4UL; i += (ulong)PageSize.Typical)
            {
                Map(i, i, PageSize.Typical);
            }

            Native.WriteCR3((ulong)PML4);
        }

        public static ulong* GetPage(ulong VirtualAddress, PageSize pageSize = PageSize.Typical)
        {
            if((VirtualAddress % (ulong)PageSize.Typical) != 0) Panic.Error("Invalid address");

            ulong pml4_entry = (VirtualAddress & ((ulong)0x1ff << 39)) >> 39;
            ulong pml3_entry = (VirtualAddress & ((ulong)0x1ff << 30)) >> 30;
            ulong pml2_entry = (VirtualAddress & ((ulong)0x1ff << 21)) >> 21;
            ulong pml1_entry = (VirtualAddress & ((ulong)0x1ff << 12)) >> 12;

            ulong* pml3 = Next(PML4, pml4_entry);
            ulong* pml2 = Next(pml3, pml3_entry);

            /*
            if (pageSize == PageSize.Huge)
            {
                return &pml2[pml2_entry];
            }
            else 
            */
            if (pageSize == PageSize.Typical)
            {
                ulong* pml1 = Next(pml2, pml2_entry);
                return &pml1[pml1_entry];
            }
            return null;
        }

        /// <summary>
        /// Map Physical Address At Virtual Address Specificed
        /// </summary>
        /// <param name="VirtualAddress"></param>
        /// <param name="PhysicalAddress"></param>
        public static void Map(ulong VirtualAddress, ulong PhysicalAddress, PageSize pageSize = PageSize.Typical)
        {
            /*
            if (pageSize == PageSize.Huge)
            {
                *GetPage(VirtualAddress, pageSize) = PhysicalAddress | 0b10000011;
            }
            else 
            */
            if (pageSize == PageSize.Typical)
            {
                *GetPage(VirtualAddress, pageSize) = PhysicalAddress | 0b11;
            }

            Native.Invlpg(PhysicalAddress);
        }

        public static ulong* Next(ulong* Directory, ulong Entry)
        {
            ulong* p = null;

            if (((Directory[Entry]) & 0x01) != 0)
            {
                p = (ulong*)(Directory[Entry] & 0x000F_FFFF_FFFF_F000);
            }
            else
            {
                p = (ulong*)Allocator.Allocate(0x1000);
                Native.Stosb(p, 0, 0x1000);

                Directory[Entry] = (((ulong)p) & 0x000F_FFFF_FFFF_F000) | 0b11;
            }

            return p;
        }
    }
}