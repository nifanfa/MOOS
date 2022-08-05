#if HasGUI

using MOOS.FS;
using MOOS.Misc;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MOOS.GUI
{
    internal class Window
    {
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

        public bool IsUnderMouse()
        {
            if (Control.MousePosition.X > X && Control.MousePosition.X < X + Width && Control.MousePosition.Y > Y && Control.MousePosition.Y < Y + Height) return true;
            return false;
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
            WindowManager.Windows.Add(this);
#if Chinese
            Title = "窗体1";
#else
            Title = "Window1";
#endif
            WindowManager.MoveToEnd(this);
        }

        public int BarHeight = 40;
        public string Title;

        bool Move;
        int OffsetX;
        int OffsetY;
        public int Index { get => WindowManager.Windows.IndexOf(this); }

        public virtual void OnInput()
        {
            if (Control.MouseButtons == MouseButtons.Left)
            {
                if (
                    !WindowManager.HasWindowMoving &&
                    Control.MousePosition.X > CloseButtonX && Control.MousePosition.X < CloseButtonX + WindowManager.CloseButton.Width &&
                    Control.MousePosition.Y > CloseButtonY && Control.MousePosition.Y < CloseButtonY + WindowManager.CloseButton.Height
                )
                {
                    this.Visible = false;
                    return;
                }
                if (!WindowManager.HasWindowMoving && !Move && Control.MousePosition.X > X && Control.MousePosition.X < X + Width && Control.MousePosition.Y > Y - BarHeight && Control.MousePosition.Y < Y)
                {
                    WindowManager.MoveToEnd(this);
                    Move = true;
                    WindowManager.HasWindowMoving = true;
                    OffsetX = Control.MousePosition.X - X;
                    OffsetY = Control.MousePosition.Y - Y;
                }
            }
            else
            {
                Move = false;
                WindowManager.HasWindowMoving = false;
            }

            if (Move)
            {
                X = Control.MousePosition.X - OffsetX;
                Y = Control.MousePosition.Y - OffsetY;
            }
        }

        private int CloseButtonX => X + Width + 2 - (BarHeight / 2) - (WindowManager.CloseButton.Width / 2);
        private int CloseButtonY => Y - BarHeight + (BarHeight / 2) - (WindowManager.CloseButton.Height / 2);

        public virtual void OnDraw()
        {
            Framebuffer.Graphics.FillRectangle(X, Y - BarHeight, Width, BarHeight, 0xFF111111);
            //ASC16.DrawString(Title, X + ((Width/2)-((Title.Length*8)/2)), Y - BarHeight + (BarHeight / 4), 0xFFFFFFFF);

            //BitFont.DrawString("Song", 0xFFFFFFFF, Title, X + (Width / 2) - ((BitFont.MeasureString("Song", Title)) / 2), Y - BarHeight + (BarHeight / 4));
            WindowManager.font.DrawString(X + (Width / 2) - ((WindowManager.font.MeasureString(Title)) / 2), Y - BarHeight + (BarHeight / 4), Title);
            //BitFont.DrawString("Song", 0xFFFFFFFF, Title, X + (Width / 2) - (BitFont.MeasureString("Song", Title) / 2), Y - BarHeight + (BarHeight / 4));

            Framebuffer.Graphics.FillRectangle(X, Y, Width, Height, 0xFF222222);
            DrawBorder();

            Framebuffer.Graphics.DrawImage(CloseButtonX, CloseButtonY, WindowManager.CloseButton);
        }

        public void DrawBorder(bool HasBar = true)
        {
            Framebuffer.Graphics.DrawRectangle(X - 1, Y - (HasBar ? BarHeight : 0) - 1, Width + 2, (HasBar ? BarHeight : 0) + Height + 2, 0xFF333333, 1);
        }
    }
}
#endif