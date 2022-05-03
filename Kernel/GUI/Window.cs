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
    internal class Window
    {
        public static List<Window> Windows;
        public static uint BackgroundColor;
        public static IFont font;
        public static Image CloseButton;

        public static void Initialize()
        {
            Windows = new List<Window>();
            PNG yehei = new PNG(File.Instance.ReadAllBytes("Images/M+.png"));
            CloseButton = new PNG(File.Instance.ReadAllBytes("Images/Close.png"));
            font = new IFont(yehei, "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~", 18);
        }

        public bool IsUnderMouse()
        {
            if (Control.MousePosition.X > X && Control.MousePosition.X < X + Width && Control.MousePosition.Y > Y && Control.MousePosition.Y < Y + Height) return true;
            return false;
        }

        public static void MoveToEnd(Window window)
        {
            if (window.Index == Windows.Count - 1) return;

            int index = window.Index;
            for(int i = 0; i < Windows.Count; i++)
            {
                var v = Windows[i];
                if (v.Index > index)
                {
                    v.Index--;
                }
            }
            window.Index = Windows.Count - 1;
        }

        public static void UpdateAll()
        {
            for (int i = 0; i < Windows.Count; i++) 
            {
                if(Windows[i].Visible)
                    Windows[i].OnInput();
            }
            for (int i = 0; i < Windows.Count; i++)
            {
                for (int k = 0; k < Windows.Count; k++)
                {
                    if (Windows[k].Index == i)
                    {
                        if (Windows[k].Visible)
                            Windows[k].OnDraw();
                    }
                }
            }
        }

        public bool Visible 
        {
            set 
            {
                _visible = value;
                OnSetVisible(value);
            }
            get 
            {
                return _visible;
            }
        }

        public bool _visible;

        public virtual void OnSetVisible(bool value) { }

        public int X, Y, Width, Height;

        public Window(int X, int Y, int Width, int Height)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.Visible = true;
            Index = Windows.Count;
            Windows.Add(this);
            Title = "Window1";
        }

        public int BarHeight = 40;
        public string Title;

        bool Move;
        int OffsetX;
        int OffsetY;
        public int Index;

        public static bool HasWindowMoving = false;

        public virtual void OnInput()
        {
            if (Control.MouseButtons == MouseButtons.Left)
            {
                if (
                    !HasWindowMoving &&
                    Control.MousePosition.X > CloseButtonX && Control.MousePosition.X < CloseButtonX + CloseButton.Width &&
                    Control.MousePosition.Y > CloseButtonY && Control.MousePosition.Y < CloseButtonY + CloseButton.Height
                )
                {
                    this.Visible = false;
                    return;
                }
                if (!HasWindowMoving && !Move && Control.MousePosition.X > X && Control.MousePosition.X < X + Width && Control.MousePosition.Y > Y - BarHeight && Control.MousePosition.Y < Y)
                {
                    MoveToEnd(this);
                    Move = true;
                    HasWindowMoving = true;
                    OffsetX = Control.MousePosition.X - X;
                    OffsetY = Control.MousePosition.Y - Y;
                }
            }
            else
            {
                Move = false;
                HasWindowMoving = false;
            }

            if (Move)
            {
                X = Control.MousePosition.X - OffsetX;
                Y = Control.MousePosition.Y - OffsetY;
            }
        }

        private int CloseButtonX => X + Width + 2 - (BarHeight / 2) - (CloseButton.Width / 2);
        private int CloseButtonY => Y - BarHeight + (BarHeight / 2) - (CloseButton.Height / 2);

        public virtual void OnDraw()
        {
            Framebuffer.Graphics.FillRectangle(X, Y - BarHeight, Width, BarHeight, 0xFF111111);
            //ASC16.DrawString(Title, X + ((Width/2)-((Title.Length*8)/2)), Y - BarHeight + (BarHeight / 4), 0xFFFFFFFF);

            //BitFont.DrawString("Song", 0xFFFFFFFF, Title, X + (Width / 2) - ((BitFont.MeasureString("Song", Title)) / 2), Y - BarHeight + (BarHeight / 4));
            font.DrawString(X + (Width / 2) - ((font.MeasureString(Title)) / 2), Y - BarHeight + (BarHeight / 4), Title);
            //BitFont.DrawString("Song", 0xFFFFFFFF, Title, X + (Width / 2) - (BitFont.MeasureString("Song", Title) / 2), Y - BarHeight + (BarHeight / 4));

            Framebuffer.Graphics.FillRectangle(X, Y, Width, Height, BackgroundColor);
            DrawBorder();

            Framebuffer.Graphics.DrawImage(CloseButtonX, CloseButtonY, CloseButton);
        }

        public void DrawBorder(bool HasBar = true)
        {
            Framebuffer.Graphics.DrawRectangle(X - 1, Y - (HasBar ? BarHeight : 0) - 1, Width + 2, (HasBar ? BarHeight : 0) + Height + 2, 0xFF333333);
        }
    }
}
