﻿using Internal.Runtime.CompilerHelpers;
using Kernel;
using Kernel.Driver;
using Kernel.FS;
using Kernel.GUI;
using Kernel.Misc;
using System;
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
        IDT.Initialize();
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
        Wallpaper = new PNG(File.Instance.ReadAllBytes("/WALP.PNG"));

        BitFont.Initialize();

        string CustomCharset = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
        BitFont.RegisterBitFont(new BitFontDescriptor("Song", CustomCharset, File.Instance.ReadAllBytes("SONG.BTF"), 16));

        Form.Initialize();

        Desktop.Initialize();

        Serial.WriteLine("Hello World");
        Console.WriteLine("Hello, World!");
        Console.WriteLine("Use Native AOT (Core RT) Technology.");

        test();

        Console.WriteLine("Press Ctrl + N To Launch Nintendo Family Computer Emulator Otherwise Enter GUI");

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

        ThreadPool.Initialize();
        //KMain();
    }

    public static void KMain()
    {
        ConsoleKeyInfo Key = Console.ReadKey();

        //if (false)
        if (Key.Key == ConsoleKey.N && Key.Modifiers.HasFlag(ConsoleModifiers.Ctrl))

        {

            Console.WriteLine("Emulator Keymap:");
            Console.WriteLine("A = Q");
            Console.WriteLine("B = E");
            Console.WriteLine("Z = Select");
            Console.WriteLine("C = Start");
            Console.WriteLine("W = Up");
            Console.WriteLine("A = Left");
            Console.WriteLine("S = Down");
            Console.WriteLine("D = Right");
            Console.WriteLine("Game Will Start After 2 Seconds");
            PIT.Wait(2000);
            NES.NES nes = new NES.NES();
            nes.openROM(File.Instance.ReadAllBytes("/MARIO.NES"));
            Console.WriteLine("Nintendo Family Computer Emulator Initialized");
            Framebuffer.TripleBuffered = true;
            for (; ; )
            {
                nes.runGame();
                for (int i = 0; i < 128; i++) Native.Nop();
            }
        }


        /*
        ARP.Initialise();
        Network.Initialise(IPAddress.Parse(192, 168, 137, 188), IPAddress.Parse(192, 168, 137, 1));
        RTL8139.Initialise();
        ARP.Require(Network.Gateway);
        for (; ; );
        */

        else

        {
            Framebuffer.TripleBuffered = true;

            new FConsole(350, 300);
            Console.WriteLine("Welcome to OS-Sharp!");
            Console.WriteLine("Thanks to all the Contributors of nifanfa/OS-Sharp.");

            for (; ; )
            {
                Framebuffer.DrawImage(0, 0, Wallpaper, false);
                Desktop.Update();
                Form.UpdateAll();
                /*
                ASC16.DrawString("FPS: ", 10, 10, 0xFFFFFFFF);
                ASC16.DrawString(((ulong)FPSMeter.FPS).ToString(), 42, 10, 0xFFFFFFFF);
                */
                Framebuffer.DrawImage(Control.MousePosition.X, Control.MousePosition.Y, Cursor);
                Framebuffer.Update();
            }
        }
    }
}
