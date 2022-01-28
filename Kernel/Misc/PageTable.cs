namespace Kernel
{
    public static unsafe class PageTable
    {
        public enum PageSize
        {
            Typical = 4096,
            Huge = 0x200000
        }

        public static ulong* PML4;

        internal static void Initialise()
        {
            ulong p = (ulong)Allocator.Allocate_Aligned(0x200000, 0x1000);
            PML4 = (ulong*)p;

            Native.Stosb(PML4, 0x00, 4096);

            ulong i = 0;
            //Map the first 4KiB-2MiB
            //Reserve 4KiB for null reference exception
            for (i = (ulong)PageSize.Typical; i < (ulong)PageSize.Huge; i += (ulong)PageSize.Typical)
            {
                Map(i, i, PageSize.Typical);
            }
            //Map the first 2MiB-4GiB
            for (i = (ulong)PageSize.Huge; i < 1024UL * 1024UL * 1024UL * 4UL; i += (ulong)PageSize.Huge)
            {
                Map(i, i);
            }

            Native.WriteCR3(p);
        }

        /// <summary>
        /// Map Physical Address At Virtual Address Specificed
        /// </summary>
        /// <param name="VirtualAddress"></param>
        /// <param name="PhysicalAddress"></param>
        public static void Map(ulong VirtualAddress, ulong PhysicalAddress, PageSize pageSize = PageSize.Huge)
        {
            ulong pml4_entry = (VirtualAddress & ((ulong)0x1ff << 39)) >> 39;
            ulong pml3_entry = (VirtualAddress & ((ulong)0x1ff << 30)) >> 30;
            ulong pml2_entry = (VirtualAddress & ((ulong)0x1ff << 21)) >> 21;
            ulong pml1_entry = (VirtualAddress & ((ulong)0x1ff << 12)) >> 12;

            ulong* pml3 = Next(PML4, pml4_entry);
            ulong* pml2 = Next(pml3, pml3_entry);

            if (pageSize == PageSize.Huge)
            {
                pml2[pml2_entry] = PhysicalAddress | 0b10000011;
            }
            else if (pageSize == PageSize.Typical)
            {
                ulong* pml1 = Next(pml2, pml2_entry);
                pml1[pml1_entry] = PhysicalAddress | 0b11;
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
                p = AllocateTable();
                Directory[Entry] = (((ulong)p) & 0x000F_FFFF_FFFF_F000) | 0b11;
            }

            return p;
        }

        private static ulong p = 0;

        public static ulong* AllocateTable()
        {
            ulong* r = (PML4 + 4096) + (p * 4096);
            Native.Stosb(r, 0x00, 4096);
            p++;
            return r;
        }
    }
}