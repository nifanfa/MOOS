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

        public override void Update()
        {
            base.Update();

            //TO-DO...
        }

        public override void Draw()
        {
            base.Draw();

            Framebuffer.Graphics.DrawImage(this.X, this.Y, ScreenBuf, false);
        }
    }
}
#endif