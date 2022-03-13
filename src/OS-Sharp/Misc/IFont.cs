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
                    uint bg = Framebuffer.GetPoint(X + w, Y + h - basey);
                    uint foreground = image.RawData[image.Width * h + w];
                    uint FontAlpha = foreground & 0xFF000000 >> 24;
                    byte R = (byte)((((((byte)((foreground >> 16) & 0xFF)) * FontAlpha) + ((255 - FontAlpha) * ((bg&0x00FF0000)>>16))) >> 8) & 0xFF);
                    byte G = (byte)((((((byte)((foreground >> 8) & 0xFF)) * FontAlpha) + ((255 - FontAlpha) * ((bg & 0x0000FF00) >> 8))) >> 8) & 0xFF);
                    byte B = (byte)((((((byte)((foreground) & 0xFF)) * FontAlpha) + ((255 - FontAlpha) * ((bg & 0x000000FF) >> 0))) >> 8) & 0xFF);
                    
                    if ((foreground & 0xFF000000 >> 24) != 0)
                    {
                        if (X >= 0 && Y >= 0)
                        {
                            Framebuffer.DrawPoint(X + w, Y + h - basey, Color.ToArgb(R, G, B));
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
                if (w + Width > LineLimit && LineLimit != -1 || Str[i] == '\n') 
                {
                    w = 0;
                    h += Height;
                }
            }
        }
    }
}
