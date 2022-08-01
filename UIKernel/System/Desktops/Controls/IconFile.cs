using MOOS;
using MOOS.FS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace System.Desktops.Controls
{
    public class IconFile : DesktopControl
    {
        public int Key { set; get; }
        public bool isDirectory { set; get; }
        public Image icon { set; get; }
        public string Path { set; get; }
        public string FilePath { set; get; }
        public Brush FocusBackground { set; get; }
        public FileInfo FileInfo { get; set; }

        bool _isFocus;
        bool _clicked;
        int offsetX, offsetY;
        public IconFile()
        {
            Foreground = Brushes.White;
            FocusBackground = Brushes.CornflowerBlue;

            icon = DesktopIcons.FileIcon;
            Width = DesktopIcons.FileIcon.Width;
            Height = DesktopIcons.FileIcon.Height;
            offsetX = 5;
            offsetY = 5;
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

            if (Control.MousePosition.X > (X - offsetX) && Control.MousePosition.X < ((X- offsetX) + Width) && Control.MousePosition.Y > Y && Control.MousePosition.Y < (Y + Height))
            {
                _isFocus = true;
                if (Control.MouseButtons.HasFlag(MouseButtons.Left) && !WindowManager.HasWindowMoving && !WindowManager.MouseHandled)
                {
                    _isFocus = true;

                    if (Command != null)
                    {
                        if (!_clicked)
                        {
                            _clicked = true;

                            if (isDirectory)
                            {
                                Command.Execute.Invoke(FilePath);
                            }
                            else
                            {
                                if (Key == 0)
                                {
                                    Command.Execute.Invoke(FilePath);
                                }
                                else
                                {
                                    Command.Execute.Invoke(Key);
                                }
                            }
                       
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
                Framebuffer.Graphics.AFillRectangle((X - offsetX), (Y - offsetY), (Width + (offsetX*2)), (Height + ((WindowManager.font.FontSize * 3) + offsetX)), FocusBackground.Value);
            }

            Framebuffer.Graphics.DrawImage(X, Y, icon);
            WindowManager.font.DrawString(X, (Y + icon.Height), Content, Foreground.Value, (icon.Width + 8), (WindowManager.font.FontSize * 3));
        }
    }
}
