using MOOS;
using MOOS.Driver;
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
        int _clickCount = 0;
        bool _prevClick;
        ulong _timer = 0;

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
            string ext = Content.ToLower();

            if (ext.EndsWith(".png"))
            {
                icon = DesktopIcons.ImageIcon;
            }
            else if (ext.EndsWith("doom1.wad"))
            {
                icon = DesktopIcons.DoomIcon;
            }
            else if (ext.EndsWith(".exe"))
            {
                icon = DesktopIcons.AppIcon;
            }
            else if (ext.EndsWith(".wav"))
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
            ext.Dispose();
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
                        if (!_clicked && !_prevClick)
                        {
                            _prevClick = true;

                            if (_clickCount > 1) //Double Click
                            {
                                _clicked = true;
                                _clickCount = 0;

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

                if (Control.MouseButtons == MouseButtons.None && _prevClick)
                {
                    _prevClick = false;
                    _clicked = false;
                    _clickCount++;
                    _timer = (Timer.Ticks + 500); //500ms
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

            if (_clickCount > 0)
            {
                if (Timer.Ticks > _timer)
                {
                    _clicked = false;
                    _prevClick = false;
                    _clickCount = 0;
                    _timer = 0;
                }

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
