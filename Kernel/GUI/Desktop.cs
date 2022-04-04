/*
 * Copyright(c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
 */
using Kernel.FS;
using Kernel.Misc;
using System.Drawing;

namespace Kernel.GUI
{
    internal class Desktop
    {
        private static Image FileIcon;
        public static string CurrentDirectory;

        public static void Initialize()
        {
            FileIcon = new PNG(File.Instance.ReadAllBytes("/UNKNOWN.PNG"));
            CurrentDirectory = " root@OS-Sharp: / ";
        }

        public static void Update()
        {
            const int BarHeight = 35;

            string[] names = File.Instance.GetFiles();
            int Devide = 60;
            int X = Devide;
            int Y = Devide + BarHeight;
            for (int i = 0; i < names.Length; i++)
            {
                if (Y + FileIcon.Height + Devide > Framebuffer.Height - Devide)
                {
                    Y = Devide + BarHeight;
                    X += FileIcon.Width + Devide;
                }

                Framebuffer.DrawImage(X, Y, FileIcon);
                //BitFont.DrawString("Song", 0xFFFFFFFF, names[i], X, Y + FileIcon.Height, FileIcon.Width + 16);
                Window.font.DrawString(X, Y + FileIcon.Height, names[i], FileIcon.Width);
                Y += FileIcon.Height + Devide;
            }
            names.Dispose();

            Framebuffer.FillRectangle(0, 0, Framebuffer.Width, BarHeight, 0xFF111111);
            //BitFont.DrawString("Song", 0xFFFFFFFF, CurrentDirectory, 0, (BarHeight / 2) - (16 / 2));
            Window.font.DrawString(0, (BarHeight / 2) - (Window.font.FontSize / 2), CurrentDirectory, Framebuffer.Width);

            string CPUUsage = ThreadPool.CPUUsage.ToString();
            string Memory = ((Allocator.NumPages * Allocator.PageSize) / 1048576).ToString();
            string MemoryUsed = (Allocator.MemoryInUse / 1048576).ToString();
            string Result = $"CPU 0: {CPUUsage}% | Memory: {MemoryUsed}/{Memory}MiB";
            CPUUsage.Dispose();
            Memory.Dispose();
            MemoryUsed.Dispose();

            //BitFont.DrawString("Song", 0xFFFFFFFF, Result, Framebuffer.Width - BitFont.MeasureString("Song", Result) - 16, (BarHeight / 2) - (16 / 2));
            Window.font.DrawString(Framebuffer.Width - Window.font.MeasureString(Result) - Window.font.FontSize, (BarHeight / 2) - (Window.font.FontSize / 2), Result);

            Result.Dispose();
        }
    }
}
