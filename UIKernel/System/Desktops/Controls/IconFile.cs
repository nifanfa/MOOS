using MOOS;
using MOOS.Driver;
using MOOS.FS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Explorers;
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
        public ExplorerManager OwnerWindow { get;  set; }

        bool _isFocus;
        int offsetX, offsetY;
        int _clickCount;
        ulong _timer;
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
            else if (ext.EndsWith(".nes"))
            {
                icon = DesktopIcons.GameIcon;
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
            int _x = X;
            int _y = Y;

            if (OwnerWindow != null)
            {
                _x = OwnerWindow.X + X;
                _y = OwnerWindow.Y + Y;
            }

            if (!WindowManager.HasWindowMoving && Control.MousePosition.X > (_x - offsetX) && Control.MousePosition.X < ((_x - offsetX) + Width) && Control.MousePosition.Y > _y && Control.MousePosition.Y < (_y + Height))
            {
                _isFocus = true;

                if (Control.Clicked)
                {
                    if (Command != null)
                    {
                        if (_clickCount >= 1) //Double Click
                        {
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

                    _clickCount++;
                    _timer = Timer.Ticks + 500; //500ms
                }

            }
            else
            {
                _isFocus = false;
                _clickCount = 0;
            }

            if (_clickCount > 0)
            {
                if (Timer.Ticks > _timer)
                {
                    _clickCount = 0;
                }
            }
        }

        public override void Draw()
        {
            base.Draw();

            int _x = X;
            int _y = Y;

            if (OwnerWindow != null)
            {
                _x = OwnerWindow.X + X;
                _y = OwnerWindow.Y + Y;
            }

            if (_isFocus)
            {
                Framebuffer.Graphics.FillRectangle(Color.FromArgb(FocusBackground.Value), (_x- offsetX), (_y - offsetY), (Width + (offsetX*2)), (Height + ((WindowManager.font.FontSize * 3) + offsetX)));
            }

            Framebuffer.Graphics.DrawImage(icon, _x, _y, true);
            WindowManager.font.DrawString(_x, (_y + icon.Height), Content, Foreground.Value, (icon.Width + 8), (WindowManager.font.FontSize * 3));
        }
    }
}
