#if HasGUI

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
            get => _visible;
        }

        public bool IsUnderMouse()
        {
            return Control.MousePosition.X > X && Control.MousePosition.X < X + Width && Control.MousePosition.Y > Y && Control.MousePosition.Y < Y + Height;
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
            Visible = true;
            WindowManager.Windows.Add(this);
#if Chinese
            Title = "窗体1";
#else
            Title = "Window";
#endif
            WindowManager.MoveToEnd(this);
        }

        public int BarHeight = 40;
        public string Title;
        private bool Move;
        private int OffsetX;
        private int OffsetY;
        public int Index => WindowManager.Windows.IndexOf(this);

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
                    Visible = false;
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
            } else
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
            Framebuffer.Graphics.FillRectangle(Color.FromArgb(0xFF111111), X, Y - BarHeight, Width, BarHeight);
            //ASC16.DrawString(Title, X + ((Width/2)-((Title.Length*8)/2)), Y - BarHeight + (BarHeight / 4), 0xFFFFFFFF);

            //BitFont.DrawString("Song", 0xFFFFFFFF, Title, X + (Width / 2) - ((BitFont.MeasureString("Song", Title)) / 2), Y - BarHeight + (BarHeight / 4));
            WindowManager.font.DrawString(X + (Width / 2) - (WindowManager.font.MeasureString(Title) / 2), Y - BarHeight + (BarHeight / 4), Title);
            //BitFont.DrawString("Song", 0xFFFFFFFF, Title, X + (Width / 2) - (BitFont.MeasureString("Song", Title) / 2), Y - BarHeight + (BarHeight / 4));

            Framebuffer.Graphics.FillRectangle(Color.FromArgb(0xFF222222), X, Y, Width, Height);
            DrawBorder();

            Framebuffer.Graphics.DrawImage(WindowManager.CloseButton, CloseButtonX, CloseButtonY, true);
        }

        public void DrawBorder(bool HasBar = true)
        {
            Framebuffer.Graphics.DrawRectangle(Color.FromArgb(0xFF333333), X - 1, Y - (HasBar ? BarHeight : 0) - 1, Width + 2, (HasBar ? BarHeight : 0) + Height + 2);
        }
    }
}
#endif