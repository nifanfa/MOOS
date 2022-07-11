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

#if HasGC
            GC.AllowCollect = false;
#endif

            PageTable.Initialise();

            GDT.Initialise();

            ASC16.Initialise();

            ACPI.Initialize();

            VBEInfo* vbe = (VBEInfo*)Info->VBEInfo;
            if (vbe->PhysBase != 0)
            {
                Framebuffer.VideoMemory = (uint*)vbe->PhysBase;
                Framebuffer.SetVideoMode(vbe->ScreenWidth, vbe->ScreenHeight);
                Framebuffer.Graphics.Clear(0x0);
            }
            else 
            {
                ACPI.Shutdown();
                for (; ; ) Native.Hlt();
            }

            Console.Setup();
            IDT.Disable();
            IDT.Initialize();
            Interrupts.Initialize();
            IDT.Enable();

            SSE.enable_sse();
            //AVX.init_avx();

#if UseAPIC
            PIC.Disable();
            LocalAPIC.Initialize();
            IOAPIC.Initialize();
#else
        PIC.Enable();
#endif

            Keyboard.Initialize();

            PS2Keyboard.Initialize();
            //Enable keyboard interrupts
            Interrupts.EnableInterrupt(0x21);

            Serial.Initialise();
            
            Timer.Initialize();

            PS2Mouse.Initialise();
            SMBIOS.Initialise();

            PCI.Initialise();

            IDE.Initialize();
            SATA.Initialize();

            ThreadPool.Initialize();

            Console.WriteLine($"[SMP] Trampoline: 0x{((ulong)Trampoline).ToString("x2")}");
            Native.Movsb((byte*)SMP.Trampoline, (byte*)Trampoline, 512);

            SMP.Initialize((uint)SMP.Trampoline);

#if HasGC
            GC.AllowCollect = true;
#endif

            //Only fixed size vhds are supported!
            Console.Write("[Initrd] Initrd: 0x");
            Console.WriteLine((Info->Mods[0]).ToString("x2"));
            Console.WriteLine("[Initrd] Initializing Ramdisk");
            new Ramdisk((IntPtr)(Info->Mods[0]));
            new FATFS();

            KMain();
        }

        [DllImport("*")]
        public static extern void KMain();
    }
}