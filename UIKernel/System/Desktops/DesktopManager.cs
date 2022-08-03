﻿using MOOS;
using MOOS.FS;
using MOOS.GUI;
using MOOS.Misc;
using System;
using System.Collections.Generic;
using System.Desktops.Controls;
using System.Diagnostics;
using System.Drawing;
using System.Explorers;
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
        public static string User;

        public static bool IsAtRoot => Dir.Length < 1;
        //static Dictionary<int, string> BuiltInAppNames;
        public static ICommand IconClickCommand { get; set; }
        public static ICommand IconNativeClickCommand { get; set; }
        public static ICommand IconDirectoryClickCommand { get; set; }

        public static void Initialize()
        {
            User = "moos";
            Dir = "";

#if Chinese
			Prefix = $" 管理员@{User}: ";
#else
            Prefix = $" root@{User}: ";
#endif

            //Sized width to 512
            //https://gitlab.com/Enthymeme/hackneyed-x11-cursors/-/blob/master/theme/right-handed-white.svg
            Cursor = new PNG(File.ReadAllBytes("sys/media/Cursor.png"));
            CursorMoving = new PNG(File.ReadAllBytes("sys/media/Grab.png"));
            //Image from unsplash
            Wallpaper = new PNG(File.ReadAllBytes("sys/media/Wallpaper1.png"));

            BitFont.Initialize();

            string CustomCharset = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
            BitFont.RegisterBitFont(new BitFontDescriptor("Song", CustomCharset, File.ReadAllBytes("sys/fonts/Song.btf"), 16));

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
            //BuiltInAppNames = new Dictionary<int, string>();

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
            Debug.WriteLine($"[DesktopBarItem] {item.ToString()}");

            onLoadIcons();
        }

        static void onLoadIcons()
        {
            int BarHeight = bar.Height + 5;
            int Devide = 60;
            int X = 5;
            int Y = BarHeight;
            string devider = "/";
            /*
            BuiltInAppNames.Clear();

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
            */
            IconClickCommand = new ICommand(onDesktopIconClick);
            IconNativeClickCommand = new ICommand(onDesktopNativeOSClick);
            IconDirectoryClickCommand = new ICommand(onDesktopDirectoryClick);

            List<FileInfo> files = File.GetFiles($"home/{User}/Desktop");

            /*
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
                        icon.Content = BuiltInAppNames[icon.Key];
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
            */

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

                if (files[i].Attribute == FileAttribute.Directory)
                {
                    icon.isDirectory = true;
                    icon.Command = IconDirectoryClickCommand;
                }
                else
                {
                    icon.Command = IconClickCommand;
                }

                icon.onLoadIconExtention();

                icons.Add(icon);

                Y += DesktopIcons.FileIcon.Height + Devide;
            }

            files.Dispose();
        }

        static void onDesktopDirectoryClick(object obj)
        {
            string dir = obj as string;
            Debug.WriteLine($"[Directory] {dir}");

            ExplorerManager explorer = new ExplorerManager();
            explorer.Title = dir;
            explorer.Dir = dir;
            explorer.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            explorer.ShowDialog();

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
            Window frm = null;

            Debug.WriteLine($"[Icon] {file}");

            if (string.IsNullOrEmpty(file))
            {
                file.Dispose();
                MessageBox.Show("Can't open file.", "Not Found");
                return;
            }

            if (file.EndsWith(".png"))
            {
                byte[] buffer = File.ReadAllBytes(file);
                PNG png = new(buffer);
                buffer.Dispose();
                ImageViewer img = new ImageViewer(100, 100);
                img.SetImage(png);
                img.ShowDialog();
            }
            else if (file.EndsWith("doom1.wad"))
            {
                frm = new Doom(300, 250);
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
                NESEmu nes = new NESEmu(100,100);
                nes.OpenROM(File.ReadAllBytes(file));
                nes.ShowDialog();
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
            Framebuffer.Graphics.DrawImage(DesktopManager.Wallpaper, (Framebuffer.Width / 2) - (DesktopManager.Wallpaper.Width / 2), (Framebuffer.Height / 2) - (DesktopManager.Wallpaper.Height / 2));

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
