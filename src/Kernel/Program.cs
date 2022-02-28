using System;
using System.Runtime;
using Internal.Runtime.CompilerHelpers;
using Kernel.Driver;
using Kernel.FileSystem;

namespace Kernel
{
    public unsafe class Program
    {
        // The compiler expects that a static Main method exists
        private static void Main() { }

        /**
         * Minimum system requirement:
         * 128MiB of RAM
         * Memory Map:
         * 64 MiB - 96MiB   -> System
         * 96 MiB - ∞     -> Free to use
        */
        [RuntimeExport("Main")]
        private static void Main(MultibootInfo* Info, IntPtr Modules)
        {
            // Initialization of drivers and classes
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

            //Enable keyboard interrupts
            IOAPIC.SetEntry(0x21);

            InitMsg(Serial.Initialize(), "Serial Interface Successfully Initialized", "Serial Interface Initialization Failure");
            InitMsg(PIT.Initialize(), "PIT Successfully Initialized", "PIT Initialization Failure");
            InitMsg(PS2Mouse.Initialize(), "PS2Mouse Successfully Initialized", "PS2Mouse Initialization Failure");
            InitMsg(SMBIOS.Initialize(), "SMBIOS Successfully Initialized", "SMBIOS Initialization Failure");
            InitMsg(PCI.Initialize(), "PCI Successfully Initialized", "PCI Initialization Failure");
            InitMsg(SATA.Initialize(), "SATA Successfully Initialized", "SATA Initialization Failure");
            InitMsg(IDE.Initialize(), "IDE Successfully Initialized", "IDE Initialization Failure");
            Allocator.AllowCollect = true;
            // Filesystem
            FAT32 fs =
            // Ramdisk
            new FAT32(new Ramdisk((IntPtr)Info->Mods[0]), 2048);
            // SATA Disk
            //new FAT32(SATA.Ports[0], 2048);
            // IDE Disk
            //new FAT32(IDE.Controllers[0], 2048);
            // Auto-Detect
            // new FAT32(SATA.Ports.Count ? SATA.Ports[0] : (IDE.Controllers.Count ? IDE.Controllers[0] : new Ramdisk((IntPtr)Info->Mods[0])), 2048);

            Serial.WriteLine("Hello from OS-Sharp!");
            Console.WriteLine("Hello, World!");
            Console.WriteLine("Using Native AOT (Core RT) Technology.");

            /*
            Console.WriteLine("Connecting to internet");
            ARP.Initialise();
            Network.Initialise(IPAddress.Parse(192, 168, 137, 188), IPAddress.Parse(192, 168, 137, 1));
            RTL8139.Initialise();
            ARP.Require(Network.Gateway);
            */
            for (; ; )
            {

            }
        }
        // Makes messages for the init methods
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