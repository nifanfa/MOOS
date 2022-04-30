using System.Drawing;

namespace Kernel.Graph
{
    public unsafe partial class Graphics
    {
        public virtual void DrawImage(int X, int Y, Image image, byte alpha)
        {
            for (int h = 0; h < image.Height; h++)
                for (int w = 0; w < image.Width; w++)
                {
                    uint foreground = image.RawData[image.Width * h + w];
                    foreground &= ~0xFF000000;
                    foreground |= (uint)alpha << 24;
                    int fA = (byte)((foreground >> 24) & 0xFF);

                    if (fA != 0)
                    {
                        DrawPoint(X + w, Y + h, foreground, true);
                    }
                }
        }

        public void DrawImage(int X, int Y, Image image, bool AlphaBlending = true)
        {
            for (int h = 0; h < image.Height; h++)
                for (int w = 0; w < image.Width; w++)
                {
                    if (AlphaBlending)
                    {

                        uint foreground = image.RawData[image.Width * h + w];
                        int fA = (byte)((foreground >> 24) & 0xFF);

                        if (fA != 0)
                        {
                            DrawPoint(X + w, Y + h, foreground, true);
                        }
                    }
                    else
                    {
                        DrawPoint(X + w, Y + h, image.RawData[image.Width * h + w]);
                    }
                }
        }
    }
}
