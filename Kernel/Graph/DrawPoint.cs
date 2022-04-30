using System.Drawing;

namespace Kernel.Graph
{
    public unsafe partial class Graphics
    {
        public virtual void DrawPoint(int X, int Y, uint color, bool alphaBlending = false)
        {
            if (alphaBlending)
            {
                uint foreground = color;
                int fA = (byte)((foreground >> 24) & 0xFF);
                int fR = (byte)((foreground >> 16) & 0xFF);
                int fG = (byte)((foreground >> 8) & 0xFF);
                int fB = (byte)((foreground) & 0xFF);

                uint background = GetPoint(X, Y);
                int bA = (byte)((background >> 24) & 0xFF);
                int bR = (byte)((background >> 16) & 0xFF);
                int bG = (byte)((background >> 8) & 0xFF);
                int bB = (byte)((background) & 0xFF);

                int alpha = fA;
                int inv_alpha = 255 - alpha;

                int newR = (fR * alpha + inv_alpha * bR) >> 8;
                int newG = (fG * alpha + inv_alpha * bG) >> 8;
                int newB = (fB * alpha + inv_alpha * bB) >> 8;

                color = Color.ToArgb((byte)newR, (byte)newG, (byte)newB);
            }

            if (X > 0 && Y > 0 && X < Width && Y < Height)
            {
                Memory[Width * Y + X] = color;
            }
        }

        public void DrawPoint(int X, int Y, uint Color, float Brightness)
        {
            byte A = (byte)((Color >> 24) & 0xFF);
            byte R = (byte)((Color >> 16) & 0xFF);
            byte G = (byte)((Color >> 8) & 0xFF);
            byte B = (byte)((Color) & 0xFF);
            A = ((byte)(A * (1f - Brightness)));
            DrawPoint(X, Y, System.Drawing.Color.ToArgb(A, R, G, B), true);
        }
    }
}
