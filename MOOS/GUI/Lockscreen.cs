#if HasGUI && Kernel
using MOOS.Driver;
using MOOS.FS;
using MOOS.Misc;
using System;
using System.Drawing;

namespace MOOS.GUI
{
    public static class Lockscreen
    {
        static IFont lsfont;
        static string[] mon;

        public static void Run()
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
                "Augest",
                "September",
                "October",
                "November",
                "December"
            };

            lsfont = new IFont(new PNG(File.ReadAllBytes("Images/Yahei128.png")), "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~", 128);

            DrawLockscreenUI();
            Image i = Framebuffer.Graphics.Save();

            Animation a0 = new Animation()
            {
                MinimumValue = 0,
                MaximumValue = 255,
                ValueChangesInPeriod = 1
            };
            Animator.AddAnimation(a0);
            while (a0.Value < a0.MaximumValue)
            {
                Framebuffer.Graphics.Clear(0x0);
                Framebuffer.Graphics.ADrawImage((Framebuffer.Width / 2) - (Program.Wallpaper.Width / 2), (Framebuffer.Height / 2) - (Program.Wallpaper.Height / 2), i, (byte)a0.Value);
                Framebuffer.Update();
            }
            Animator.DisposeAnimation(a0);

            while (Keyboard.KeyInfo.Key != System.ConsoleKey.Up && Keyboard.KeyInfo.Key != System.ConsoleKey.W)
            {
                DrawLockscreenUI();

                Framebuffer.Update();
            }

            DrawLockscreenUI();
            i.Dispose();
            i = Framebuffer.Graphics.Save();

            Animation a1 = new Animation()
            {
                MinimumValue = 0,
                MaximumValue = Program.Wallpaper.Height,
                ValueChangesInPeriod = 1
            };
            Animator.AddAnimation(a1);
            while (a1.Value < Program.Wallpaper.Height)
            {
                Framebuffer.Graphics.Clear(0x0);
                Framebuffer.Graphics.DrawImage((Framebuffer.Width / 2) - (Program.Wallpaper.Width / 2), (Framebuffer.Height / 2) - (Program.Wallpaper.Height / 2) - a1.Value, i, false);
                Framebuffer.Update();
            }
            Animator.DisposeAnimation(a1);

            Framebuffer.Graphics.Clear(0x0);
            Framebuffer.Update();

            i.Dispose();
        }

        private static void DrawLockscreenUI()
        {
            Framebuffer.Graphics.DrawImage((Framebuffer.Width / 2) - (Program.Wallpaper.Width / 2), (Framebuffer.Height / 2) - (Program.Wallpaper.Height / 2), Program.Wallpaper, false);

            string s = null;
            if (RTC.Minute < 10)
            {
                s = $"{RTC.Hour}:0{RTC.Minute}";
            }
            else
            {
                s = $"{RTC.Hour}:{RTC.Minute}";
            }
            lsfont.DrawString((Framebuffer.Width / 2) - (lsfont.MeasureString(s) / 2), Framebuffer.Height / 6, s);
            s.Dispose();

            string _1 = mon[Math.Clamp(RTC.Month, 0, mon.Length - 1)];
            string _2 = ", ";
            string _3 = RTC.Day.ToString();
            string _4 = "th";
            string res = _1 + _2 + _3 + _4;
            WindowManager.font.DrawString((Framebuffer.Width / 2) - (WindowManager.font.MeasureString(res) / 2), (Framebuffer.Height / 6) + lsfont.FontSize, res);

            //_1.Dispose();
            _2.Dispose();
            _3.Dispose();
            _4.Dispose();
            res.Dispose();

            /*
            string tips = "Press W or Up Arrow to unlock";
            WindowManager.font.DrawString((Framebuffer.Width / 2) - (WindowManager.font.MeasureString(tips) / 2), Framebuffer.Height - (Framebuffer.Height / 6), tips);
            tips.Dispose();
            */
        }
    }
}
#endif