using MOOS;
using MOOS.FS;
using MOOS.GUI;
using MOOS.Misc;
using System;
using System.Collections.Generic;
using System.Desktops.Controls;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using static MOOS.stdio;

namespace System.Desktops
{
    public static class DesktopManager
    {
        static DesktopBar bar { set; get; }
        static DesktopDocker docker { set; get; }
        static List<DesktopControl> barMenu { set; get; }
        static List<IconFile> icons { set; get; }
        public static Image Cursor;
        static Image CursorMoving;
        public static Image Wallpaper;

        public static string Prefix;
        public static string Dir;
        public static bool IsAtRoot => Dir.Length < 1;
        public static Dictionary<int, string> BuiltInAppNames;
        static ICommand IconClickCommand { get; set; }
        static ICommand IconNativeClickCommand { get; set; }

        public static void Initialize()
        {
#if Chinese
			Prefix = " 管理员@Moos: ";
#else
            Prefix = " root@Moos: ";
#endif

            Dir = "";

            //Sized width to 512
            //https://gitlab.com/Enthymeme/hackneyed-x11-cursors/-/blob/master/theme/right-handed-white.svg
            Cursor = new PNG(File.ReadAllBytes("Images/Cursor.png"));
            CursorMoving = new PNG(File.ReadAllBytes("Images/Grab.png"));
            //Image from unsplash
            Wallpaper = new PNG(File.ReadAllBytes("Images/Wallpaper1.png"));

            BitFont.Initialize();

            string CustomCharset = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
            BitFont.RegisterBitFont(new BitFontDescriptor("Song", CustomCharset, File.ReadAllBytes("Song.btf"), 16));

            Image wall = Wallpaper;
            Wallpaper = wall.ResizeImage(Framebuffer.Width, Framebuffer.Height);
            wall.Dispose();

            DesktopIcons.Initialize();
            WindowManager.Initialize();
            MessageBox.Initialize();
            CursorManager.Initialize();

            bar = new DesktopBar();
            docker = new DesktopDocker();
            barMenu = new List<DesktopControl>();
            icons = new List<IconFile>();

            //Bar Elements
            DesktopBarItem item = new DesktopBarItem();
            item.Content = "Desktop";
            item.X = 0;
            item.Y = 0;
            item.Command = new ICommand(onItemDesktop);
            barMenu.Add(item);

            DesktopBarClock clock = new DesktopBarClock();
            clock.HorizontalAlignment = Windows.HorizontalAlignment.Right;
            clock.X = 5;
            clock.Y = 0;
            clock.Command = new ICommand(onItemClock);
            barMenu.Add(clock);

            onLoadIcons();
        }

        static void onLoadIcons()
        {
            int BarHeight = bar.Height + 5;
            int Devide = 60;
            int X = 5;
            int Y = BarHeight;
            string devider = "/";

            BuiltInAppNames = new Dictionary<int, string>();

#if Chinese
            BuiltInAppNames.Add(1, "计算器");
            BuiltInAppNames.Add(2, " 时钟");
            BuiltInAppNames.Add(3, " 画图");
            BuiltInAppNames.Add(4, "贪吃蛇");
            BuiltInAppNames.Add(5, "控制台");
            BuiltInAppNames.Add(6, "监视器");

#else
            BuiltInAppNames.Add(1, "Calculator");
            BuiltInAppNames.Add(2, "Clock");
            BuiltInAppNames.Add(3, "Paint");
            BuiltInAppNames.Add(4, "Snake");
            BuiltInAppNames.Add(5, "Console");
            BuiltInAppNames.Add(6, "Monitor");
#endif

            IconClickCommand = new ICommand(onDesktopIconClick);
            IconNativeClickCommand = new ICommand(onDesktopNativeOSClick);

            List<FileInfo> files = File.GetFiles(Dir);

            if (IsAtRoot)
            {
                for (int i = 0; i < BuiltInAppNames.Count; i++)
                {
                    if (Y + DesktopIcons.FileIcon.Height + Devide > Framebuffer.Graphics.Height - Devide)
                    {
                        Y =  BarHeight;
                        X += DesktopIcons.FileIcon.Width + (Devide/2);
                    }

                    if (BuiltInAppNames.ContainsKey((i + 1)))
                    {
                        IconFile icon = new IconFile();
                        icon.Key = (i + 1);
                        icon.Content = BuiltInAppNames[(i + 1)];
                        icon.Path = Dir + devider;
                        icon.FilePath = Dir + devider + icon.Content;
                        icon.FileInfo = null;
                        icon.X = X;
                        icon.Y = Y;
                        icon.Command = IconNativeClickCommand;
                        icon.icon = DesktopIcons.BuiltInAppIcon;

                        icons.Add(icon);

                        Y += DesktopIcons.FileIcon.Height + Devide;
                    }
                }
            }

            for (int i = 0; i < files.Count; i++)
            {
                if (Y + DesktopIcons.FileIcon.Height + Devide > Framebuffer.Graphics.Height - Devide)
                {
                    Y = BarHeight;
                    X += DesktopIcons.FileIcon.Width + (Devide / 2);
                }

                if (files[i].Attribute == FileAttribute.Hidden || files[i].Attribute == FileAttribute.System)
                {
                    continue;
                }

                IconFile icon = new IconFile();
                icon.Content = files[i].Name;
                icon.Path = Dir + devider;
                icon.FilePath = Dir + devider + icon.Content;
                icon.FileInfo = files[i];
                icon.X = X;
                icon.Y = Y;
                icon.Command = IconClickCommand;

                if (files[i].Attribute == FileAttribute.Directory)
                {
                    icon.isDirectory = true;
                }

                icon.onLoadIconExtention();

                icons.Add(icon);

                Y += DesktopIcons.FileIcon.Height + Devide;
            }

            files.Dispose();
        }

        static void onDesktopNativeOSClick(object obj)
        {
            int key = Convert.ToInt32(obj.ToString());

            Debug.WriteLine($"[Icon] {key}");
            Window frm = null;

            switch (key)
            {
                case 1:
                    frm = new Calculator(300, 500);
                    frm.ShowDialog();
                    break;
                case 2:
                    frm = new Clock(650, 500);
                    frm.ShowDialog();
                    break;
                case 3:
                    frm = new Paint(500, 200);
                    frm.ShowDialog();
                    break;
                case 4:
                    frm = new Snake(600, 100);
                    frm.ShowDialog();
                    break;
                case 5:
                    Program.FConsole.Visible = true;
                    break;
                case 6:
                    frm = new Monitor(200, 450);
                    frm.ShowDialog();
                    break;
                default:
                    MessageBox.Show("Can't open application.", "Not Found");
                    break;
            }
        }

        static void onDesktopIconClick(object obj)
        {
            string file = obj.ToString().ToLower();

            Debug.WriteLine($"[Icon] {file}");

            if (string.IsNullOrEmpty(file))
            {
                file.Dispose();
                MessageBox.Show("Can't open file.", "Not Found");
                return;
            }

            if (file.EndsWith(".png"))
            {
                //byte[] buffer = File.ReadAllBytes(file);
                //PNG png = new(buffer);
                //buffer.Dispose();
               // png.Dispose();
        
            }
            else if (file.EndsWith("DOOM1.wad"))
            {
                Doom frm = new Doom(300, 250);
                frm.ShowDialog();
            }
            else if (file.EndsWith(".exe"))
            {
                byte[] buffer = File.ReadAllBytes(file);
                Process.Start(buffer);
            }
            else if (file.EndsWith(".wav"))
            {
                if (Audio.HasAudioDevice)
                {
                    byte[] buffer = File.ReadAllBytes(file);
                    WAV.Decode(buffer, out byte[] pcm);
                    Audio.Play(pcm);
                    pcm.Dispose();
                    buffer.Dispose();
                }
                else
                {
                    MessageBox.Show("Audio controller is unavailable!", "Error");
                }
            }
            else if (file.EndsWith(".nes"))
            {
                //nesemu.OpenROM(File.ReadAllBytes(file));
                //WindowManager.MoveToEnd(nesemu);
                //nesemu.Visible = true;
            }
            else 
            {
                MessageBox.Show("Can't open file.", "Not Found");
            }

            file.Dispose();
        }

        static void onItemDesktop(object obj)
        {
            Debug.WriteLine($"[Item] Desktop");
        }

        static void onItemClock(object obj)
        {
            Debug.WriteLine($"[Item] Clock");
        }

        public static void Update()
        {
            for (int i = 0; i < icons.Count; i++)
            {
                icons[i].Update();
            }

            docker.Update();
            bar.Update();

            for (int i = 0; i < barMenu.Count; i++)
            {
                barMenu[i].Update();
            }
        }

        public static void Draw()
        {
            Framebuffer.Graphics.DrawImage((Framebuffer.Width / 2) - (DesktopManager.Wallpaper.Width / 2), (Framebuffer.Height / 2) - (DesktopManager.Wallpaper.Height / 2), DesktopManager.Wallpaper, false);

            for (int i = 0; i < icons.Count; i++)
            {
                icons[i].Draw();
            }

            docker.Draw();
            bar.Draw();

            for (int i = 0; i < barMenu.Count; i++)
            {
                barMenu[i].Draw();
            }
        }
    }
}
