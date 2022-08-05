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
        class Chart
        {
            public Image image;
            public Graphics graphics;
            public int lastValue;
            public string name;

            public Chart(int Width,int Height,string Name)
            {
                image = new Image(Width, Height);
                graphics = Graphics.FromImage(image);
                lastValue = 100;
                name = Name;
            }
        }

        Chart CPUUsage;
        Chart RAMUsage;

        public Monitor(int X, int Y) : base(X, Y, 200-1, 120)
        {
            this.Title = "System Monitor";

            CPUUsage = new Chart(100, 100,"CPU");
            RAMUsage = new Chart(100, 100,"RAM");
        }

        const int LineWidth = 5;

        public override void OnDraw()
        {
            base.OnDraw();

            if ((Timer.Ticks % 10) == 0)
            {
                DrawLineChart((int)ThreadPool.CPUUsage, ref CPUUsage.lastValue, CPUUsage.graphics, 0xFF5DADE2);
                DrawLineChart((int)(Allocator.MemoryInUse * 100 / Allocator.MemorySize), ref RAMUsage.lastValue, RAMUsage.graphics, 0xFF58D68D);
            }

            int aX = 0;
            Render(ref aX,CPUUsage);
            Render(ref aX,RAMUsage);

            base.DrawBorder();
        }

        private void Render(ref int aX,Chart chart)
        {
            WindowManager.font.DrawString(X + aX + (chart.graphics.Width / 2) - (WindowManager.font.MeasureString(chart.name) / 2), Y, chart.name);
            Framebuffer.Graphics.DrawImage(X + aX, Y + this.Height - chart.graphics.Height, chart.image, true);
            Framebuffer.Graphics.DrawRectangle(X + aX, Y, chart.graphics.Width, this.Height, 0xFF333333);
            aX += chart.graphics.Width;
            aX -= 1;
        }

        private void DrawLineChart(int value,ref int lastValue, Graphics graphics,uint Color)
        {
            int val = (100 - value);

            graphics.FillRectangle(graphics.Width - LineWidth, 0, LineWidth, graphics.Height, 0xFF222222);
            graphics.DrawLine(graphics.Width - LineWidth, (graphics.Height / 100) * lastValue, graphics.Width, (graphics.Height / 100) * val, Color);

            lastValue = val;

            graphics.Copy(-LineWidth, 0, 0, 0, graphics.Width, graphics.Height);
        }
    }
}
#endif