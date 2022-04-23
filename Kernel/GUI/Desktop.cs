/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
using Kernel.FS;
using Kernel.Misc;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Kernel.GUI
{
    internal class Desktop
    {
        private static Image FileIcon;
        public static string CurrentDirectory;
        public static string Dir;
        public static ImageViewer imageViewer;
        public static MessageBox msgbox;

        public static void Initialize()
        {
            IndexClicked = -1;
            FileIcon = new PNG(File.Instance.ReadAllBytes("0:/UNKNOWN.PNG"));
            CurrentDirectory = " root@Moos: / ";
            Dir = "0:/";
            imageViewer = new ImageViewer(400,400);
            msgbox = new MessageBox(100,300);
            imageViewer.Visible = false;
            msgbox.Visible = false;
            Window.Forms.Add(msgbox);
            Window.Forms.Add(imageViewer);
        }

        public static void Update()
        {
            const int BarHeight = 35;

            List<string> names = File.Instance.GetFiles(Dir);
            int Devide = 60;
            int X = Devide;
            int Y = Devide + BarHeight;
            for (int i = 0; i < names.Count; i++)
            {
                if (Y + FileIcon.Height + Devide > Framebuffer.Height - Devide)
                {
                    Y = Devide + BarHeight;
                    X += FileIcon.Width + Devide;
                }

                if(Control.MouseButtons == MouseButtons.Left)
                {
                    bool clickable = true;
                    for(int d = 0; d < Window.Forms.Count; d++) 
                    {
                        if(Window.IsUnderMouse(Window.Forms[d].X, Window.Forms[d].Y, Window.Forms[d].Width, Window.Forms[d].Height)) 
                        {
                            clickable = false;
                        }
                    }
                    
                    if (clickable && !ClickLock && Control.MousePosition.X > X && Control.MousePosition.X < X + FileIcon.Width && Control.MousePosition.Y > Y && Control.MousePosition.Y < Y + FileIcon.Height)
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
                    Framebuffer.AFillRectangle(X + ((FileIcon.Width/2) - (w/2)), Y, w, FileIcon.Height * 2, 0x7F2E86C1);
                }

                Framebuffer.DrawImage(X, Y, FileIcon);
                //BitFont.DrawString("Song", 0xFFFFFFFF, names[i], X, Y + FileIcon.Height, FileIcon.Width + 16);
                Window.font.DrawString(X, Y + FileIcon.Height, names[i], FileIcon.Width);
                Y += FileIcon.Height + Devide;
                names[i].Dispose();
            }
            names.Dispose();

            Framebuffer.FillRectangle(0, 0, Framebuffer.Width, BarHeight, 0xFF111111);
            //BitFont.DrawString("Song", 0xFFFFFFFF, CurrentDirectory, 0, (BarHeight / 2) - (16 / 2));
            Window.font.DrawString(0, (BarHeight / 2) - (Window.font.FontSize / 2), CurrentDirectory, Framebuffer.Width);

            string CPUUsage = ThreadPool.CPUUsage.ToString();
            string Memory = ((Allocator.NumPages * Allocator.PageSize) / 1048576).ToString();
            string MemoryUsed = (Allocator.MemoryInUse / 1048576).ToString();
            string Year = (2000+RTC.Year).ToString();
            string Month = RTC.Month.ToString();
            string Day = RTC.Day.ToString();
            string Hour = RTC.Hour.ToString();
            string Minute = RTC.Minute.ToString();
            string Result = $"{Year}/{Month}/{Day},{Hour}:{Minute} | CPU 0: {CPUUsage}% | Memory: {MemoryUsed}/{Memory}MiB";
            CPUUsage.Dispose();
            Memory.Dispose();
            MemoryUsed.Dispose();
            Year.Dispose();
            Month.Dispose();
            Day.Dispose();
            Hour.Dispose();
            Minute.Dispose();

            //BitFont.DrawString("Song", 0xFFFFFFFF, Result, Framebuffer.Width - BitFont.MeasureString("Song", Result) - 16, (BarHeight / 2) - (16 / 2));
            Window.font.DrawString(Framebuffer.Width - Window.font.MeasureString(Result) - Window.font.FontSize, (BarHeight / 2) - (Window.font.FontSize / 2), Result);

            Result.Dispose();
        }

        static bool ClickLock = false;
        static int IndexClicked;

        public static void OnClick(string name)
        {
            ClickLock = true;
            if (
                name[name.Length-3].ToUpper() == 'P' &&
                name[name.Length-2].ToUpper() == 'N' &&
                name[name.Length-1].ToUpper() == 'G'
                )
            {
                byte[] buffer = File.Instance.ReadAllBytes(name);
                PNG png = new PNG(buffer);
                buffer.Dispose();
                imageViewer.SetImage(png);
                png.Dispose();
                Window.MoveToEnd(imageViewer);
                imageViewer.Visible = true;
            }
            else
            {
                msgbox.SetText("No application can open this file!");
                Window.MoveToEnd(msgbox);
                msgbox.Visible = true;
            }
        }
    }
}
