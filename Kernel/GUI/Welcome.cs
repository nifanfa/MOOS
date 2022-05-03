﻿using Kernel.FS;
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
            this.Title = "Welcome";
            img = new PNG(File.Instance.ReadAllBytes("Images/Banner.png"));
        }

        public override void OnDraw()
        {
            base.OnDraw();
            Framebuffer.Graphics.DrawImage(X, Y, img);
            font.DrawString(X, Y + img.Height, "(Banner by andreweathan)\nWelcome to Moos!\nThis project is aim to show how to make asimple but powerful operating system.\nCheck out https://github.com/nifanfa/Moos!\nContributors: nifanfa, Elijah629, devrusty, TRDP1404", Width);
        }
    }
}
