#if Kernel && HasGUI
using System.Drawing;
using System.Windows;

namespace MOOS.GUI
{
    internal class PortableApp : Window
    {
        public Image ScreenBuf;

        public PortableApp(int X, int Y, int Width, int Height) : base(X, Y, Width, Height)
        {
            ScreenBuf = new Image(Width, Height);
        }

        public override void OnInput()
        {
            if (!IsFocus) return;
            base.OnInput();

            //TO-DO...
        }

        public override void OnDraw()
        {
            base.OnDraw();

            Framebuffer.Graphics.DrawImage(ScreenBuf, X, Y);
        }
    }
}
#endif