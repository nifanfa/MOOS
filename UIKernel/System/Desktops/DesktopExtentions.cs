using MOOS.FS;
using MOOS.Misc;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace System.Desktops
{
    public static class DesktopExtentions
    {
        public static Image AppIcon { set; get; }
        public static Image BuiltInAppIcon { set; get; }
        public static Image AppTerminal { set; get; }
        public static void Initialize()
        {
            AppIcon = new PNG(File.Instance.ReadAllBytes("Images/App.png"));
            BuiltInAppIcon = new PNG(File.Instance.ReadAllBytes("Images/BApp.png"));
            AppTerminal = new PNG(File.Instance.ReadAllBytes("Images/Terminal.png"));
        }
    }
}
