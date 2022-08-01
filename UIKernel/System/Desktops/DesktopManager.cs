using MOOS;
using MOOS.FS;
using MOOS.Misc;
using System;
using System.Collections.Generic;
using System.Desktops.Controls;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

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
        public static string[] BuiltInAppNames;

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

            BuiltInAppNames = new string[]
            {
            #if Chinese
				            "计算器",
				            " 时钟",
				            " 画图",
				            "贪吃蛇",
				            "控制台",
				            "监视器"
            #else
				            "Calculator",
                            "Clock",
                            "Paint",
                            "Snake",
                            "Console",
                            "Monitor"
            #endif
            };


            List<FileInfo> files = File.GetFiles(Dir);

            Debug.WriteLine($"[Desktop] Files: {files.Count}");

            if (IsAtRoot)
            {
                for (int i = 0; i < BuiltInAppNames.Length; i++)
                {
                    if (Y + DesktopIcons.FileIcon.Height + Devide > Framebuffer.Graphics.Height - Devide)
                    {
                        Y =  BarHeight;
                        X += DesktopIcons.FileIcon.Width + (Devide/2);
                    }

                    IconFile icon = new IconFile();
                    icon.Content = files[i].Name;
                    icon.FileInfo = files[i];
                    icon.X = X;
                    icon.Y = Y;
                    icon.command = new ICommand(onDesktopNativeOSClick);

                    if (files[i].Attribute == FileAttribute.Directory)
                    {
                        icon.isDirectory = true;
                    }

                    icon.onLoadIconExtention();

                    icons.Add(icon);

                    Y += DesktopIcons.FileIcon.Height + Devide;
                }
            }

            for (int i = 0; i < files.Count; i++)
            {
                if (Y + DesktopIcons.FileIcon.Height + Devide > Framebuffer.Graphics.Height - Devide)
                {
                    Y = BarHeight;
                    X += DesktopIcons.FileIcon.Width + (Devide / 2);
                }

                IconFile icon = new IconFile();
                icon.Content = files[i].Name;
                icon.FileInfo = files[i];
                icon.X = X;
                icon.Y = Y;
                icon.command = new ICommand(onDesktopIconClick);

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
            Debug.WriteLine($"[Native Icon]");
        }

        static void onDesktopIconClick(object obj)
        {
            FileInfo info = (FileInfo)obj;
            Debug.WriteLine($"[Icon] {info.Name}");
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
