#if Kernel
using Internal.Runtime.CompilerHelpers;
using MOOS;
using MOOS.Misc;

namespace System.Diagnostics
{
    public static unsafe class Process
    {
        public static void Start(byte[] exe)
        {
            fixed (byte* ptr = exe)
            {
                DOSHeader* doshdr = (DOSHeader*)ptr;
                NtHeaders64* nthdr = (NtHeaders64*)(ptr + doshdr->e_lfanew);

                if (!nthdr->OptionalHeader.BaseRelocationTable.VirtualAddress) Panic.Error("[Process.Start] Invalid Base Relocation Table");
                if (nthdr->OptionalHeader.ImageBase != 0x140000000) Panic.Error("[Process.Start] Invalid Base Address");

                byte* newPtr = (byte*)Allocator.Allocate(nthdr->OptionalHeader.SizeOfImage + 0x1000);
                Native.Movsb(newPtr, ptr, (ulong)exe.Length);
                LoadUnfixedPE(newPtr);
            }
        }

        public static void LoadUnfixedPE(byte* ptr) 
        {
            DOSHeader* doshdr = (DOSHeader*)ptr;
            NtHeaders64* nthdr = (NtHeaders64*)(ptr + doshdr->e_lfanew);
            ulong orignalBase = nthdr->OptionalHeader.ImageBase;
            nthdr->OptionalHeader.ImageBase = (ulong)ptr;
            SectionHeader* sections = ((SectionHeader*)(ptr + doshdr->e_lfanew + sizeof(NtHeaders64)));
            IntPtr moduleSeg = IntPtr.Zero;
            for (int i = 0; i < nthdr->FileHeader.NumberOfSections; i++)
            {
                if (*(ulong*)sections[i].Name == 0x73656C75646F6D2E) moduleSeg = (IntPtr)(nthdr->OptionalHeader.ImageBase + sections[i].VirtualAddress);
                Native.Movsb((void*)(nthdr->OptionalHeader.ImageBase + sections[i].VirtualAddress), ptr + sections[i].PointerToRawData, sections[i].SizeOfRawData);
            }
            FixImageRelocations(doshdr, nthdr, (long)(nthdr->OptionalHeader.ImageBase - orignalBase));
            delegate*<void> p = (delegate*<void>)(nthdr->OptionalHeader.ImageBase + nthdr->OptionalHeader.AddressOfEntryPoint);
            //TO-DO disposing
            StartupCodeHelpers.InitializeModules(moduleSeg);
            p();
            Allocator.Free((IntPtr)ptr);
        }

        static void FixImageRelocations(DOSHeader* dos_header, NtHeaders64* nt_header, long delta)
        {
            ulong size;
            long* intruction;
            DataDirectory* reloc_block =
                (DataDirectory*)(nt_header->OptionalHeader.BaseRelocationTable.VirtualAddress +
                    (ulong)dos_header);

            while (reloc_block->VirtualAddress)
            {
                size = (ulong)((reloc_block->Size - sizeof(DataDirectory)) / sizeof(ushort));
                ushort* fixup = (ushort*)((ulong)reloc_block + (ulong)sizeof(DataDirectory));
                for (ulong i = 0; i < size; i++, fixup++)
                {
                    if (10 == *fixup >> 12)
                    {
                        intruction = (long*)(reloc_block->VirtualAddress + (ulong)dos_header + (*fixup & 0xfffu));
                        *intruction += delta;
                    }
                }
                reloc_block = (DataDirectory*)(reloc_block->Size + (ulong)reloc_block);
            }
        }
    }
}
#endif
