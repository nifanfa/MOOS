using System;
using System.Windows.Forms;

namespace Cosmos.System
{
    [Flags]
    public enum MouseState
    {
        Left = 0x100000,
        None = 0x0,
        Right = 0x200000,
        Middle = 0x400000,
        XButton1 = 0x800000,
        XButton2 = 0x1000000
    }

    internal class MouseManager
    {
        public static int X
        {
            get => Control.MousePosition.X;
            set { }
        }

        public static int Y
        {

            get => Control.MousePosition.Y;
            set { }
        }

        public static int ScreenWidth
        {
            get{ return MOOS.Framebuffer.Width; }
            set { }
        }

        public static int ScreenHeight
        {
            get { return MOOS.Framebuffer.Height; }
            set { }
        }

        public static MouseState MouseState
        {
            get => (MouseState)Control.MouseButtons;
        }
    }
}
