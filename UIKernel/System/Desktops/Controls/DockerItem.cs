using MOOS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace System.Desktops.Controls
{
    public class DockerItem : DesktopControl
    {
        public string Name { set; get; }
        Image _icon;
        public Image Icon
        {
            set
            {
                _icon = value;

                if (IconNormal == null)
                {
                    IconNormal = _icon.ResizeImage(_icon.Width, _icon.Height);
                }
                if (IconZoom == null)
                {
                    IconZoom = IconNormal.ResizeImage((int)(IconNormal.Width * zoom), (int)(IconNormal.Height * zoom));
                }
            }
            get { return _icon; }
        }

        Image IconNormal;
        Image IconZoom;

        bool _isFocus;
        bool _clicked;

        double zoom = 1.5;
        bool isZoom;
        public DockerItem()
        {
            Background = Brushes.White;
            Width = 48;
            Height = 48;
        }

        public override void Update()
        {
            base.Update();

            if (!WindowManager.HasWindowMoving && Control.MousePosition.X > (X - (Width/2)) && Control.MousePosition.X < (X + Width) && Control.MousePosition.Y > Y && Control.MousePosition.Y < (Y + Height))
            {
                _isFocus = true;

                if (Control.MouseButtons == MouseButtons.Left)
                {
                    if (Command != null && Command != null)
                    {
                        if (!_clicked)
                        {
                            _clicked = true;

                            Command.Execute.Invoke(CommandParameter);
                        }
                    }
                }
            }
            else
            {
                _isFocus = false;
            }

            if (Control.MouseButtons == MouseButtons.None)
            {
                _clicked = false;
            }

        }

        public override void Draw()
        {
            base.Draw();

            if (_isFocus)
            {
                if (!isZoom)
                {
                    isZoom = true;
                    Icon = IconZoom;
                }
            }
            else
            {
                if (isZoom)
                {
                    isZoom = false;
                    Icon = IconNormal;
                }
            }

            if (Icon != null)
            {
                //Framebuffer.Graphics.FillRectangle((X - (Width/2)), Y, Width, Height , Background.Value);
                Framebuffer.Graphics.DrawImage((X - (Icon.Width / 3)),( (Y + (Height / 2)) - (Icon.Height/2)), Icon);
            }
        }
    }
}
