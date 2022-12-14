using Internal.Runtime.CompilerHelpers;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;

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

                if (!nthdr->OptionalHeader.BaseRelocationTable.VirtualAddress) return;
                if (nthdr->OptionalHeader.ImageBase != 0x140000000) return;

                byte* newPtr = (byte*)StartupCodeHelpers.malloc(nthdr->OptionalHeader.SizeOfImage);
                memset(newPtr, 0, (int)nthdr->OptionalHeader.SizeOfImage);
                memcpy(newPtr, ptr, nthdr->OptionalHeader.SizeOfHeaders);

                DOSHeader* newdoshdr = (DOSHeader*)newPtr;
                NtHeaders64* newnthdr = (NtHeaders64*)(newPtr + newdoshdr->e_lfanew);

                IntPtr moduleSeg = IntPtr.Zero;
                SectionHeader* sections = ((SectionHeader*)(newPtr + newdoshdr->e_lfanew + sizeof(NtHeaders64)));
                for (int i = 0; i < newnthdr->FileHeader.NumberOfSections; i++)
                {
                    if (*(ulong*)sections[i].Name == 0x73656C75646F6D2E) moduleSeg = (IntPtr)((ulong)newPtr + sections[i].VirtualAddress);
                    memcpy((byte*)((ulong)newPtr + sections[i].VirtualAddress), ptr + sections[i].PointerToRawData, sections[i].SizeOfRawData);
                }
                FixImageRelocations(newdoshdr, newnthdr, (long)((ulong)newPtr - newnthdr->OptionalHeader.ImageBase));

                delegate*<void> p = (delegate*<void>)((ulong)newPtr + newnthdr->OptionalHeader.AddressOfEntryPoint);
                //TO-DO disposing
                StartupCodeHelpers.InitializeModules(moduleSeg);
                StartThread(p);
                //StartupCodeHelpers.free((IntPtr)ptr);
            }
        }

        [DllImport("*")]
        static unsafe extern void memset(byte* ptr, int c, int count);

        [DllImport("*")]
        static unsafe extern void memcpy(byte* dest, byte* src, ulong count);

        [DllImport("StartThread")]
        static extern void StartThread(delegate*<void> func);

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