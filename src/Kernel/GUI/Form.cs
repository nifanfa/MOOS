/*
* Copyright (c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
*/
using System.Collections.Generic;
using System.Windows.Forms;
using OS_Sharp.FileSystem;
using OS_Sharp.Misc;

namespace OS_Sharp.GUI
{
    internal class Form
    {
        public static List<Form> Forms;
        public static uint BackgroundColor;
        //public static IFont font;

        public static void Initialize()
        {
            //PNG yehei = new PNG(File.Instance.ReadAllBytes("/FONT.PNG"));
            //font = new IFont(yehei, "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~");
        }

        public static void UpdateAll()
        {
            for (int i = 0; i < Forms.Count; i++)
            {
                Forms[i].Update();
            }
        }

        public int X, Y, Width, Height;

        public Form(int X, int Y, int Width, int Height)
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
        private bool Move;
        private int OffsetX;
        private int OffsetY;

        public virtual void Update()
        {
            if (Control.MouseButtons == MouseButtons.Left)
            {
                if (!Move && Control.MousePosition.X > X && Control.MousePosition.X < X + Width && Control.MousePosition.Y > Y - BarHeight && Control.MousePosition.Y < Y)
                {
                    Move = true;
                    OffsetX = Control.MousePosition.X - X;
                    OffsetY = Control.MousePosition.Y - Y;
                }
            }
            else
            {
                Move = false;
            }

            if (Move)
            {
                X = Control.MousePosition.X - OffsetX;
                Y = Control.MousePosition.Y - OffsetY;
            }

            Framebuffer.Fill(X, Y - BarHeight - 5, Width, 5, 0xFF2E86C1);
            Framebuffer.Fill(X, Y - BarHeight, Width, BarHeight, 0xFF424949);
            //ASC16.DrawString(Title, X + ((Width/2)-((Title.Length*8)/2)), Y - BarHeight + (BarHeight / 4), 0xFFFFFFFF);

            BitFont.DrawString("Song", 0xFFFFFFFF, Title, X + (Width / 2) - ((BitFont.MeasureString("Song", Title)) / 2), Y - BarHeight + (BarHeight / 4));
            //font.DrawString(X + (Width / 2) - ((font.MeasureString(Title)) / 2), Y - BarHeight + (BarHeight / 4), Title);
            //BitFont.DrawString("Song", 0xFFFFFFFF, Title, X + (Width / 2) - (BitFont.MeasureString("Song", Title) / 2), Y - BarHeight + (BarHeight / 4));

            Framebuffer.Fill(X, Y, Width, Height, BackgroundColor);
        }
    }
}
