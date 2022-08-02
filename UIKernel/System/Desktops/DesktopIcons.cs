﻿using MOOS.FS;
using MOOS.Misc;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace System.Desktops
{
    public static class DesktopIcons
    {
        public static Image AppIcon { set; get; }
        public static PNG AudioIcon { get; set; }
        public static Image BuiltInAppIcon { set; get; }
        public static PNG FolderIcon { get; set; }
        public static PNG DoomIcon { get; set; }
        public static Image AppTerminal { set; get; }
        public static Image FileIcon { get;  set; }
        public static PNG ImageIcon { get; set; }
        public static PNG GameIcon { get; set; }

        public static void Initialize()
        {
            FileIcon = new PNG(File.ReadAllBytes("Images/file.png"));
            ImageIcon = new PNG(File.ReadAllBytes("Images/Image.png"));
            GameIcon = new PNG(File.ReadAllBytes("Images/Game.png"));
            AppIcon = new PNG(File.Instance.ReadAllBytes("Images/App.png"));
            AudioIcon = new PNG(File.ReadAllBytes("Images/Audio.png"));
            BuiltInAppIcon = new PNG(File.Instance.ReadAllBytes("Images/BApp.png"));
            FolderIcon = new PNG(File.ReadAllBytes("Images/folder.png"));
            DoomIcon = new PNG(File.ReadAllBytes("Images/Doom1.png"));
            AppTerminal = new PNG(File.Instance.ReadAllBytes("Images/Terminal.png"));
        }
    }
}