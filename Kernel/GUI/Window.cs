/*
 * Copyright(c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
 */
using Kernel.FS;
using Kernel.Misc;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Kernel.GUI
{
    internal class Window
    {
        public static List<Window> Forms;
        public static uint BackgroundColor;
        public static IFont font;

        public static void Initialize()
        {
            Forms = new List<Window>();
            PNG yehei = new PNG(File.Instance.ReadAllBytes("/CASC.PNG"));
            font = new IFont(yehei, "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~", 16);
        }

        public static void UpdateAll()
        {
            for (int i = Forms.Count - 1; i >= 0; i--)
            {
                Forms[i].Update();
            }
        }

        public int X, Y, Width, Height;

        public Window(int X, int Y, int Width, int Height)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            Forms.Add(this);
            Title = "Form1";
        }

        public int BarHeight = 40;
        public string Title;

        bool Move;
        int OffsetX;
        int OffsetY;

        public static bool HasFormMoving = false;

        public virtual void Update()
        {
            if (Control.MouseButtons == MouseButtons.Left)
            {
                if (!HasFormMoving && !Move && Control.MousePosition.X > X && Control.MousePosition.X < X + Width && Control.MousePosition.Y > Y - BarHeight && Control.MousePosition.Y < Y)
                {
                    Forms.Insert(0, this, false);
                    Move = true;
                    HasFormMoving = true;
                    OffsetX = Control.MousePosition.X - X;
                    OffsetY = Control.MousePosition.Y - Y;
                }
            }
            else
            {
                Move = false;
                HasFormMoving = false;
            }

            if (Move)
            {
                X = Control.MousePosition.X - OffsetX;
                Y = Control.MousePosition.Y - OffsetY;
            }

            Framebuffer.Fill(X, Y - BarHeight, Width, BarHeight, 0xFF111111);
            //ASC16.DrawString(Title, X + ((Width/2)-((Title.Length*8)/2)), Y - BarHeight + (BarHeight / 4), 0xFFFFFFFF);

            //BitFont.DrawString("Song", 0xFFFFFFFF, Title, X + (Width / 2) - ((BitFont.MeasureString("Song", Title)) / 2), Y - BarHeight + (BarHeight / 4));
            font.DrawString(X + (Width / 2) - ((font.MeasureString(Title)) / 2), Y - BarHeight + (BarHeight / 4), Title);
            //BitFont.DrawString("Song", 0xFFFFFFFF, Title, X + (Width / 2) - (BitFont.MeasureString("Song", Title) / 2), Y - BarHeight + (BarHeight / 4));

            Framebuffer.Fill(X, Y, Width, Height, BackgroundColor);
            Framebuffer.DrawRectangle(X, Y - BarHeight, Width, BarHeight + Height, 0xFF333333);
        }
    }
}
