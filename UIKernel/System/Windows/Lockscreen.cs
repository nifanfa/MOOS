#if HasGUI && Kernel
using System;
using System.Desktops;
using System.Drawing;
using System.Windows;
using MOOS.FS;
using MOOS.Misc;

namespace MOOS.GUI
{
	public static class Lockscreen
	{
		private static IFont lsfont;
		private static string[] mon;

		public static void Initialize()
		{
			mon = new string[12]
			{
				"January",
				"February",
				"March",
				"April",
				"May",
				"June",
				"July",
				"August",
				"September",
				"October",
				"November",
				"December"
			};

			lsfont = new IFont(new PNG(File.ReadAllBytes("sys/media/Yahei128.png")), "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~", 128);

			DrawLockscreenUI();
			Image i = Framebuffer.Graphics.Save();

			Animation a0 = new()
			{
				MinimumValue = 0,
				MaximumValue = 255,
				ValueChangesInPeriod = 1
			};
			Animator.AddAnimation(a0);
			while (a0.Value < a0.MaximumValue)
			{
				Framebuffer.Graphics.Clear(Color.Black);
				Framebuffer.Graphics.DrawImage(i, (Framebuffer.Width / 2) - (DesktopManager.Wallpaper.Width / 2), (Framebuffer.Height / 2) - (DesktopManager.Wallpaper.Height / 2), (byte)a0.Value);
				Framebuffer.Update();
			}
			a0.Dispose();

			while (Keyboard.KeyInfo.Key != System.ConsoleKey.Up && Keyboard.KeyInfo.Key != System.ConsoleKey.W)
			{
				DrawLockscreenUI();

				Framebuffer.Update();
			}

			DrawLockscreenUI();
			i.Dispose();
			i = Framebuffer.Graphics.Save();

			Animation a1 = new()
			{
				MinimumValue = 0,
				MaximumValue = DesktopManager.Wallpaper.Height,
				ValueChangesInPeriod = 1
			};
			Animator.AddAnimation(a1);
			while (a1.Value < DesktopManager.Wallpaper.Height)
			{
				Framebuffer.Graphics.Clear(Color.Black);
				Framebuffer.Graphics.DrawImage(i, (Framebuffer.Width / 2) - (DesktopManager.Wallpaper.Width / 2), (Framebuffer.Height / 2) - (DesktopManager.Wallpaper.Height / 2) - a1.Value, false);
				Framebuffer.Update();
			}
			a1.Dispose();

			Framebuffer.Graphics.Clear(Color.Black);
			Framebuffer.Update();

			i.Dispose();
		}

		private static void DrawLockscreenUI()
		{
			Framebuffer.Graphics.DrawImage(DesktopManager.Wallpaper, (Framebuffer.Width / 2) - (DesktopManager.Wallpaper.Width / 2), (Framebuffer.Height / 2) - (DesktopManager.Wallpaper.Height / 2), false);

			/* Standard Time */
			// string s = (RTC.Minute < 10 ? $"{RTC.Hour - (RTC.Hour > 12 ? 12 : 0)}:0{RTC.Minute}" : $"{RTC.Hour - (RTC.Hour > 12 ? 12 : 0)}:{RTC.Minute}") + (RTC.Hour > 12 ? "PM" : "AM");
			/* Military Time */
			string s = RTC.Minute < 10 ? $"{RTC.Hour}:0{RTC.Minute}" : $"{RTC.Hour}:{RTC.Minute}";
			lsfont.DrawString((Framebuffer.Width / 2) - (lsfont.MeasureString(s) / 2), Framebuffer.Height / 6, s, 0xFFFFFFFF);
			s.Dispose();

			string _1 = mon[Math.Clamp(RTC.Month - 1, 0, mon.Length - 1)];
			string _2 = ", ";
			string _3 = RTC.Day.ToString();
			string _4 = "th";
			string res = _1 + _2 + _3 + _4;
			WindowManager.font.DrawString((Framebuffer.Width / 2) - (WindowManager.font.MeasureString(res) / 2), (Framebuffer.Height / 6) + lsfont.FontSize, res, 0xFFFFFFFF);

			//_1.Dispose();
			_2.Dispose();
			_3.Dispose();
			_4.Dispose();
			res.Dispose();


			//string tips = "Press W or Up Arrow to unlock";
			//WindowManager.font.DrawString((Framebuffer.Width / 2) - (WindowManager.font.MeasureString(tips) / 2), Framebuffer.Height - (Framebuffer.Height / 6), tips);
			//tips.Dispose();

		}
	}
}
#endif