using Internal.Runtime.CompilerHelpers;
using Internal.Runtime.CompilerServices;
using Kernel;
using System;
using System.Runtime;
using System.Runtime.InteropServices;

unsafe class Program
{
    public const int ImageBase = 0x110000; //Do not modify

    [RuntimeExport("Main")]
    static void Main()
    {
        Console.Setup();

        #region Initializations
        DOSHeader* doshdr = (DOSHeader*)ImageBase;
        NtHeaders64* nthdr = (NtHeaders64*)(ImageBase + doshdr->e_lfanew);
        SectionHeader* sections = ((SectionHeader*)(ImageBase + doshdr->e_lfanew + sizeof(NtHeaders64)));
        IntPtr moduleSeg = IntPtr.Zero;
        for (int i = 0; i < nthdr->FileHeader.NumberOfSections; i++) 
        {
            Console.Write("Loading ");
            for(int k = 0; k < 8; k++)
            {
                Console.Write((char)sections[i].Name[k]);
            }
            Console.WriteLine();
            if (*(ulong*)sections[i].Name == 0x73656C75646F6D2E) moduleSeg = (IntPtr)(ImageBase + sections[i].VirtualAddress);
            Native.Movsb((void*)(ImageBase + sections[i].VirtualAddress), (void*)(ImageBase + sections[i].PointerToRawData), sections[i].SizeOfRawData);
        }

        //                 10MiB                 512MiB                1MiB
        for (uint i = 1024 * 1024 * 10; i < 1024 * 1024 * 512; i += 1024 * 1024)
        {
            //                                      1MiB / 4KiB
            Allocator.AddFreePages((System.IntPtr)(i), 256);
        }

        StartupCodeHelpers.InitialiseRuntime(moduleSeg);
        #endregion

        IDT.Disable();
        GDT.Initialise();
        IDT.Initialise();
        IDT.Enable();
        Serial.Initialise();
        PageTable.Initialise();

        Serial.WriteLine("Hello World");
        Console.WriteLine("Hello, World!");
        Console.WriteLine("Use Native AOT (Core RT) Technology.");

        /*
        BGA.Setup();
        BGA.SetVideoMode(640, 480);
        BGA.Clear(0xFFFF0000);
        */

        for (; ; );
    }
}
