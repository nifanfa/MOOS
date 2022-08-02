using MOOS;
using MOOS.FS;
using System;
using System.Collections.Generic;
using System.Desktops;
using System.Desktops.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace System.Explorers
{
    public class ExplorerManager : Window
    {
        public string Dir { set; get; }
        public List<IconFile> Files { private set; get; }

        public ExplorerManager()
        {
            Foreground = Brushes.Black;
            Width = 800;
            Height=600;

            Files = new List<IconFile>();

        }

        public override void OnLoaded()
        {
            base.OnLoaded();
            int BarHeight = 5;
            int Devide = 60;
            int X = 5;
            int Y = BarHeight;
            string devider = "/";

            List<FileInfo> files = File.GetFiles(Dir);

            for (int i = 0; i < files.Count; i++)
            {
                if (Y + DesktopIcons.FileIcon.Height + Devide > this.Height - Devide)
                {
                    Y = BarHeight;
                    X += DesktopIcons.FileIcon.Width + (Devide / 2);
                }

                if (files[i].Attribute == FileAttribute.Hidden || files[i].Attribute == FileAttribute.System)
                {
                    continue;
                }

                IconFile icon = new IconFile();
                icon.OwnerWindow = this;
                icon.Content = files[i].Name;
                icon.Foreground = Brushes.Black;
                icon.Path = Dir + devider;
                icon.FilePath = Dir + devider + icon.Content;
                icon.FileInfo = files[i];
                icon.X = X;
                icon.Y = Y;

                if (files[i].Attribute == FileAttribute.Directory)
                {
                    icon.isDirectory = true;
                    icon.Command = DesktopManager.IconDirectoryClickCommand;
                }
                else
                {
                    icon.Command = DesktopManager.IconClickCommand;
                }

                icon.onLoadIconExtention();

                Files.Add(icon);

                Y += DesktopIcons.FileIcon.Height + Devide;
            }

            files.Dispose();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            for (int i = 0; i < Files.Count; i++)
            {
                Files[i].Update();
            }
        }

        public override void OnDraw()
        {
            base.OnDraw();

            for (int i = 0; i < Files.Count; i++)
            {
                Files[i].Draw();
            }
        }

        public override void OnClose()
        {
            base.OnClose();
            Files.Clear();
            this.Dispose();
        }

        void onDirectoryClick(object obj)
        {

        }

        void onFileClick(object obj)
        {

        }

    }
}
