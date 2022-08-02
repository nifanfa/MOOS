#if HasGUI
using System.Drawing;
using System.Windows;
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

				g.FillRectangle(Color.FromArgb(0xFF222222), Width - lineWidth, 0, lineWidth, Height);
				g.DrawLine(Color.FromArgb(0xFFFF0000), Width - lineWidth, Height / 100 * lastCPUUsage, Width, Height / 100 * cpuUsage);

				lastCPUUsage = cpuUsage;

				g.CopyFromScreen(0, 0,-lineWidth, 0, new Size(Width, Height));
			}

			Framebuffer.Graphics.DrawImage(img, X, Y, false);
		}
	}
}
#endif