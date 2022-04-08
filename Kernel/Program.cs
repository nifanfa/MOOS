/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
using Internal.Runtime.CompilerHelpers;
using Kernel;
using Kernel.Driver;
using Kernel.FS;
using Kernel.GUI;
using Kernel.Misc;
using Kernel.NET;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
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

        IDE.Initialize();
        SATA.Initialize();

#if HasGC
        GC.AllowCollect = true;
#endif

        //Only fixed size vhds are supported!
        Console.Write("Initrd: 0x");
        Console.WriteLine((Info->Mods[0]).ToString("x2"));
        Console.WriteLine("Initializing Ramdisk");
        new Ramdisk((IntPtr)(Info->Mods[0]));
        new FATFS();

        Cursor = new PNG(File.Instance.ReadAllBytes("0:/CURSOR.PNG"));
        //Image from unsplash
        Wallpaper = new PNG(File.Instance.ReadAllBytes("0:/WALP.PNG"));

        BitFont.Initialize();

        string CustomCharset = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
        BitFont.RegisterBitFont(new BitFontDescriptor("Song", CustomCharset, File.Instance.ReadAllBytes("0:/SONG.BTF"), 16));

        Window.Initialize();

        Desktop.Initialize();

        Serial.WriteLine("Hello World");
        Console.WriteLine("Hello, World!");
        Console.WriteLine("Use Native AOT (Core RT) Technology.");

        //test();

        //FIXME - to support unspecific addressed exe
        //2.EXE is a unspecific addressed exe for testing
        Console.WriteLine("Loading EXE...");
        byte[] buffer = File.Instance.ReadAllBytes("0:/ConsoleApp1.exe");
        Process.Start(buffer);

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

        /*
        ARP.Initialise();
        Network.Initialise(IPAddress.Parse(192, 168, 137, 188), IPAddress.Parse(192, 168, 137, 1), IPAddress.Parse(255, 255, 255, 0));
        RTL8139.Initialise();
        ARP.Require(Network.Gateway);
        TCPConnection conn = TCP.Connect(new byte[] { 192, 168, 137, 1 }, 54188, 54188);
        conn.Send(new byte[]
        {
            (byte)'H',
            (byte)'e',
            (byte)'l',
            (byte)'l',
            (byte)'o'
        });
        for (; ; ) Native.Hlt();
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
            Timer.Wait(2000);
            NES.NES nes = new NES.NES();
            nes.openROM(File.Instance.ReadAllBytes("0:/MARIO.NES"));
            Console.WriteLine("Nintendo Family Computer Emulator Initialized");
            Framebuffer.TripleBuffered = true;
            for (; ; )
            {
                nes.runGame();
                for (int i = 0; i < 128; i++) Native.Nop();
            }
        }
        else

        {
            Framebuffer.TripleBuffered = true;

            new FConsole(350, 300);
            new Clock(650, 500);
            new Welcome(200, 200);
            Console.WriteLine("Welcome to Moos!");
            Console.WriteLine("Thanks to all the Contributors of nifanfa/Moos.");

            for (; ; )
            {
                Framebuffer.DrawImage(0, 0, Wallpaper, false);
                Desktop.Update();
                Window.UpdateAll();
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
