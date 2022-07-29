using MOOS;
using System.Drawing;

namespace Cosmos.System.Graphics
{
    public class Canvas
    {
        public Canvas()
        {
            Framebuffer.TripleBuffered = true;
        }

        public int Width => Framebuffer.Graphics.Width;
        public int Height => Framebuffer.Graphics.Height;

        internal void Update()
        {
            Framebuffer.Update();
        }

        internal unsafe void DrawImage(Image image, int X, int Y)
        {
            for(int w = 0; w < image.Width; w++) 
            {
                for(int h = 0; h < image.Height; h++) 
                {
                    DrawPoint((uint)(X + w), (uint)(Y + h), (uint)image.RawData[h * image.Width + w]);
                }
            }
        }

        internal void DrawPoint(uint v1, uint v2, uint v3)
        {
            Framebuffer.Graphics.DrawPoint((int)v1, (int)v2, v3);
        }

        internal void DrawFillRectangle(uint x, uint y, uint width, uint height, uint v)
        {
            Framebuffer.Graphics.FillRectangle((int)x, (int)y, (int)width, (int)height, v);
        }

        internal void DrawRectangle(uint v, int x, int y, int width, int height)
        {
            Framebuffer.Graphics.DrawRectangle(x, y, width, height, v);
        }

        internal void DrawLine(uint color, int xStart, int yStart, int xEnd, int yEnd)
        {
            Framebuffer.Graphics.DrawLine(xStart, yStart, xEnd, yEnd, color);
        }

        internal void Clear(uint v)
        {
            Framebuffer.Graphics.Clear(v);
        }
    }
}
