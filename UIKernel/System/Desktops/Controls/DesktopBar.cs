using MOOS;
using System;
using System.Collections.Generic;
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

            Framebuffer.Graphics.AFillRectangle(X, Y, Width, Height, Background.Value);
            Framebuffer.Graphics.AFillRectangle(X, Y + Height, Width, 2, backgroundLine.Value);
        }
    }
}
