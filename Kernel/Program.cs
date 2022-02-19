using Internal.Runtime.CompilerHelpers;
using Kernel;
using Kernel.Driver;
using Kernel.FS;
using Kernel.GUI;
using Kernel.Misc;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Windows.Forms;

unsafe class Program
{
    static void Main() { }

    [DllImport("*")]
    public static extern void test();

    static Image Cursor;
    static Image Wallpaper;

    /*
     * Minimum system requirement:
     * 128MiB of RAM
     * Memory Map:
     * 64 MiB - 96MiB   -> System
     * 96 MiB - ∞     -> Free to use
     */
    [RuntimeExport("Main")]
    static void Main(MultibootInfo* Info, IntPtr Modules)
    {
        Allocator.Initialize((IntPtr)0x6000000);

        StartupCodeHelpers.InitializeModules(Modules);

        PageTable.Initialise();
        VBE.Initialise((VBEInfo*)Info->VBEInfo);
        Console.Setup();
        IDT.Disable();
        GDT.Initialise();
        IDT.Initialise();
        IDT.Enable();

        ACPI.Initialize();
        PIC.Disable();
        LocalAPIC.Initialize();
        IOAPIC.Initialize();
        HPET.Initialize();

        PS2Keyboard.Initialize();
        //Enable keyboard interrupts
        IOAPIC.SetEntry(0x21);

        Serial.Initialise();
        PIT.Initialise();
        PS2Mouse.Initialise();
        SMBIOS.Initialise();

        PCI.Initialise();
        SATA.Initialize();

        IDE.Initialize();

        Allocator.AllowCollect = true;

        Console.Write("Initrd: 0x");
        Console.WriteLine((Info->Mods[0]).ToString("x2"));
        Console.WriteLine("Initializing Ramdisk");
        FAT32 fat = new FAT32(new Ramdisk((IntPtr)Info->Mods[0]), 2048);

        /*
        if(SATA.Ports.Count)
        {
            FAT32 fat = new FAT32(SATA.Ports[0], 2048);
            //byte[] data = File.Instance.ReadAllBytes("TEST1.TXT");
        }
        */

        /*
        Console.WriteLine(
            IDE.Controllers[0].ReadOrWrite(IDE.Drive.Master, 0, (byte*)0x2000_0000, false) ? "IDE success" : "IDE failed");
        */

        /*
        ushort* _P = (ushort*)0x40000000;
        Console.WriteLine(SATA.Ports[0].Read(0, 1, _P) ? "SATA Success" : "SATA Failed");
        */

        /*
        FAT32 fat = new FAT32(SATA.Ports[0], 2048);
        for(int i = 0; i < fat.Items.Count; i++) 
        {
            Console.WriteLine($"{fat.Items[i].Parent}{fat.Items[i].Name}");
        }
        Bitmap bitmap = new Bitmap(fat.ReadAllBytes("/CURSOR.BMP"));
        Framebuffer.DrawImage(0, 0, bitmap);
        */

        /*
        FAT32 fat = new FAT32(SATA.Ports[0], 2048);
        AC97.Initialize();
        AC97.Play(fat.ReadAllBytes("/TEST.PCM"));
        */

        /*
        byte[] buffer = File.Instance.ReadAllBytes("/CURS.PNG");
        Console.WriteLine("File read");
        PNG png = new PNG(buffer);
        Framebuffer.DrawImage(0, 0, png);
        */

        Cursor = new PNG(File.Instance.ReadAllBytes("/CURSOR.PNG"));
        //Image from unsplash
        Wallpaper = new PNG(File.Instance.ReadAllBytes("/WALLP.PNG"));

        Serial.WriteLine("Hello World");
        Console.WriteLine("Hello, World!");
        Console.WriteLine("Use Native AOT (Core RT) Technology.");

        test();

        for(; ; ) 
        {
            Console.ReadKey(true);
            Allocator.Collect();
        }

        /*
        byte[] buffer = File.Instance.ReadAllBytes("TEST.PCM");
        AC97.Initialize();
        AC97.Play(buffer);
        */

        /*
        for(; ; ) 
        {
            Console.WriteLine(Console.ReadLine());
        }
        */

        /*
        ARP.Initialise();
        Network.Initialise(IPAddress.Parse(192, 168, 137, 188), IPAddress.Parse(192, 168, 137, 1));
        RTL8139.Initialise();
        ARP.Require(Network.Gateway);

        for (; ; );
        */

        Framebuffer.TripleBuffered = true;

        Form.Initialize();

        new FConsole(100, 100);

        for (; ; )
        {
            DoGUI();
        }
    }

    public static void DoGUI()
    {
        Framebuffer.DrawImage(0, 0, Wallpaper, false);
        Form.UpdateAll();
        /*
        ASC16.DrawString("FPS: ", 10, 10, 0xFFFFFFFF);
        ASC16.DrawString(((ulong)FPSMeter.FPS).ToString(), 42, 10, 0xFFFFFFFF);
        */
        Framebuffer.DrawImage(Control.MousePosition.X, Control.MousePosition.Y, Cursor);
        Framebuffer.Update();
        FPSMeter.Update();
    }
}
