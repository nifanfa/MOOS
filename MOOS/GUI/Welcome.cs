#if HasGUI
using MOOS.FS;
using MOOS.Misc;
using System.Drawing;

namespace MOOS.GUI
{
    internal class Welcome : Window
    {
        public Image img;

        public Welcome(int X, int Y) : base(X, Y, 280, 225)
        {
#if Chinese
            this.Title = "欢迎";
#else
            this.Title = "Welcome";
#endif
            img = new PNG(File.ReadAllBytes("Images/Banner.png"));
        }

        public override void OnDraw()
        {
            base.OnDraw();
            Framebuffer.Graphics.DrawImage(X, Y, img);
#if Chinese
            WindowManager.font.DrawString(X, Y + img.Height, "欢迎使用Moos(原Mosa)操作系统!\n这个项目的目标是实现一款麻雀虽小但五脏俱全的操作系统.\n源码: https://github.com/nifanfa/Moos!", Width);
#else
            WindowManager.font.DrawString(X, Y + img.Height, "Welcome to Moos!\nThis project is aim to show how to make asimple but powerful operating system.\nCheck out: https://github.com/nifanfa/Moos!", Width);
#endif
        }
    }
}
#endif