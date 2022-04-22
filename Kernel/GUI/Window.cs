/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
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
            PNG yehei = new PNG(File.Instance.ReadAllBytes("0:/CASC.PNG"));
            font = new IFont(yehei, "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~", 16);
        }

        public static bool IsUnderMouse(int X,int Y ,int Width,int Height)
        {
            if (Control.MousePosition.X > X && Control.MousePosition.X < X + Width && Control.MousePosition.Y > Y && Control.MousePosition.Y < Y + Height) return true;
            return false;
        }

        public static void MoveToEnd(Window window)
        {
            if (window.Index == Forms.Count - 1) return;

            int index = window.Index;
            for(int i = 0; i < Forms.Count; i++)
            {
                var v = Forms[i];
                if (v.Index > index)
                {
                    v.Index--;
                }
            }
            window.Index = Forms.Count - 1;
        }

        public static void UpdateAll()
        {
            for (int i = 0; i < Forms.Count; i++) 
            {
                if(Forms[i].Visible)
                    Forms[i].OnInput();
            }
            for (int i = 0; i < Forms.Count; i++)
            {
                if (Forms[i].Visible)
                    for (int k = 0; k < Forms.Count; k++)
                    {
                        if (Forms[k].Index == i) Forms[k].OnDraw();
                    }
            }
        }

        public bool Visible;

        public int X, Y, Width, Height;

        public Window(int X, int Y, int Width, int Height)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.Visible = true;
            Index = Forms.Count;
            Forms.Add(this);
            Title = "Form1";
        }

        public int BarHeight = 40;
        public string Title;

        bool Move;
        int OffsetX;
        int OffsetY;
        public int Index;

        public static bool HasFormMoving = false;

        public virtual void OnInput()
        {
            if (Control.MouseButtons == MouseButtons.Left)
            {
                if (!HasFormMoving && !Move && Control.MousePosition.X > X && Control.MousePosition.X < X + Width && Control.MousePosition.Y > Y - BarHeight && Control.MousePosition.Y < Y)
                {
                    MoveToEnd(this);
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
        }

        public virtual void OnDraw()
        {
            Framebuffer.FillRectangle(X, Y - BarHeight, Width, BarHeight, 0xFF111111);
            //ASC16.DrawString(Title, X + ((Width/2)-((Title.Length*8)/2)), Y - BarHeight + (BarHeight / 4), 0xFFFFFFFF);

            //BitFont.DrawString("Song", 0xFFFFFFFF, Title, X + (Width / 2) - ((BitFont.MeasureString("Song", Title)) / 2), Y - BarHeight + (BarHeight / 4));
            font.DrawString(X + (Width / 2) - ((font.MeasureString(Title)) / 2), Y - BarHeight + (BarHeight / 4), Title);
            //BitFont.DrawString("Song", 0xFFFFFFFF, Title, X + (Width / 2) - (BitFont.MeasureString("Song", Title) / 2), Y - BarHeight + (BarHeight / 4));

            Framebuffer.FillRectangle(X, Y, Width, Height, BackgroundColor);
            Framebuffer.DrawRectangle(X - 1, Y - BarHeight - 1, Width + 2, BarHeight + Height + 2, 0xFF333333);
        }
    }
}
