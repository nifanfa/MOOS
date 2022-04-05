#if Kernel
//https://www.52pojie.cn/thread-858841-1-1.html
using Kernel;
using Kernel.Misc;

namespace System.Diagnostics
{
    public static unsafe class Process
    {
        /// <summary>
        /// TO-DO disposing
        /// </summary>
        /// <param name="exe"></param>
        public static void Start(byte[] exe)
        {
            fixed (byte* ptr = exe)
            {
                DOSHeader* pDos = (DOSHeader*)ptr;
                NtHeaders64* pNt = (NtHeaders64*)(ptr + pDos->e_lfanew);
                if(pNt->OptionalHeader.BaseRelocationTable.VirtualAddress == 0) 
                {
                    Panic.Error("Base-address fixed exe is not supported");
                }

                byte* mem = (byte*)Allocator.Allocate(pNt->OptionalHeader.SizeOfImage);
                Native.Stosb(mem, 0, pNt->OptionalHeader.SizeOfImage);

                Native.Movsb(mem, ptr, (ulong)exe.Length);

                //Load sections
                SectionHeader* sections = ((SectionHeader*)(mem + pDos->e_lfanew + sizeof(NtHeaders64)));
                IntPtr moduleSeg = IntPtr.Zero;
                for (int i = 0; i < pNt->FileHeader.NumberOfSections; i++)
                {
                    if (*(ulong*)sections[i].Name == 0x73656C75646F6D2E) moduleSeg = (IntPtr)(mem + sections[i].VirtualAddress);
                    Native.Movsb((void*)(mem + sections[i].VirtualAddress), mem + sections[i].PointerToRawData, sections[i].SizeOfRawData);
                }

                //Fix relocation
                DataDirectory pLoc = pNt->OptionalHeader.BaseRelocationTable;
                if(pLoc.VirtualAddress != 0) 
                {
                    while ((pLoc.VirtualAddress + pLoc.Size) != 0)
                    {
                        ushort* pLocData = (ushort*)((byte*)&pLoc + sizeof(DataDirectory));
                        int nNumberOfReloc = (int)((pLoc.Size - sizeof(DataDirectory)) / sizeof(ushort));

                        for (int i = 0; i < nNumberOfReloc; i++)
                        {
                            if ((uint)(pLocData[i] & 0x0000F000) == 0x00003000)
                            {
                                uint* pAddress = (uint*)((byte*)pDos + pLoc.VirtualAddress + (pLocData[i] & 0x0FFF));
                                uint dwDelta = (uint)(pDos - pNt->OptionalHeader.ImageBase);
                                *pAddress += dwDelta;
                            }
                        }

                        pLoc = *(&pLoc + pLoc.Size);
                    }
                }

                delegate*<void> p = (delegate*<void>)(mem + pNt->OptionalHeader.AddressOfEntryPoint);
                p();
            }
        }
    }
}
#endif
