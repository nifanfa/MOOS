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

    public static Image[] SizedScreens;

    /*
     * Minimum system requirement:
     * 1024MiB of RAM
     * Memory Map:
     * 256 MiB - 512MiB   -> System
     * 512 MiB - ∞     -> Free to use
     */
    [RuntimeExport("Main")]
    static void Main(MultibootInfo* Info, IntPtr Modules)
    {
        Allocator.Initialize((IntPtr)0x20000000);

        StartupCodeHelpers.InitializeModules(Modules);

        PageTable.Initialise();
        VBE.Initialise((VBEInfo*)Info->VBEInfo);
        Console.Setup();
        IDT.Disable();
        GDT.Initialise();
        IDT.Initialize();
        IDT.Enable();

        ACPI.Initialize();
        PIC.Enable();
        //LocalAPIC.Initialize();
        //IOAPIC.Initialize();
        //HPET.Initialize();

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

        //TO-DO disposing
        Console.WriteLine("Loading EXE...");
        byte[] buffer = File.Instance.ReadAllBytes("0:/ConsoleApp1.exe");
        Process.Start(buffer);

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
        Network.Initialise(IPAddress.Parse(192, 168, 137, 188), IPAddress.Parse(192, 168, 137, 1), IPAddress.Parse(255, 255, 255, 0));
        RTL8139.Initialise();
        ARP.Require(Network.Gateway);
        TcpClient client = TcpClient.Connect(IPAddress.Parse(192, 168, 137, 1), 54188);
        client.Send(new byte[]
        {
            (byte)'H',
            (byte)'e',
            (byte)'l',
            (byte)'l',
            (byte)'o'
        });
        for (; ; )
        {
            byte[] data = client.Receive();
            for (int i = 0; i < data.Length; i++) 
            {
                Console.Write((char)data[i]);
            }
            Console.WriteLine();
        }
        */

        ThreadPool.Initialize(&KMain);
        //KMain();
    }

    public static void KMain()
    {
        Console.WriteLine("Press any key to enter desktop...");
        Console.ReadKey();

        Framebuffer.TripleBuffered = true;

        new FConsole(350, 300);
        new Clock(650, 500);
        new Welcome(200, 200);
        Console.WriteLine("Welcome to Moos!");
        Console.WriteLine("Thanks to all the Contributors of nifanfa/Moos.");

        #region Animation of entering Desktop
        Framebuffer.DrawImage(0, 0, Wallpaper, false);
        Desktop.Update();
        Window.UpdateAll();
        Framebuffer.DrawImage(Control.MousePosition.X, Control.MousePosition.Y, Cursor);
        Image _screen = Framebuffer.Save();
        Framebuffer.Clear(0x0);

        SizedScreens = new Image[60];
        int startat = 40;
        for (int i = startat; i < SizedScreens.Length; i++)
        {
            SizedScreens[i] = _screen.ResizeImage(
                (int)(_screen.Width * (i / ((float)SizedScreens.Length))),
                (int)(_screen.Height * (i / ((float)SizedScreens.Length)))
                );
        }

        ulong lasttick = Timer.Ticks;
        for (int i = startat + 1; i < SizedScreens.Length; i++)
        {
            Image img = SizedScreens[i];
            Framebuffer.Clear(0x0);
            Framebuffer.ADrawImage(
                (Framebuffer.Width / 2) - (img.Width / 2),
                (Framebuffer.Height / 2) - (img.Height / 2),
                img, (byte)(255 * (i / (float)(SizedScreens.Length - startat))));
            Framebuffer.Update();
            while (lasttick + 10 > Timer.Ticks) Native.Hlt();
            lasttick = Timer.Ticks;
        }

        _screen.Dispose();
        for (int i = 0; i < SizedScreens.Length; i++) SizedScreens[i]?.Dispose();
        SizedScreens.Dispose();
        #endregion

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
