using System.Drawing;
using MOOS;

namespace Cosmos.System.Graphics
{
    public unsafe class Canvas
    {
        public int Width => Framebuffer.Width;
        public int Height => Framebuffer.Height;
        public Canvas()
        {
            Framebuffer.TripleBuffered = true;
        }
        public void Update()
        {
            Framebuffer.Update();
        }
        public void FillRectangle(Color color, int X, int Y, int Width, int Height, bool alpha = false)
        {
            Framebuffer.Graphics.FillRectangle(color, X, Y, Width, Height, alpha);
        }
        public void DrawRectangle(Color color, int X, int Y, int Width, int Height)
        {
            Framebuffer.Graphics.FillRectangle(color, X, Y, Width, Height);
        }
        public void DrawPoint(Color color, int X, int Y)
        {
            Framebuffer.Graphics.DrawPoint(color, X, Y);
        }
        public void Clear(Color color)
        {
            Framebuffer.Graphics.Clear(color);
        }
        public void DrawImage(Image image, int X, int Y)
        {
            Framebuffer.Graphics.DrawImage(image, X, Y);
        }
    }
}
