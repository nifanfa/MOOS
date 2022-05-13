using Kernel.FS;
using Kernel.Misc;
using System.Drawing;

namespace Kernel.GUI
{
    internal class Welcome : Window
    {
        public Image img;

        public Welcome(int X, int Y) : base(X, Y, 280, 280)
        {
            BackgroundColor = 0xFF222222;
#if Chinese
            this.Title = "欢迎";
#else
            this.Title = "Welcome";
#endif
            img = new PNG(File.Instance.ReadAllBytes("Images/Banner.png"));
        }

        public override void OnDraw()
        {
            base.OnDraw();
            Framebuffer.Graphics.DrawImage(X, Y, img);
#if Chinese
            font.DrawString(X, Y + img.Height, "(由andreweathan设计的横幅)\n欢迎使用Moos操作系统!\n这个项目的目标是实现一款麻雀虽小但五脏俱全的操作系统.\n源码 https://github.com/nifanfa/Moos!\n贡献者: nifanfa, Elijah629, devrusty, TRDP1404", Width);
#else
            font.DrawString(X, Y + img.Height, "(Banner by andreweathan)\nWelcome to Moos!\nThis project is aim to show how to make asimple but powerful operating system.\nCheck out https://github.com/nifanfa/Moos!\nContributors: nifanfa, Elijah629, devrusty, TRDP1404", Width);
#endif
        }
    }
}
