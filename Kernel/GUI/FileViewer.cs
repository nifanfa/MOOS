#if HasGUI
using MOOS.FS;
using System.Drawing;
using System.Text;
using System.Windows;

namespace MOOS.GUI
{
	internal unsafe class FileViewer : Window
	{
		public string FileContents;
		public unsafe FileViewer(int X, int Y, string path) : base(X, Y, 500, 500)
		{
			Title = "FileViewer";
			byte[] data = File.ReadAllBytes(path);
			fixed (byte* dataPtr = data)
			{
				FileContents = Encoding.ASCII.GetString(data);
			}
			data.Dispose();
		}

		public override void OnDraw()
		{
			base.OnDraw();
			WindowManager.font.DrawString(X, Y, FileContents, 0xFF000000, Width);
		}
	}
}
#endif