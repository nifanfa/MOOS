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

        public int DrawChar(int X, int Y, char Chr)
        {
            int index = charset.IndexOf(Chr);
            if (index == -1)
            {
                if (Chr == ' ') return Width / 2;
                return 0;
            }

            int width = 0;
            int counter = 0;
            int basey = index * (image.Height / charset.Length);
            for (int w = 0; w < image.Width; w++)
            {
                for (int h = basey; h < (basey) + (image.Height / charset.Length); h++)
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
                        if (X >= 0 && Y >= 0)
                        {
                            Framebuffer.DrawPoint(X + w, Y + h - basey, Color.ToArgb((byte)newR, (byte)newG, (byte)newB));
                        }
                    }
                    else
                    {
                        counter++;
                        if (counter == Height && w > 5) return width;
                    }
                }
                counter = 0;
                width++;
            }
            return width;
        }

        public int MeasureString(string Str) 
        {
            int width = 0;
            for (int i = 0; i < Str.Length; i++)
            {
                width+=DrawChar(-1, -1, Str[i]);
            }
            return width;
        }

        public void DrawString(int X, int Y, string Str,int LineLimit = -1)
        {
            int w = 0, h = 0;
            for (int i = 0; i < Str.Length; i++)
            {
                w += DrawChar(X + w, Y + h, Str[i]);
                if (w + Width > LineLimit && LineLimit != -1) 
                {
                    w = 0;
                    h += Height;
                }
            }
        }
    }
}
