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
                    DrawPoint(Color.FromArgb((uint)image.RawData[h * image.Width + w]),(uint)(X + w), (uint)(Y + h));
                }
            }
        }

        internal void DrawPoint(Color color,uint x, uint y)
        {
            Framebuffer.Graphics.DrawPoint(color, (int)x, (int)y);
        }

        internal void DrawFilledRectangle(Color color,uint x, uint y, uint width, uint height)
        {
            Framebuffer.Graphics.FillRectangle(color,(int)x, (int)y, (int)width, (int)height);
        }

        internal void DrawRectangle(Color color, int x, int y, int width, int height)
        {
            Framebuffer.Graphics.DrawRectangle(color,x, y, width, height);
        }

        internal void DrawLine(Color color, int xStart, int yStart, int xEnd, int yEnd)
        {
            Framebuffer.Graphics.DrawLine(color, xStart, yStart, xEnd, yEnd);
        }

        internal void Clear(Color color)
        {
            Framebuffer.Graphics.Clear(color);
        }
    }
}
