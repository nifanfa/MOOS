#if HasGUI
using System;
using System.Drawing;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Windows;
using MOOS.Driver;
using MOOS.FS;
using MOOS.Graph;
using MOOS.Misc;

namespace MOOS.GUI
{
	internal unsafe class Doom : Window
	{
		#region Doom
		[DllImport("*")]
		public static extern int doommain(byte* gb, long gl);

		[RuntimeExport("GetTickCount")]
		public static uint GetTickCount()
		{
			return (uint)Timer.Ticks;
		}

		[RuntimeExport("Sleep")]
		public static void Sleep(uint ms)
		{
			Thread.Sleep(ms);
		}

		[RuntimeExport("DrawPoint")]
		public static void DrawPoint(int x, int y, uint color)
		{
			dg.DrawPoint(Color.FromArgb(color), x, y);
		}

		[DllImport("*")]
		public static extern void addKeyToQueue(int pressed, byte keyCode);

		public static byte[] gb;

		public static Image di;
		public static Graphics dg;
		#endregion

		Thread _thread;
		public Doom(int X, int Y, string file) : base(X, Y, 640, 400)
		{
#if Chinese
			Title = "毁灭战士";
#else
			Title = "DOOM Shareware";
#endif

			di = new Image(640, 400);
			dg = Graphics.FromImage(di);

			gb = File.ReadAllBytes(file);

			Keyboard.OnKeyChanged += PS2Keyboard_OnKeyChanged;

#if Chinese
			MessageBox.Show("键位: WASD Ctrl Shift ESC Enter", "Information");
#else
			MessageBox.Show("Keymap: WASD Ctrl Shift ESC Enter", "Information");
#endif

			_thread = new Thread(new Action(() =>
			{
				fixed (byte* ptr = gb)
				{
					doommain(ptr, gb.Length);
				}
			}));

			_thread.Start(1);
		}

		private void PS2Keyboard_OnKeyChanged(ConsoleKeyInfo key)
		{
			if (!IsFocus) return;
			addKeyToQueue(key.KeyState != ConsoleKeyState.Released ? 1 : 0, (byte)key.Key);
		}

		public override void OnDraw()
		{
			base.OnDraw();
			Framebuffer.Graphics.DrawImage(di, X, Y, false);
		}

        public override void OnClose()
        {
            base.OnClose();
			_thread.Terminated = true;
			_thread.Dispose();
		}
    }
}
#endif