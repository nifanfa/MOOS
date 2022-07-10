//#define NETWORK

using Internal.Runtime.CompilerHelpers;
using MOOS;
using MOOS.Driver;
using MOOS.FS;
using MOOS.Graph;
using MOOS.GUI;
using MOOS.Misc;
using MOOS.NET;
using MOOS.GUI;
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
    //Check out Kernel/Misc/EntryPoint.cs
    [RuntimeExport("KMain")]
    static void KMain()
    {
        HID.Initialize();

        EHCI.Initialize();

        //Use qemu for USB debug
        //VMware won't virtual a USB HID
#if false
        if (HID.Keyboard != null && HID.Mouse != null) 
        {
            new Thread(() =>
            {
                for (; ; )
                {
                    USB.OnInterrupt();
                }
            }).Start();
        }
        else 
        {
            Panic.Error("Either USB Mouse or USB Keyboard not present");
        }
#endif

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
        WindowManager.Initialize();

        Desktop.Initialize();

        Serial.WriteLine("Hello World");
        Console.WriteLine("Hello, World!");
        Console.WriteLine("Use Native AOT (Core RT) Technology.");

        //test();

        Audio.Initialize();
        AC97.Initialize();

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
        //right click your network connection device. then share the network with tap 
        //Run
        Network.Initialise(IPAddress.Parse(192, 168, 137, 188), IPAddress.Parse(192, 168, 137, 1), IPAddress.Parse(255, 255, 255, 0));
        //Make sure this IP is pointing your gateway
        TcpClient client = TcpClient.Connect(IPAddress.Parse(192,168, 137, 1), 80);
        client.OnData += Client_OnData;
        client.Send(ToASCII("GET / HTTP/1.1\r\nHost: 192.168.1.1\r\nUser-Agent: Mozilla/4.0 (compatible; MOOS Operating System)\r\n\r\n"));
        for (; ; ) Native.Hlt();
#endif

        SMain();
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

    public static bool rightClicked;
    public static FConsole FConsole;
    public static RightMenu rightmenu;

    public static void SMain()
    {
        Console.WriteLine("Press any key to enter desktop...");

        Framebuffer.TripleBuffered = true;

        if(PCI.GetDevice(0x15AD, 0x0405) != null)
            Framebuffer.Graphics = new VMWareSVGAIIGraphics();

        Image wall = Wallpaper;
        Wallpaper = wall.ResizeImage(Framebuffer.Width, Framebuffer.Height);
        wall.Dispose();

        FConsole = new FConsole(350, 300);
        FConsole.Visible = false;

        var welcome = new Welcome(400, 250);

        rightmenu = new RightMenu();
        rightClicked = false;

        Console.WriteLine("Welcome to MOOS!");
        Console.WriteLine("Thanks to all the Contributors of nifanfa/MOOS.");

#region Animation of entering Desktop
        Framebuffer.Graphics.DrawImage((Framebuffer.Width / 2) - (Wallpaper.Width / 2), (Framebuffer.Height / 2) - (Wallpaper.Height / 2), Wallpaper, false);
        Desktop.Update();
        WindowManager.DrawAll();
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
                Keyboard.KeyInfo.Key == ConsoleKey.T &&
                Keyboard.KeyInfo.Modifiers.HasFlag(ConsoleModifiers.Ctrl) &&
                Keyboard.KeyInfo.Modifiers.HasFlag(ConsoleModifiers.Alt)
                )
            {
                WindowManager.MoveToEnd(FConsole);
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
            WindowManager.InputAll();

            Framebuffer.Graphics.DrawImage((Framebuffer.Width / 2) - (Wallpaper.Width / 2), (Framebuffer.Height / 2) - (Wallpaper.Height / 2), Wallpaper, false);
            Desktop.Update();
            WindowManager.DrawAll();
            /*
            ASC16.DrawString("FPS: ", 10, 10, 0xFFFFFFFF);
            ASC16.DrawString(((ulong)FPSMeter.FPS).ToString(), 42, 10, 0xFFFFFFFF);
            */
            Framebuffer.Graphics.DrawImage(Control.MousePosition.X, Control.MousePosition.Y, WindowManager.HasWindowMoving ? CursorMoving : Cursor);
            Framebuffer.Update();

            FPSMeter.Update();
        }
    }
}
