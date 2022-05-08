/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */

//#define NETWORK

using Internal.Runtime.CompilerHelpers;
using Kernel;
using Kernel.Driver;
using Kernel.FS;
using Kernel.Graph;
using Kernel.GUI;
using Kernel.Misc;
using Kernel.NET;
using Kernel.GUI;
using System;
using System.Collections.Generic;
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
    static Image CursorMoving;
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

        ASC16.Initialise();
        VBE.Initialise((VBEInfo*)Info->VBEInfo);
        Console.Setup();
        IDT.Disable();
        GDT.Initialise();
        IDT.Initialize();
        IDT.Enable();

        SSE.enable_sse();
        AVX.init_avx();

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

#if HasGC
        GC.AllowCollect = true;
#endif

        //Only fixed size vhds are supported!
        Console.Write("Initrd: 0x");
        Console.WriteLine((Info->Mods[0]).ToString("x2"));
        Console.WriteLine("Initializing Ramdisk");
        new Ramdisk((IntPtr)(Info->Mods[0]));
        new FATFS();

        //Sized width to 512
        //https://gitlab.com/Enthymeme/hackneyed-x11-cursors/-/blob/master/theme/right-handed-white.svg
        Cursor = new PNG(File.Instance.ReadAllBytes("Images/Cursor.png"));
        CursorMoving = new PNG(File.Instance.ReadAllBytes("Images/Grab.png"));
        //Image from unsplash
        Wallpaper = new PNG(File.Instance.ReadAllBytes("Images/Wallpaper1.png"));
        //Wallpaper = new PNG(File.Instance.ReadAllBytes("Wallpaper.png"));

        BitFont.Initialize();

        string CustomCharset = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
        BitFont.RegisterBitFont(new BitFontDescriptor("Song", CustomCharset, File.Instance.ReadAllBytes("Song.btf"), 16));

        FConsole = null;
        Window.Initialize();

        Desktop.Initialize();

        Serial.WriteLine("Hello World");
        Console.WriteLine("Hello, World!");
        Console.WriteLine("Use Native AOT (Core RT) Technology.");

        //test();

        /*
        byte[] buffer = File.Instance.ReadAllBytes("TEST.PCM");
        AC97.Initialize();
        AC97.Play(buffer);
        */

        /*
        for (; ; )
        {
            Console.WriteLine(Console.ReadLine());
        }
        */

#if NETWORK
        //To use network. edit Kernel.csproj and use qemu. add "-net nic,model=rtl8139 -net tap,ifname=tap" to the end of command
        //Install openVPN's windows tap driver
        //rename the network adapter to tap in control panel
        //select your currently connected network controller and share the network with tap
        //Run
        Network.Initialise(IPAddress.Parse(192, 168, 137, 188), IPAddress.Parse(192, 168, 137, 1), IPAddress.Parse(255, 255, 255, 0));
        RTL8139.Initialise();
        ARP.Require(Network.Gateway);
        //Make sure you have already setup a web server on your PC
        TcpClient client = TcpClient.Connect(IPAddress.Parse(192,168,137,1), 80);
        client.OnData += Client_OnData;
        client.Send(ToASCII("GET / HTTP/1.1\r\nHost: 192.168.137.1\r\nUser-Agent: Mozilla/4.0 (compatible; MOOS Operating System)\r\n\r\n"));
        for (; ; ) Native.Hlt();
#endif

        ThreadPool.Initialize();

        KMain();
    }

#if NETWORK
    private static void Client_OnData(byte[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            Console.Write((char)data[i]);
        }
        Console.WriteLine();
    }

    public static byte[] ToASCII(string s) 
    {
        byte[] buffer = new byte[s.Length];
        for (int i = 0; i < buffer.Length; i++) buffer[i] = (byte)s[i];
        return buffer;
    }
#endif

    public static FConsole FConsole;

    public static void KMain()
    {
        Console.WriteLine("Press any key to enter desktop...");

        Framebuffer.TripleBuffered = true;

        if(PCI.GetDevice(0x15AD, 0x0405) != null)
            Framebuffer.Graphics = new VMWareSVGAIIGraphics();

        FConsole = new FConsole(350, 300);
        new Clock(650, 500);
        new Welcome(200, 200);

        new Calculator(300, 500);

        RightMenu rightmenu = new RightMenu();
        bool rightClicked = false;

        Console.WriteLine("Welcome to Moos!");
        Console.WriteLine("Thanks to all the Contributors of nifanfa/Moos.");

#region Animation of entering Desktop
        Framebuffer.Graphics.DrawImage((Framebuffer.Width / 2) - (Wallpaper.Width / 2), (Framebuffer.Height / 2) - (Wallpaper.Height / 2), Wallpaper, false);
        Desktop.Update();
        Window.UpdateAll();
        Framebuffer.Graphics.DrawImage(Control.MousePosition.X, Control.MousePosition.Y, Cursor);
        Image _screen = Framebuffer.Graphics.Save();
        Framebuffer.Graphics.Clear(0x0);

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
            Framebuffer.Graphics.Clear(0x0);
            Framebuffer.Graphics.ADrawImage(
                (Framebuffer.Graphics.Width / 2) - (img.Width / 2),
                (Framebuffer.Graphics.Height / 2) - (img.Height / 2),
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
#region ConsoleHotKey
            if (
                PS2Keyboard.KeyInfo.Key == ConsoleKey.T &&
                PS2Keyboard.KeyInfo.Modifiers.HasFlag(ConsoleModifiers.Ctrl) &&
                PS2Keyboard.KeyInfo.Modifiers.HasFlag(ConsoleModifiers.Alt)
                ) 
            {
                Window.MoveToEnd(FConsole);
                if (FConsole.Visible == false)
                    FConsole.Visible = true;
            }
            #endregion
            #region Right Menu
            if (Control.MouseButtons.HasFlag(MouseButtons.Right)) 
            {
                rightClicked = true;
            }
            else
            {
                if (rightClicked == true) rightmenu.Visible = !rightmenu.Visible;
                rightClicked = false;
            }
            #endregion

            Framebuffer.Graphics.DrawImage((Framebuffer.Width / 2) - (Wallpaper.Width / 2), (Framebuffer.Height / 2) - (Wallpaper.Height / 2), Wallpaper, false);
            Desktop.Update();
            Window.UpdateAll();
            /*
            ASC16.DrawString("FPS: ", 10, 10, 0xFFFFFFFF);
            ASC16.DrawString(((ulong)FPSMeter.FPS).ToString(), 42, 10, 0xFFFFFFFF);
            */
            Framebuffer.Graphics.DrawImage(Control.MousePosition.X, Control.MousePosition.Y, Window.HasWindowMoving ? CursorMoving : Cursor);
            Framebuffer.Update();

            FPSMeter.Update();
        }
    }
}
