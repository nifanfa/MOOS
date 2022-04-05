#if Kernel
//https://www.52pojie.cn/thread-858841-1-1.html
using Internal.Runtime.CompilerHelpers;
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
                DOSHeader* doshdr = (DOSHeader*)ptr;
                NtHeaders64* nthdr = (NtHeaders64*)(ptr + doshdr->e_lfanew);
                SectionHeader* sections = ((SectionHeader*)(ptr + doshdr->e_lfanew + sizeof(NtHeaders64)));
                IntPtr moduleSeg = IntPtr.Zero;
                for (int i = 0; i < nthdr->FileHeader.NumberOfSections; i++)
                {
                    if (*(ulong*)sections[i].Name == 0x73656C75646F6D2E) moduleSeg = (IntPtr)(nthdr->OptionalHeader.ImageBase + sections[i].VirtualAddress);
                    Native.Movsb((void*)(nthdr->OptionalHeader.ImageBase + sections[i].VirtualAddress), ptr + sections[i].PointerToRawData, sections[i].SizeOfRawData);
                }
                delegate*<void> p = (delegate*<void>)(nthdr->OptionalHeader.ImageBase + nthdr->OptionalHeader.AddressOfEntryPoint);
                StartupCodeHelpers.InitializeModules(moduleSeg);
                p();
            }
        }
    }
}
#endif
