using MOOS;
using MOOS.GUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace System.Windows
{

    public enum WindowStartupLocation
    {
        Manual = 0,
        CenterScreen = 1,
        CenterOwner = 2
    }

    public class Window : Widget
    {
        public string Title { set; get; }
        public WindowStartupLocation WindowStartupLocation { get; set; }
        public Widget Focus { set; get; }

        Widget _content;
        public Widget Content
        {
            set
            {
                _content = value;
                _content.Parent = this;
            }
            get
            {
                return _content;
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

        public Window() : base()
        {
            this.Visible = false;
            X= 0;
            Y= 0;
            Width = 300;
            Height = 150;
            Background = Brushes.White;
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            Focus = this;
            WindowManager.Childrens.Add(this);
        }

        public Window(int x, int y, int width, int height)
        {
            this.Visible = false;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Background = Brushes.White;
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            Focus = this;
            WindowManager.Childrens.Add(this);
        }

        public void ShowDialog()
        {
            onWindowStartupLocation();
            WindowManager.MoveToEnd(this);
            this.Visible = true;
        }

        public void Close()
        {
            this.Visible = false;

            this.OnClose();
            if (Content != null)
            {
                Content.Dispose();
            }
            WindowManager.Childrens.Remove(this);
            this.Dispose();
        }

        void onWindowStartupLocation()
        {
            switch (this.WindowStartupLocation)
            {
                case WindowStartupLocation.Manual:
                    break;
                case WindowStartupLocation.CenterOwner:

                    break;
                case WindowStartupLocation.CenterScreen:
                    X = (Framebuffer.Width / 2) - (this.Width / 2);
                    Y = (Framebuffer.Height / 2) - (this.Height / 2);
                    break;
            }

        }

        public bool IsUnderMouse()
        {
            if (Control.MousePosition.X > X && Control.MousePosition.X < X + Width && Control.MousePosition.Y > Y && Control.MousePosition.Y < Y + Height) return true;
            return false;
        }

        public bool _visible;

        public virtual void OnSetVisible(bool value) { }

        public int BarHeight = 40;

        bool Move;
        int OffsetX;
        int OffsetY;
        public int Index { get => WindowManager.Childrens.IndexOf(this); }

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

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Content != null)
            {
                Content.OnUpdate();
            }
        }

        public override void OnDraw()
        {
            base.OnDraw();

            //WindowBar
            Framebuffer.Graphics.FillRectangle(Color.FromArgb(0xFFFFFFFF), X, Y - BarHeight, Width, BarHeight);
            WindowManager.font.DrawString(X + (Width / 2) - ((WindowManager.font.MeasureString(Title)) / 2), Y - BarHeight + (BarHeight / 4), Title, 0xFF000000);

            Framebuffer.Graphics.FillRectangle(Color.FromArgb(Background.Value), X, Y, Width, Height);

            if (Content != null)
            {
                Content.OnDraw();
            }

            if (BorderBrush != null)
            {
                DrawBorder();
            }

            Framebuffer.Graphics.DrawImage(WindowManager.CloseButton, CloseButtonX, CloseButtonY);
        }

        public virtual void OnClose()
        {
            this.Dispose();
        }
    }
}
