using System;
using System.Runtime;
using Internal.Runtime.CompilerHelpers;
using Kernel.Driver;
using Kernel.FS;
//using Kernel.Misc;

namespace Kernel
{
    public unsafe class Program
    {
        private static void Main() { }

        [RuntimeExport("Main")]
        private static void Main(MultibootInfo* Info, IntPtr Modules)
        {
            Allocator.Initialize((IntPtr)0x6000000);
            StartupCodeHelpers.InitializeModules(Modules);
            PageTable.Initialize();
            VBE.Initialize((VBEInfo*)Info->VBEInfo);
            Console.Initialize();
            IDT.Disable();
            InitMsg(GDT.Initialize(), "GDT Successfully Initialized", "GDT Initialization Failure");
            InitMsg(IDT.Initialize(), "IDT Successfully Initialized", "IDT Initialization Failure");
            IDT.Enable();
            InitMsg(ACPI.Initialize(), "ACPI Successfully Initialized", "ACPI Initialization Failure");
            PIC.Disable();
            InitMsg(LocalAPIC.Initialize(), "LocalAPIC Successfully Initialized", "LocalAPIC Initialization Failure");
            InitMsg(IOAPIC.Initialize(), "IOAPIC Successfully Initialized", "IOAPIC Initialization Failure");
            InitMsg(HPET.Initialize(), "HPET Successfully Initialized", " Initialization Failure");
            InitMsg(PS2Keyboard.Initialize(), "PS2Keyboard Successfully Initialized", "PS2Keyboard Initialization Failure");
            IOAPIC.SetEntry(0x21);
            InitMsg(Serial.Initialize(), "Serial Interface Successfully Initialized", "Serial Interface Initialization Failure");
            InitMsg(PIT.Initialize(), "PIT Successfully Initialized", "PIT Initialization Failure");
            InitMsg(PS2Mouse.Initialize(), "PS2Mouse Successfully Initialized", "PS2Mouse Initialization Failure");
            InitMsg(SMBIOS.Initialize(), "SMBIOS Successfully Initialized", "SMBIOS Initialization Failure");
            InitMsg(PCI.Initialize(), "PCI Successfully Initialized", "PCI Initialization Failure");
            InitMsg(SATA.Initialize(), "SATA Successfully Initialized", "SATA Initialization Failure");
            InitMsg(IDE.Initialize(), "IDE Successfully Initialized", "IDE Initialization Failure");
            Allocator.AllowCollect = true;
            FAT32 fs = new(new Ramdisk((IntPtr)Info->Mods[0]), 2048);

            for (; ; )
            {
                ;
            }
        }

        private static void InitMsg(bool initfunc, string smsg, string fmsg)
        {
            Console.Write("[");
            Console.ForegroundColor = initfunc ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write(initfunc ? "   OK   " : " FAILURE ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] ");
            Console.WriteLine($" {(initfunc ? smsg : fmsg)}");
        }
    }
}