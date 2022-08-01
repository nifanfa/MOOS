//#define USBDebug
//#define NETWORK

using MOOS;
using MOOS.Driver;
using MOOS.FS;
using MOOS.GUI;
using MOOS.Misc;
using System;
using System.Desktops;
using System.Drawing;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;

unsafe class Program
{
    static void Main() { }

    [DllImport("*")]
    public static extern void test();


    static bool USBMouseTest()
    {
        HID.GetMouseThings(HID.Mouse, out sbyte AxisX, out sbyte AxisY, out var Buttons);
        return Buttons != MouseButtons.None;
    }

    static bool USBKeyboardTest()
    {
        HID.GetKeyboardThings(HID.Keyboard, out var ScanCode, out var Key);
        return ScanCode != 0;
    }

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
        Animator.Initialize();

#if USBDebug
        Hub.Initialize();
        HID.Initialize();
        EHCI.Initialize();
        //USB.StartPolling();

        //Use qemu for USB debug
        //VMware won't connect virtual USB HIDs
        if (HID.Mouse == null)
        {
            Console.WriteLine("USB Mouse not present");
        }
        if (HID.Keyboard == null)
        {
            Console.WriteLine("USB Keyboard not present");
        }

        for(; ; )
        {
            if (HID.Mouse != null)
            {
                HID.GetMouseThings(HID.Mouse, out sbyte AxisX, out sbyte AxisY, out var Buttons);
                if (AxisX != 0 && AxisY != 0)
                {
                    Console.WriteLine($"X:{AxisX} Y:{AxisY}");
                }
            }
            if(HID.Keyboard != null) 
            {
                HID.GetKeyboardThings(HID.Keyboard, out var ScanCode, out var Key);
                if(ScanCode != 0)
                {
                    Console.WriteLine($"ScanCode:{ScanCode}");
                }
            }
        }
#else
        Hub.Initialize();
        HID.Initialize();
        EHCI.Initialize();

        if (HID.Mouse != null)
        {
            Console.Write("[Warning] Press please press Mouse any key to validate USB Mouse ");
            bool res = Console.Wait(&USBMouseTest, 2000);
            Console.WriteLine();
            if (!res)
            {
                lock (null)
                {
                    USB.NumDevice--;
                    HID.Mouse = null;
                }
            }
        }

        if (HID.Keyboard != null)
        {
            Console.Write("[Warning] Press please press any key to validate USB keyboard ");
            bool res = Console.Wait(&USBKeyboardTest, 2000);
            Console.WriteLine();
            if (!res)
            {
                lock (null)
                {
                    USB.NumDevice--;
                    HID.Keyboard = null;
                }
            }
        }

        USB.StartPolling();

        //Use qemu for USB debug
        //VMware won't connect virtual USB HIDs
        if (HID.Mouse == null)
        {
            Console.WriteLine("USB Mouse not present");
        }
        if (HID.Keyboard == null)
        {
            Console.WriteLine("USB Keyboard not present");
        }
#endif

       
        FConsole = null;
        DesktopManager.Initialize();

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
        //Install openVPN's windows tap driver
        //You can download it here: http://swupdate.openvpn.org/community/releases/openvpn-2.2.2-install.exe
        //rename the network adapter to tap in control panel
        //right click your network connection device. then share the network with tap 
        //Select "QEMU with network" from dropdown menu
        //add "#define NETWORK" to the start of this file
        //Run
        Network.Initialise(IPAddress.Parse(192, 168, 137, 188), IPAddress.Parse(192, 168, 137, 1), IPAddress.Parse(255, 255, 255, 0));
        //Make sure this IP is pointing your gateway
        TcpClient client = TcpClient.Connect(IPAddress.Parse(192,168, 137, 1), 80);
        client.OnData += Client_OnData;
        client.Send(ToASCII("GET / HTTP/1.1\r\nHost: 192.168.137.1\r\nUser-Agent: Mozilla/4.0 (compatible; MOOS Operating System)\r\n\r\n"));
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
    //public static RightMenu rightmenu;

    public static void SMain()
    {
        Console.WriteLine("Press any key to enter desktop...");

        Framebuffer.TripleBuffered = true;

        /*
        //This driver doesn't support drawing without update
        if(PCI.GetDevice(0x15AD, 0x0405) != null)
            Framebuffer.Graphics = new VMWareSVGAIIGraphics();
        */

        Lockscreen.Initialize();

        FConsole = new FConsole(350, 300);
        FConsole.Visible = false;

        //rightmenu = new RightMenu();
        rightClicked = false;

        #region Animation of entering Desktop
        Framebuffer.Graphics.DrawImage((Framebuffer.Width / 2) - (DesktopManager.Wallpaper.Width / 2), (Framebuffer.Height / 2) - (DesktopManager.Wallpaper.Height / 2), DesktopManager.Wallpaper, false);
        DesktopManager.Update();
        DesktopManager.Draw();
        WindowManager.DrawAll();
        Framebuffer.Graphics.DrawImage(Control.MousePosition.X, Control.MousePosition.Y, DesktopManager.Cursor);
        Image _screen = Framebuffer.Graphics.Save();
        Framebuffer.Graphics.Clear(0x0);

        var SizedScreens = new Image[60];
        int startat = 40;
        for (int i = startat; i < SizedScreens.Length; i++)
        {
            SizedScreens[i] = _screen.ResizeImage(
                (int)(_screen.Width * (i / ((float)SizedScreens.Length))),
                (int)(_screen.Height * (i / ((float)SizedScreens.Length)))
                );
        }

        Animation ani = new Animation()
        {
            Value = startat + 1,
            MinimumValue = startat + 1,
            MaximumValue = SizedScreens.Length - 1,
            ValueChangesInPeriod = 1,
            PeriodInMS = 16
        };
        Animator.AddAnimation(ani);
        while (ani.Value < SizedScreens.Length - 1)
        {
            int i = ani.Value;
            Image img = SizedScreens[i];
            Framebuffer.Graphics.Clear(0x0);
            Framebuffer.Graphics.ADrawImage(
                (Framebuffer.Graphics.Width / 2) - (img.Width / 2),
                (Framebuffer.Graphics.Height / 2) - (img.Height / 2),
                img, (byte)(255 * (i / (float)(SizedScreens.Length - startat))));
            Framebuffer.Update();
        }
        Animator.DisposeAnimation(ani);

        _screen.Dispose();
        for (int i = 0; i < SizedScreens.Length; i++) SizedScreens[i]?.Dispose();
        SizedScreens.Dispose();
        #endregion

        NotificationManager.Initialize();

        for (; ; )
        {
            /*
            #region ConsoleHotKey
            if (
                Keyboard.KeyInfo.Key == ConsoleKey.T &&
                Keyboard.KeyInfo.Modifiers.HasFlag(ConsoleModifiers.Control) &&
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
                if (rightClicked == true)
                {
                    rightmenu.Visible = !rightmenu.Visible;
                    WindowManager.MoveToEnd(rightmenu);
                }

                rightClicked = false;
            }
            #endregion
            */
            WindowManager.InputAll();

            //UIKernel
            DesktopManager.Update();
            DesktopManager.Draw();
            WindowManager.UpdateAll();
            WindowManager.DrawAll();
            CursorManager.Update();

            Framebuffer.Graphics.DrawImage(Control.MousePosition.X, Control.MousePosition.Y, CursorManager.GetCursor);

            Framebuffer.Update();

            FPSMeter.Update();
        }
    }
}
