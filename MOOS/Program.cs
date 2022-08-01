//#define USBDebug
//#define NETWORK

using System;
using System.Drawing;
using System.Runtime;
using System.Windows.Forms;
using MOOS;
using MOOS.Driver;
using MOOS.FS;
using MOOS.GUI;
using MOOS.Misc;
#if NETWORK
using MOOS.NET;
using System.Net;
#endif

internal unsafe class Program
{
	/// <summary>
	/// Compiler requires a static main method in class 'Program'
	/// 
	/// <![CDATA[
	/// Do not remove!
	/// ]]>
	/// </summary>
	private static void Main() { }

	private static Image Cursor;
	private static Image CursorMoving;
	public static Image Wallpaper;

	private static bool USBMouseTest()
	{
		HID.GetMouseThings(HID.Mouse, out sbyte AxisX, out sbyte AxisY, out MouseButtons Buttons);
		return Buttons != MouseButtons.None;
	}

	private static bool USBKeyboardTest()
	{
		HID.GetKeyboardThings(HID.Keyboard, out byte ScanCode, out ConsoleKey Key);
		return ScanCode != 0;
	}

	/**
	 * Minimum memory is 1024MiB of RAM
	 * Memory Map:
	 * 256 MiB - 512MiB    System
	 * 512 MiB - ∞         Free to use
	 * <see cref="EntryPoint"/>
	 */
	[RuntimeExport("KMain")]
	private
#pragma warning disable IDE0051 // Remove unused private members
	static void KMain()
#pragma warning restore IDE0051 // Remove unused private members
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

		for (; ; )
		{
			if (HID.Mouse != null)
			{
				HID.GetMouseThings(HID.Mouse, out sbyte AxisX, out sbyte AxisY, out MouseButtons Buttons);
				if (AxisX != 0 && AxisY != 0)
				{
					Console.WriteLine($"X:{AxisX} Y:{AxisY}");
				}
			}
			if (HID.Keyboard != null)
			{
				HID.GetKeyboardThings(HID.Keyboard, out byte ScanCode, out ConsoleKey Key);
				if (ScanCode != 0)
				{
					Console.WriteLine($"ScanCode:{ScanCode}");
				}
			}
		}
#else

		if (HID.Mouse != null)
		{
			Console.WriteInfo("Warning", "Press please press Mouse any key to validate USB Mouse ");
			bool res = Console.Wait(&USBMouseTest, 3500);
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
			Console.WriteInfo("Warning", "Press please press any key to validate USB keyboard ");
			bool res = Console.Wait(&USBKeyboardTest, 3500);
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
		//Sized width to 512
		//https://gitlab.com/Enthymeme/hackneyed-x11-cursors/-/blob/master/theme/right-handed-white.svg
		Cursor = new PNG(File.ReadAllBytes("Images/Cursor.png"));

		CursorMoving = new PNG(File.ReadAllBytes("Images/Grab.png"));
		//Image from unsplash
		Wallpaper = new PNG(File.ReadAllBytes("Images/Wallpaper1.png"));
		BitFont.Initialize();

		string CustomCharset = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
		BitFont.RegisterBitFont(new BitFontDescriptor("Song", CustomCharset, File.ReadAllBytes("Song.btf"), 16));

		FConsole = null;
		WindowManager.Initialize();

		Desktop.Initialize();

		Serial.WriteLine("Hello World");
		Console.WriteLine("Hello, World!");
		Console.WriteLine("Use Native AOT (Core RT) Technology.");

		Audio.Initialize();
		AC97.Initialize();

		#region Network
		/// <![CDATA[
		/// How to use network
		/// 
		/// 1. Install OpenVPN's Windows tap driver ( Availible here: http://swupdate.openvpn.org/community/releases/openvpn-2.2.2-install.exe )
		/// 2. Open "Control Panel\Network and Internet\Network Connections" in the Windows Control Panel.
		/// 3. Rename the newly created network adapter to "tap" in the Windows Control Panel.
		/// 4. Ctrl-Click your network adapter and the "tap" network adapter, then right-click on the "tap" network adapter and click "Bridge Connections"
		/// 5. The network has been successfully linked.
		/// To Un-Bridge the connections, Click on "Network Bridge" and then click at the top of the window "Delete this connection". ( Do this when you are not using the network as it slows down your internet connection )
		///
		/// <remember>
		/// BUILD WITH 'QEMU Network' not 'QEMU Normal' or 'QEMU USB' for Networking to work
		/// </remember>
		/// ]]>
#if NETWORK
		Network.Initialize(IPAddress.Parse(192, 168, 137, 188), IPAddress.Parse(192, 168, 137, 1), IPAddress.Parse(255, 255, 255, 0));
		//Make sure this IP is pointing your gateway
		TcpClient client = TcpClient.Connect(IPAddress.Parse(192, 168, 137, 1), 80);
		client.OnData += Client_OnData;
		client.Send(ToASCII("GET / HTTP/1.1\r\nHost: 192.168.137.1\r\nUser-Agent: Mozilla/4.0 (compatible; MOOS Operating System)\r\n\r\n"));
		for (; ; )
		{
			Native.Hlt();
		}
#endif
		#endregion

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
		for (int i = 0; i < buffer.Length; i++)
		{
			buffer[i] = (byte)s[i];
		}

		return buffer;
	}
#endif



	public static bool rightClicked;
	public static FConsole FConsole;
	public static RightMenu rightmenu;

	public static void SMain()
	{
		Framebuffer.DoubleBuffered = true;

		/*
		//This driver doesn't support drawing without update
		if(PCI.GetDevice(0x15AD, 0x0405) != null)
			Framebuffer.Graphics = new VMWareSVGAIIGraphics();
		*/

		Image wall = Wallpaper;
		Wallpaper = wall.ResizeImage(Framebuffer.Width, Framebuffer.Height);
		wall.Dispose();

		Lockscreen.Initialize();

		FConsole = new FConsole(350, 300)
		{
			Visible = false
		};

		_ = new Welcome(400, 250);

		rightmenu = new RightMenu();
		rightClicked = false;

		#region Animation of entering Desktop
		Framebuffer.Graphics.DrawImage(Wallpaper, (Framebuffer.Width / 2) - (Wallpaper.Width / 2), (Framebuffer.Height / 2) - (Wallpaper.Height / 2), false);
		Desktop.Update();
		WindowManager.DrawAll();
		Framebuffer.Graphics.DrawImage(Cursor, Control.MousePosition.X, Control.MousePosition.Y, true);
		Image _screen = Framebuffer.Graphics.Save();
		Framebuffer.Graphics.Clear(Color.FromArgb(0x0));

		Image[] SizedScreens = new Image[60];
		int startat = 40;
		for (int i = startat; i < SizedScreens.Length; i++)
		{
			SizedScreens[i] = _screen.ResizeImage(
				(int)(_screen.Width * (i / ((float)SizedScreens.Length))),
				(int)(_screen.Height * (i / ((float)SizedScreens.Length)))
				);
		}

		Animation ani = new()
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
			Framebuffer.Graphics.Clear(Color.FromArgb(0x0));
			Framebuffer.Graphics.DrawImage(img, (Framebuffer.Graphics.Width / 2) - (img.Width / 2), (Framebuffer.Graphics.Height / 2) - (img.Height / 2), (byte)(255 * (i / (float)(SizedScreens.Length - startat))));
			Framebuffer.Update();
		}
		ani.Dispose();

		_screen.Dispose();
		for (int i = 0; i < SizedScreens.Length; i++)
		{
			SizedScreens[i]?.Dispose();
		}

		SizedScreens.Dispose();
		#endregion

		NotificationManager.Initialize();
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
				{
					FConsole.Visible = true;
				}
			}
			#endregion
			#region Right Menu
			if (Control.MouseButtons.HasFlag(MouseButtons.Right))
			{
				rightClicked = true;
			} else
			{
				if (rightClicked == true)
				{
					rightmenu.Visible = !rightmenu.Visible;
					WindowManager.MoveToEnd(rightmenu);
				}

				rightClicked = false;
			}
			#endregion
			WindowManager.InputAll();

			Framebuffer.Graphics.DrawImage(Wallpaper, (Framebuffer.Width / 2) - (Wallpaper.Width / 2), (Framebuffer.Height / 2) - (Wallpaper.Height / 2), false);
			Desktop.Update();
			NotificationManager.Update();
			WindowManager.DrawAll();
			/*
			ASC16.DrawString("FPS: ", 10, 10, 0xFFFFFFFF);
			ASC16.DrawString(((ulong)FPSMeter.FPS).ToString(), 42, 10, 0xFFFFFFFF);
			*/
			Framebuffer.Graphics.DrawImage(WindowManager.HasWindowMoving ? CursorMoving : Cursor, Control.MousePosition.X, Control.MousePosition.Y, true);
			Framebuffer.Update();

			FPSMeter.Update();
		}
	}
}
