using MOOS;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;

namespace System.Desktops.Controls
{
    public class DesktopBarClock : DesktopControl
    {
        bool _isFocus;
        bool _clicked;

        public DesktopBarClock()
        {
            Width = 32;
            Height = 32;
            HorizontalAlignment = HorizontalAlignment.Right;
        }

        public override void Update()
        {
            base.Update();

            if (RTC.Minute < 10)
            {
                Content = $"{RTC.Hour}:0{RTC.Minute}";
            }
            else
            {
                Content = $"{RTC.Hour}:{RTC.Minute}";
            }

            int minWidth = (WindowManager.font.MeasureString(Content) + 10);

            if (Width == 0 || Width < minWidth)
            {
                Width = minWidth;
            }

            if (Control.MouseButtons == MouseButtons.Left)
            {
                if (!WindowManager.HasWindowMoving && Control.MousePosition.X > X && Control.MousePosition.X < (X + Width) && Control.MousePosition.Y > Y && Control.MousePosition.Y < (Y + Height))
                {
                    _isFocus = true;

                    if (Command != null && Command.Source != null)
                    {
                        if (!_clicked)
                        {
                            _clicked = true;

                            Command.Source.Execute.Invoke(CommandParameter);
                        }
                    }
                }
            }

            if (Control.MouseButtons == MouseButtons.None)
            {
                _clicked = false;
            }
        }

        public override void Draw()
        {
            base.Draw();


            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    WindowManager.font.DrawString(X + ((Width / 2) - (WindowManager.font.MeasureString(Content) / 2)), Y + (WindowManager.font.FontSize / 2), Content, Foreground.Value);
                    break;
                case HorizontalAlignment.Center:
                    //Nothing
                    break;
                case HorizontalAlignment.Right:
                    WindowManager.font.DrawString((Framebuffer.Graphics.Width - X) - ((Width / 2) + (WindowManager.font.MeasureString(Content) / 2)), Y + (WindowManager.font.FontSize / 2), Content, Foreground.Value);
                    break;
            }

        }
    }
}
