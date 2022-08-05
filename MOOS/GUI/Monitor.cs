#if HasGUI
using MOOS.Driver;
using MOOS.Graph;
using MOOS.Misc;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MOOS.GUI
{
    internal class Monitor : Window
    {
        public Image img;
        public Graphics g;

        public Monitor(int X, int Y) : base(X, Y, 200, 200)
        {
            this.Title = "System Monitor";
            img = new Image(this.Width, this.Height);
            g = Graphics.FromImage(img);
            g.Clear(0xFF222222);
        }

        int lastCPUUsage;

        const int lineWidth = 5;

        public override void OnDraw()
        {
            int w = Width;

            Width = w + 1;
            base.OnDraw();
            Width = w;


            if((Timer.Ticks % 10) == 0)
            {
                int cpuUsage = (int)(100 -ThreadPool.CPUUsage);

                g.FillRectangle(Width - lineWidth, 0, lineWidth, Height, 0xFF222222);
                g.DrawLine(Width - lineWidth, (Height / 100) * lastCPUUsage, Width, (Height / 100) * cpuUsage, 0xFFFF0000);

                lastCPUUsage = cpuUsage;

                g.Copy(-lineWidth, 0, 0, 0, Width, Height);
            }

            Framebuffer.Graphics.DrawImage(X, Y, img, true);
        }
    }
}
#endif