/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
#if HasGUI
using MOOS.Driver;
using MOOS.FS;
using MOOS.Misc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace MOOS.GUI
{
    internal class Desktop
    {
        private static Image FileIcon;
        private static Image IamgeIcon;
        private static Image GameIcon;
        private static Image AppIcon;
        private static Image AudioIcon;
        private static Image BuiltInAppIcon;
        private static Image FolderIcon;

        public static string Prefix;
        public static string Dir;
        public static ImageViewer imageViewer;
        public static MessageBox msgbox;
        public static NESEmu nesemu;

        public static bool IsAtRoot 
        {
            get => Desktop.Dir.Length < 1;
        }

        public static void Initialize()
        {
            IndexClicked = -1;
            FileIcon = new PNG(File.Instance.ReadAllBytes("Images/file.png"));
            IamgeIcon = new PNG(File.Instance.ReadAllBytes("Images/Image.png"));
            GameIcon = new PNG(File.Instance.ReadAllBytes("Images/Game.png"));
            AppIcon = new PNG(File.Instance.ReadAllBytes("Images/App.png"));
            AudioIcon = new PNG(File.Instance.ReadAllBytes("Images/Audio.png"));
            BuiltInAppIcon = new PNG(File.Instance.ReadAllBytes("Images/BApp.png"));
            FolderIcon = new PNG(File.Instance.ReadAllBytes("Images/folder.png"));
#if Chinese
            Prefix = " 管理员@Moos: ";
#else
            Prefix = " root@Moos: ";
#endif
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

            BuiltInAppNames = new string[]
            {
#if Chinese
                "计算器",
                " 时钟",
                " 画图",
                "贪吃蛇",
                "控制台",
#else
                "Calculator",
                "Clock",
                "Paint",
                "Snake",
                "Console",
#endif
            };

            LastPoint.X = -1;
            LastPoint.Y = -1;
        }

        public static string[] BuiltInAppNames;

        public static void Update()
        {
            const int BarHeight = 35;

            List<FileInfo> names = File.Instance.GetFiles(Dir);
            int Devide = 60;
            int X = Devide;
            int Y = Devide + BarHeight;

            if(IsAtRoot)
            {
                for (int i = 0; i < BuiltInAppNames.Length; i++)
                {
                    if (Y + FileIcon.Height + Devide > Framebuffer.Graphics.Height - Devide)
                    {
                        Y = Devide + BarHeight;
                        X += FileIcon.Width + Devide;
                    }

                    ClickEvent(BuiltInAppNames[i], false, X, Y, i);

                    Framebuffer.Graphics.DrawImage(X, Y, BuiltInAppIcon);
                    Window.font.DrawString(X, Y + FileIcon.Height, BuiltInAppNames[i], FileIcon.Width + 8, Window.font.FontSize * 3); 
                    Y += FileIcon.Height + Devide;
                }
            }

            for (int i = 0; i < names.Count; i++)
            {
                if (Y + FileIcon.Height + Devide > Framebuffer.Graphics.Height - Devide)
                {
                    Y = Devide + BarHeight;
                    X += FileIcon.Width + Devide;
                }

                ClickEvent(names[i].Name, names[i].Attribute == FileAttribute.Directory, X, Y, i + (IsAtRoot ? BuiltInAppNames.Length : 0));

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
                    (
                    names[i].Name[names[i].Name.Length - 3].ToUpper() == 'P' &&
                    names[i].Name[names[i].Name.Length - 2].ToUpper() == 'C' &&
                    names[i].Name[names[i].Name.Length - 1].ToUpper() == 'M'
                    )
                    )
                {
                    Framebuffer.Graphics.DrawImage(X, Y, AudioIcon);
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

#if Chinese
            string Result = $"{(2000 + RTC.Year)}年{RTC.Month}月{RTC.Day}日,{RTC.Hour}:{RTC.Minute} | FPS:{FPSMeter.FPS} | CPU 0: {ThreadPool.CPUUsage}% | 当前空闲处理器数量: {SMP.NumFreeCPU} | 内存: {(Allocator.MemoryInUse / 1024)}/{((Allocator.NumPages * Allocator.PageSize) / 1024)}kbytes";
#else
            string Result = $"{(2000 + RTC.Year)}/{RTC.Month}/{RTC.Day},{RTC.Hour}:{RTC.Minute} | FPS:{FPSMeter.FPS} | CPU 0: {ThreadPool.CPUUsage}% | Num Free CPU: {SMP.NumFreeCPU} | Memory: {(Allocator.MemoryInUse / 1024)}/{((Allocator.NumPages * Allocator.PageSize) / 1024)}kbytes";
#endif

            //BitFont.DrawString("Song", 0xFFFFFFFF, Result, Framebuffer.Graphics.Width - BitFont.MeasureString("Song", Result) - 16, (BarHeight / 2) - (16 / 2));
            Window.font.DrawString(Framebuffer.Graphics.Width - Window.font.MeasureString(Result) - Window.font.FontSize, (BarHeight / 2) - (Window.font.FontSize / 2), Result);

            if (Control.MouseButtons.HasFlag(MouseButtons.Left) && !Window.HasWindowMoving && !Window.MouseHandled) 
            {
                if (LastPoint.X == -1 && LastPoint.Y == -1) 
                {
                    LastPoint.X = Control.MousePosition.X;
                    LastPoint.Y = Control.MousePosition.Y;
                }
                else 
                {
                    if(Control.MousePosition.X > LastPoint.X && Control.MousePosition.Y > LastPoint.Y)
                    {
                        Framebuffer.Graphics.AFillRectangle(
                            LastPoint.X,
                            LastPoint.Y,
                            Control.MousePosition.X - LastPoint.X,
                            Control.MousePosition.Y - LastPoint.Y,
                            0x7F2E86C1);
                    }

                    if (Control.MousePosition.X < LastPoint.X && Control.MousePosition.Y < LastPoint.Y)
                    {
                        Framebuffer.Graphics.AFillRectangle(
                            Control.MousePosition.X,
                            Control.MousePosition.Y,
                            LastPoint.X - Control.MousePosition.X,
                            LastPoint.Y - Control.MousePosition.Y,
                            0x7F2E86C1);
                    }

                    if (Control.MousePosition.X < LastPoint.X && Control.MousePosition.Y > LastPoint.Y)
                    {
                        Framebuffer.Graphics.AFillRectangle(
                            Control.MousePosition.X,
                            LastPoint.Y,
                            LastPoint.X - Control.MousePosition.X,
                            Control.MousePosition.Y - LastPoint.Y,
                            0x7F2E86C1);
                    }

                    if (Control.MousePosition.X > LastPoint.X && Control.MousePosition.Y < LastPoint.Y)
                    {
                        Framebuffer.Graphics.AFillRectangle(
                            LastPoint.X,
                            Control.MousePosition.Y,
                            Control.MousePosition.X - LastPoint.X,
                            LastPoint.Y - Control.MousePosition.Y,
                            0x7F2E86C1);
                    }
                }
            }
            else
            {
                LastPoint.X = -1;
                LastPoint.Y = -1;
            }

            Result.Dispose();
        }

        public static Point LastPoint;

        private static void ClickEvent(string name,bool isDirectory, int X, int Y, int i)
        {
            if (Control.MouseButtons == MouseButtons.Left)
            {
                bool clickable = true;
                for (int d = 0; d < Window.Windows.Count; d++)
                {
                    if (Window.Windows[d].Visible)
                        if (Window.Windows[d].IsUnderMouse())
                        {
                            clickable = false;
                        }
                }

                if (!Window.HasWindowMoving && clickable && !ClickLock && Control.MousePosition.X > X && Control.MousePosition.X < X + FileIcon.Width && Control.MousePosition.Y > Y && Control.MousePosition.Y < Y + FileIcon.Height)
                {
                    IndexClicked = i;
                    OnClick(name,isDirectory);
                }
            }
            else
            {
                ClickLock = false;
            }

            if (IndexClicked == i)
            {
                int w = (int)(FileIcon.Width * 1.5f);
                Framebuffer.Graphics.AFillRectangle(X + ((FileIcon.Width / 2) - (w / 2)), Y, w, FileIcon.Height * 2, 0x7F2E86C1);
            }
        }

        static bool ClickLock = false;
        static int IndexClicked;

        public static void OnClick(string name, bool isDirectory)
        {
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
                name[name.Length - 3].ToUpper() == 'P' &&
                name[name.Length - 2].ToUpper() == 'C' &&
                name[name.Length - 1].ToUpper() == 'M'
                )
            {
                if (Audio.HasAudioDevice)
                {
                    byte[] buffer = File.Instance.ReadAllBytes(path);
                    Audio.Play(buffer);
                    buffer.Dispose();
                }
                else
                {
                    msgbox.X = Control.MousePosition.X + 50;
                    msgbox.Y = Control.MousePosition.Y + 50;
#if Chinese
                msgbox.SetText("没有兼容的声卡!");
#else
                    msgbox.SetText("Audio controller is unavailable!");
#endif
                    Window.MoveToEnd(msgbox);
                    msgbox.Visible = true;
                }
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

#if Chinese
            else if (name == "计算器")
#else
            else if (name == "Calculator")
#endif
            {
                new Calculator(300, 500);
            }
#if Chinese
            else if (name == " 时钟")
#else
            else if (name == "Clock")
#endif
            {
                new Clock(650, 500);
            }
#if Chinese
            else if (name == " 画图")
#else
            else if (name == "Paint")
#endif
            {
                new Paint(500, 200);
            }
#if Chinese
            else if (name == "贪吃蛇")
#else
            else if (name == "Snake")
#endif
            {
                new Snake(600, 100);
            }
#if Chinese
            else if (name == "控制台")
#else
            else if (name == "Console")
#endif
            {
                Program.FConsole.Visible = true;
            }


            else if (isDirectory) 
            {
                string newd = Dir + devider + name;
                Dir.Dispose();
                Dir = newd;
            }
            else
            {
                msgbox.X = Control.MousePosition.X + 50;
                msgbox.Y = Control.MousePosition.Y + 50;
#if Chinese
                msgbox.SetText("没有任何程序可以打开此程序!");
#else
                msgbox.SetText("No application can open this file!");
#endif
                Window.MoveToEnd(msgbox);
                msgbox.Visible = true;
            }

            path.Dispose();
            devider.Dispose();
        }
    }
}
#endif
