#if HasGUI
using System.Drawing;
using MOOS.Driver;
using MOOS.Graph;
using MOOS.Misc;

namespace MOOS.GUI
{
    internal class Monitor : Window
    {
        public Image img;
        public Graphics g;

        public Monitor(int X, int Y) : base(X, Y, 200, 200)
        {
            Title = "System Monitor";
            img = new Image(Width, Height);
            g = Graphics.FromImage(img);
            g.Clear(Color.FromArgb(0xFF222222));
        }

        private int lastCPUUsage;
        private const int lineWidth = 5;

        public override void OnDraw()
        {
            base.OnDraw();


            if ((Timer.Ticks % 10) == 0)
            {
                int cpuUsage = (int)(100 - ThreadPool.CPUUsage);

                g.FillRectangle(Width - lineWidth, 0, lineWidth, Height, 0xFF222222);
                g.DrawLine(Width - lineWidth, Height / 100 * lastCPUUsage, Width, Height / 100 * cpuUsage, 0xFFFF0000);

                lastCPUUsage = cpuUsage;

                g.CopyFromScreen(-lineWidth, 0, 0, 0, new Size(Width, Height));
            }

            Framebuffer.Graphics.DrawImage(img, X, Y, false);
        }
    }
}
#endif