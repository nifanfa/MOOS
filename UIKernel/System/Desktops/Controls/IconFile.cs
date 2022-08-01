using MOOS;
using MOOS.FS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace System.Desktops.Controls
{
    public class IconFile : DesktopControl
    {
        public bool isDirectory { set; get; }
        public Image icon { set; get; }
        public ICommand command { set; get; }
        public FileInfo FileInfo { get; set; }

        bool _isFocus;
        bool _clicked;

        public IconFile()
        {
            Foreground = Brushes.White;
            icon = DesktopIcons.FileIcon;
            Width = DesktopIcons.FileIcon.Width;
            Height = DesktopIcons.FileIcon.Height;
        }

        public void onLoadIconExtention()
        {
            if (Content.EndsWith(".png"))
            {
                icon = DesktopIcons.ImageIcon;
            }
            else if (Content.EndsWith("DOOM1.wad"))
            {
                icon = DesktopIcons.DoomIcon;
            }
            else if (Content.EndsWith(".exe"))
            {
                icon = DesktopIcons.AppIcon;
            }
            else if (Content.EndsWith(".wav"))
            {
                icon = DesktopIcons.AudioIcon;
            }
            else if (isDirectory)
            {
                icon = DesktopIcons.FolderIcon;
            }
            else
            {
                icon = DesktopIcons.FileIcon;
            }

        }

        public override void Update()
        {
            base.Update();

            if (Control.MouseButtons.HasFlag(MouseButtons.Left) && !WindowManager.HasWindowMoving && !WindowManager.MouseHandled)
            {
                if (Control.MousePosition.X > X && Control.MousePosition.X < (X + Width) && Control.MousePosition.Y > Y && Control.MousePosition.Y < (Y + Height))
                {
                    _isFocus = true;

                    if (Command != null && Command != null)
                    {
                        if (!_clicked)
                        {
                            _clicked = true;

                            Command.Execute.Invoke(FileInfo);
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

            Framebuffer.Graphics.DrawImage(X, Y, icon);
            WindowManager.font.DrawString(X, (Y + icon.Height), Content, Foreground.Value, (icon.Width + 8), WindowManager.font.FontSize * 3);
        }
    }
}
