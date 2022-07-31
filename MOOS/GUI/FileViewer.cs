#if HasGUI
using MOOS.FS;

namespace MOOS.GUI
{
    internal unsafe class FileViewer : Window
    {
        public string FileContents;
        public unsafe FileViewer(int X, int Y, string path) : base(X, Y, 500, 500)
        {
            Title = "FileViewer";
            byte[] data = File.Instance.ReadAllBytes(path);
            fixed (byte* dataPtr = data)
            {
                FileContents = string.FromASCII((System.IntPtr)dataPtr, data.Length);
            }
            data.Dispose();
        }

        public override void OnDraw()
        {
            base.OnDraw();
            WindowManager.font.DrawString(X, Y, FileContents, Width);
        }
    }
}
#endif