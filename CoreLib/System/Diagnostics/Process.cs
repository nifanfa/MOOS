using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using Internal.Runtime.CompilerHelpers;

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

                if (!nthdr->OptionalHeader.BaseRelocationTable.VirtualAddress)
                {
                    return;
                }

                if (nthdr->OptionalHeader.ImageBase != 0x140000000)
                {
                    return;
                }

                byte* newPtr = (byte*)malloc(nthdr->OptionalHeader.SizeOfImage + 0x1000);
                memcpy(newPtr, ptr, (ulong)exe.Length);
                LoadUnfixedPE(newPtr);
            }
        }

        [DllImport("*")]
        private static extern nint malloc(ulong size);

        [DllImport("*")]
        private static extern ulong free(nint ptr);

        public static void LoadUnfixedPE(byte* ptr)
        {
            DOSHeader* doshdr = (DOSHeader*)ptr;
            NtHeaders64* nthdr = (NtHeaders64*)(ptr + doshdr->e_lfanew);
            ulong orignalBase = nthdr->OptionalHeader.ImageBase;
            nthdr->OptionalHeader.ImageBase = (ulong)ptr;
            SectionHeader* sections = (SectionHeader*)(ptr + doshdr->e_lfanew + sizeof(NtHeaders64));
            IntPtr moduleSeg = IntPtr.Zero;
            for (int i = 0; i < nthdr->FileHeader.NumberOfSections; i++)
            {
                if (*(ulong*)sections[i].Name == 0x73656C75646F6D2E)
                {
                    moduleSeg = (IntPtr)(nthdr->OptionalHeader.ImageBase + sections[i].VirtualAddress);
                }

                memcpy((byte*)(nthdr->OptionalHeader.ImageBase + sections[i].VirtualAddress), ptr + sections[i].PointerToRawData, sections[i].SizeOfRawData);
            }
            FixImageRelocations(doshdr, nthdr, (long)(nthdr->OptionalHeader.ImageBase - orignalBase));
            delegate*<void> p = (delegate*<void>)(nthdr->OptionalHeader.ImageBase + nthdr->OptionalHeader.AddressOfEntryPoint);
            //TO-DO disposing
            StartupCodeHelpers.InitializeModules(moduleSeg);
            p();
            free((IntPtr)ptr);
        }

        [DllImport("*")]
        private static extern unsafe void memset(byte* ptr, int c, int count);

        [DllImport("*")]
        private static extern unsafe void memcpy(byte* dest, byte* src, ulong count);

        private static void FixImageRelocations(DOSHeader* dos_header, NtHeaders64* nt_header, long delta)
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