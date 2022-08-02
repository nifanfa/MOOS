using MOOS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media;

namespace System.Desktops.Controls
{
    public class DesktopBar : DesktopControl
    {
        Brush backgroundLine;
        public DesktopBar()
        {
            X = 0;
            Y = 0;
            Width = Framebuffer.Width;
            Height = 30;
            Background = ColorConverter.FromARGB(225, 255, 255, 255);
            backgroundLine = ColorConverter.FromARGB(250, 222, 222, 222);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            base.Draw();

            Framebuffer.Graphics.FillRectangle(Color.FromArgb(Background.Value), X, Y, Width, Height, true);
            Framebuffer.Graphics.FillRectangle(Color.FromArgb(backgroundLine.Value), X, Y + Height, Width, 2, true);
        }
    }
}
