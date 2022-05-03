/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
using Kernel.Driver;
using Kernel.FS;
using Kernel.Misc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Kernel.GUI
{
    internal class Desktop
    {
        private static Image FileIcon;
        private static Image IamgeIcon;
        private static Image GameIcon;
        private static Image AppIcon;
        private static Image FolderIcon;

        public static string Prefix;
        public static string Dir;
        public static ImageViewer imageViewer;
        public static MessageBox msgbox;
        public static NESEmu nesemu;

        public static void Initialize()
        {
            IndexClicked = -1;
            FileIcon = new PNG(File.Instance.ReadAllBytes("Images/file.png"));
            IamgeIcon = new PNG(File.Instance.ReadAllBytes("Images/Image.png"));
            GameIcon = new PNG(File.Instance.ReadAllBytes("Images/Game.png"));
            AppIcon = new PNG(File.Instance.ReadAllBytes("Images/App.png"));
            FolderIcon = new PNG(File.Instance.ReadAllBytes("Images/folder.png"));
            Prefix = " root@Moos: ";
            Dir = "";
            imageViewer = new ImageViewer(400,400);
            msgbox = new MessageBox(100,300);
            nesemu = new(150, 350);
            imageViewer.Visible = false;
            msgbox.Visible = false;
            nesemu.Visible = false;
            Window.Windows.Add(msgbox);
            Window.Windows.Add(imageViewer);
            Window.Windows.Add(nesemu);
        }

        public static void Update()
        {
            const int BarHeight = 35;

            List<FileInfo> names = File.Instance.GetFiles(Dir);
            int Devide = 60;
            int X = Devide;
            int Y = Devide + BarHeight;
            for (int i = 0; i < names.Count; i++)
            {
                if (Y + FileIcon.Height + Devide > Framebuffer.Graphics.Height - Devide)
                {
                    Y = Devide + BarHeight;
                    X += FileIcon.Width + Devide;
                }

                if(Control.MouseButtons == MouseButtons.Left)
                {
                    bool clickable = true;
                    for(int d = 0; d < Window.Windows.Count; d++) 
                    {
                        if(Window.Windows[d].Visible)
                            if(Window.Windows[d].IsUnderMouse()) 
                            {
                                clickable = false;
                            }
                    }
                    
                    if (!Window.HasWindowMoving && clickable && !ClickLock && Control.MousePosition.X > X && Control.MousePosition.X < X + FileIcon.Width && Control.MousePosition.Y > Y && Control.MousePosition.Y < Y + FileIcon.Height)
                    {
                        IndexClicked = i;
                        OnClick(names[i]);
                    }
                }
                else 
                {
                    ClickLock = false;
                }

                if(IndexClicked == i) 
                {
                    int w = (int)(FileIcon.Width * 1.5f);
                    Framebuffer.Graphics.AFillRectangle(X + ((FileIcon.Width/2) - (w/2)), Y, w, FileIcon.Height * 2, 0x7F2E86C1);
                }


                if (
                    (
                    names[i].Name[names[i].Name.Length - 3].ToUpper() == 'P' &&
                    names[i].Name[names[i].Name.Length - 2].ToUpper() == 'N' &&
                    names[i].Name[names[i].Name.Length - 1].ToUpper() == 'G'
                    ) ||
                    (
                    names[i].Name[names[i].Name.Length - 3].ToUpper() == 'B' &&
                    names[i].Name[names[i].Name.Length - 2].ToUpper() == 'M' &&
                    names[i].Name[names[i].Name.Length - 1].ToUpper() == 'P'
                    )
                    )
                {
                    Framebuffer.Graphics.DrawImage(X, Y, IamgeIcon);
                }
                else if
                    (
                    (
                    names[i].Name[names[i].Name.Length - 3].ToUpper() == 'N' &&
                    names[i].Name[names[i].Name.Length - 2].ToUpper() == 'E' &&
                    names[i].Name[names[i].Name.Length - 1].ToUpper() == 'S'
                    )
                    )
                {
                    Framebuffer.Graphics.DrawImage(X, Y, GameIcon);
                }
                else if
                    (
                    (
                    names[i].Name[names[i].Name.Length - 3].ToUpper() == 'E' &&
                    names[i].Name[names[i].Name.Length - 2].ToUpper() == 'X' &&
                    names[i].Name[names[i].Name.Length - 1].ToUpper() == 'E'
                    )
                    )
                {
                    Framebuffer.Graphics.DrawImage(X, Y, AppIcon);
                }
                else if
                    (
                    names[i].Attribute == FileAttribute.Directory
                    )
                {
                    Framebuffer.Graphics.DrawImage(X, Y, FolderIcon);
                }
                else
                {
                    Framebuffer.Graphics.DrawImage(X, Y, FileIcon);
                }
                //BitFont.DrawString("Song", 0xFFFFFFFF, names[i], X, Y + FileIcon.Height, FileIcon.Width + 16);
                Window.font.DrawString(X, Y + FileIcon.Height, names[i].Name, FileIcon.Width + 8, Window.font.FontSize * 3);
                Y += FileIcon.Height + Devide;
                names[i].Dispose();
            }
            names.Dispose();

            Framebuffer.Graphics.FillRectangle(0, 0, Framebuffer.Graphics.Width, BarHeight, 0xFF111111);
            //BitFont.DrawString("Song", 0xFFFFFFFF, CurrentDirectory, 0, (BarHeight / 2) - (16 / 2));
            
            string pre = Prefix + Dir;
            Window.font.DrawString(0, (BarHeight / 2) - (Window.font.FontSize / 2), pre, Framebuffer.Graphics.Width);
            pre.Dispose();

            string CPUUsage = ThreadPool.CPUUsage.ToString();
            string Memory = ((Allocator.NumPages * Allocator.PageSize) / 1048576).ToString();
            string MemoryUsed = (Allocator.MemoryInUse / 1048576).ToString();
            string Year = (2000+RTC.Year).ToString();
            string Month = RTC.Month.ToString();
            string Day = RTC.Day.ToString();
            string Hour = RTC.Hour.ToString();
            string Minute = RTC.Minute.ToString();
            string FPS = FPSMeter.FPS.ToString();
            string Result = $"{Year}/{Month}/{Day},{Hour}:{Minute} | FPS:{FPS} | CPU 0: {CPUUsage}% | Memory: {MemoryUsed}/{Memory}MiB";
            CPUUsage.Dispose();
            Memory.Dispose();
            MemoryUsed.Dispose();
            Year.Dispose();
            Month.Dispose();
            Day.Dispose();
            Hour.Dispose();
            Minute.Dispose();
            FPS.Dispose();

            //BitFont.DrawString("Song", 0xFFFFFFFF, Result, Framebuffer.Graphics.Width - BitFont.MeasureString("Song", Result) - 16, (BarHeight / 2) - (16 / 2));
            Window.font.DrawString(Framebuffer.Graphics.Width - Window.font.MeasureString(Result) - Window.font.FontSize, (BarHeight / 2) - (Window.font.FontSize / 2), Result);

            Result.Dispose();
        }

        static bool ClickLock = false;
        static int IndexClicked;

        public static void OnClick(FileInfo info)
        {
            string name = info.Name;
            ClickLock = true;

            string devider = "/";
            string path = Dir + devider + name;

            if (
                name[name.Length - 3].ToUpper() == 'P' &&
                name[name.Length - 2].ToUpper() == 'N' &&
                name[name.Length - 1].ToUpper() == 'G'
                )
            {
                byte[] buffer = File.Instance.ReadAllBytes(path);
                PNG png = new PNG(buffer);
                buffer.Dispose();
                imageViewer.SetImage(png);
                png.Dispose();
                Window.MoveToEnd(imageViewer);
                imageViewer.Visible = true;
            }
            else if (
                name[name.Length - 3].ToUpper() == 'E' &&
                name[name.Length - 2].ToUpper() == 'X' &&
                name[name.Length - 1].ToUpper() == 'E'
                )
            {
                Window.MoveToEnd(Program.FConsole);
                if (Program.FConsole.Visible == false)
                    Program.FConsole.Visible = true;

                //TO-DO disposing
                Console.WriteLine("Loading EXE...");

                byte[] buffer = File.Instance.ReadAllBytes(path);
                Process.Start(buffer);
            }
            else if (
                name[name.Length - 3].ToUpper() == 'N' &&
                name[name.Length - 2].ToUpper() == 'E' &&
                name[name.Length - 1].ToUpper() == 'S'
                )
            {
                nesemu.OpenROM(File.Instance.ReadAllBytes(path));
                Window.MoveToEnd(nesemu);
                nesemu.Visible = true;
            }
            else if (info.Attribute == FileAttribute.Directory) 
            {
                string newd = Dir + devider + name;
                Dir.Dispose();
                Dir = newd;
            }
            else
            {
                msgbox.X = Control.MousePosition.X + 50;
                msgbox.Y = Control.MousePosition.Y + 50;
                msgbox.SetText("No application can open this file!");
                Window.MoveToEnd(msgbox);
                msgbox.Visible = true;
            }

            path.Dispose();
            devider.Dispose();
        }
    }
}
