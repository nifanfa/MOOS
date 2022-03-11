// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence
using System.Drawing;

namespace OS_Sharp.Misc
{
    internal class IFont
    {
        private readonly Image image;
        private readonly string charset;

        public int Width => image.Width;

        public int Height => image.Height / charset.Length;

        public IFont(Image _img, string _charset)
        {
            image = _img;
            charset = _charset;
        }

        public void DrawChar(int X, int Y, char Chr)
        {
            int index = charset.IndexOf(Chr);
            if (index == -1)
            {
                return;
            }

            int basey = index * (image.Height / charset.Length);
            for (int h = basey; h < (basey) + (image.Height / charset.Length); h++)
            {
                for (int w = 0; w < image.Width; w++)
                {
                    uint foreground = image.RawData[image.Width * h + w];
                    int fA = (byte)((foreground >> 24) & 0xFF);
                    int fR = (byte)((foreground >> 16) & 0xFF);
                    int fG = (byte)((foreground >> 8) & 0xFF);
                    int fB = (byte)((foreground) & 0xFF);

                    uint background = Framebuffer.GetPoint(X + w, Y + h - basey);
                    int bA = (byte)((background >> 24) & 0xFF);
                    int bR = (byte)((background >> 16) & 0xFF);
                    int bG = (byte)((background >> 8) & 0xFF);
                    int bB = (byte)((background) & 0xFF);

                    int alpha = fA;
                    int inv_alpha = 255 - alpha;

                    int newR = (fR * alpha + inv_alpha * bR) >> 8;
                    int newG = (fG * alpha + inv_alpha * bG) >> 8;
                    int newB = (fB * alpha + inv_alpha * bB) >> 8;

                    if (fA != 0)
                    {
                        Framebuffer.DrawPoint(X + w, Y + h - basey, Color.ToArgb((byte)newR, (byte)newG, (byte)newB));
                    }
                }
            }
        }

        public void DrawString(int X, int Y, string Str)
        {
            for (int i = 0; i < Str.Length; i++)
            {
                DrawChar(X + (i * Width), Y, Str[i]);
            }
        }
    }
}
