#if HasGUI && Kernel
using MOOS.Driver;
using MOOS.FS;
using MOOS.Misc;
using System;

namespace MOOS.GUI
{
    public static class Lockscreen
    {
        static IFont lsfont;
        static string[] mon;

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
                "Augest",
                "September",
                "October",
                "November",
                "December"
            };

            lsfont = new IFont(new PNG(File.Instance.ReadAllBytes("Images/Yahei128.png")), "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~", 128);

            int alpha = 0;
            while (alpha < 0xFF)
            {
                alpha += 16;
                Framebuffer.Graphics.Clear(0x0);
                Framebuffer.Graphics.ADrawImage((Framebuffer.Width / 2) - (Program.Wallpaper.Width / 2), ((Framebuffer.Height / 2) - ((Program.Wallpaper.Height) / 2)), Program.Wallpaper, (byte)alpha);
                Framebuffer.Update();
            }

            while (Keyboard.KeyInfo.Key != System.ConsoleKey.Up && Keyboard.KeyInfo.Key != System.ConsoleKey.W) 
            {
                Framebuffer.Graphics.DrawImage((Framebuffer.Width / 2) - (Program.Wallpaper.Width / 2), (Framebuffer.Height / 2) - (Program.Wallpaper.Height / 2), Program.Wallpaper, false);
                
                string s = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}";
                lsfont.DrawString((Framebuffer.Width /2) - (lsfont.MeasureString(s)/2), Framebuffer.Height / 6, s);
                s.Dispose();

                string m = $"{mon[DateTime.Now.Month]}, {DateTime.Now.Day}th";
                WindowManager.font.DrawString((Framebuffer.Width / 2) - (WindowManager.font.MeasureString(m) / 2), (Framebuffer.Height / 6)+lsfont.FontSize, m);
                m.Dispose();

                Framebuffer.Update();
            }

            int y = 0;
            while (y < Program.Wallpaper.Height)
            {
                y+=8;

                Framebuffer.Graphics.Clear(0x0);
                Framebuffer.Graphics.DrawImage((Framebuffer.Width / 2) - (Program.Wallpaper.Width / 2), ((Framebuffer.Height / 2) - ((Program.Wallpaper.Height) / 2)) - y, Program.Wallpaper, false);
                Framebuffer.Update();
            }
        }
    }
}
#endif