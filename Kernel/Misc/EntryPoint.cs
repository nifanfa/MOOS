using Internal.Runtime.CompilerHelpers;
using MOOS;
using MOOS.Driver;
using MOOS.FS;
using MOOS.Graph;
using MOOS.Misc;
using MOOS.NET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MOOS.Misc
{
    internal static unsafe class EntryPoint
    {
        [RuntimeExport("Entry")]
        public static void Entry(MultibootInfo* Info, IntPtr Modules, IntPtr Trampoline)
        {
            Allocator.Initialize((IntPtr)0x20000000);

            StartupCodeHelpers.InitializeModules(Modules);

            PageTable.Initialise();

            ASC16.Initialise();
            VBE.Initialise((VBEInfo*)Info->VBEInfo);
            Console.Setup();
            IDT.Disable();
            GDT.Initialise();
            IDT.Initialize();
            Interrupts.Initialize();
            IDT.Enable();

            SSE.enable_sse();
            //AVX.init_avx();

            ACPI.Initialize();
#if UseAPIC
            PIC.Disable();
            LocalAPIC.Initialize();
            IOAPIC.Initialize();
            HPET.Initialize();
#else
        PIC.Enable();
#endif

            PS2Keyboard.Initialize();
            //Enable keyboard interrupts
            Interrupts.EnableInterrupt(0x21);

            Serial.Initialise();
            PIT.Initialise();
            PS2Mouse.Initialise();
            SMBIOS.Initialise();

            PCI.Initialise();

            IDE.Initialize();
            SATA.Initialize();

            ThreadPool.Initialize();

            Console.WriteLine($"Trampoline: 0x{((ulong)Trampoline).ToString("x2")}");
            Native.Movsb((byte*)SMP.Trampoline, (byte*)Trampoline, 512);

            SMP.Initialize((uint)SMP.Trampoline);

#if HasGC
            GC.AllowCollect = true;
#endif

            //Only fixed size vhds are supported!
            Console.Write("Initrd: 0x");
            Console.WriteLine((Info->Mods[0]).ToString("x2"));
            Console.WriteLine("Initializing Ramdisk");
            new Ramdisk((IntPtr)(Info->Mods[0]));
            new FATFS();

            KMain();
        }

        [DllImport("*")]
        public static extern void KMain();
    }
}
