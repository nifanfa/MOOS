using System;
using System.Diagnostics;
using System.Drawing;

namespace Kernel.Misc
{
    internal class IFont
    {
        private readonly Image image;
        private readonly string charset;

        public int Width => image.Width;

        public int Height => image.Height;

        public int FontSize;

        public int NumRow => Width / FontSize;

        public IFont(Image _img, string _charset,int size)
        {
            image = _img;
            charset = _charset;
            FontSize = size;
        }

        public int DrawChar(int X, int Y, char Chr)
        {
            int index = charset.IndexOf(Chr);
            if (index == -1)
            {
                if (Chr == ' ') return FontSize / 2;
                return 0;
            }

            int baseX = 0, baseY = 0;
            for(int i = 0; i <= index; i++)
            {
                if ((i % NumRow) == 0 && i != 0)
                {
                    baseX = 0;
                    baseY += FontSize;
                }
                if (i != index)
                    baseX += FontSize;
            }

            for (int w = 0; w < FontSize; w++)
            {
                int counter = 0;
                for (int h = 0; h < FontSize; h++)
                {
                    uint color = image.GetPixel(baseX + w, baseY + h);
                    if (X != -1 && Y != -1)
                        Framebuffer.ADrawPoint(X + w, Y + h, color);
                    if ((color & 0xFF000000) == 0) counter++;
                }
                if (w > 5 && counter == FontSize) return w;
            }

            return FontSize;
        }

        public int MeasureString(string Str)
        {
            int w = 0;
            for (int i = 0; i < Str.Length; i++)
            {
                w += DrawChar(-1, -1, Str[i]);
            }
            return w;
        }

        public void DrawString(int X, int Y, string Str, int LineLimit = -1)
        {
            int w = 0, h = 0;
            for (int i = 0; i < Str.Length; i++)
            {
                w += DrawChar(X + w, Y + h, Str[i]);
                if (w + FontSize > LineLimit && LineLimit != -1 || Str[i] == '\n')
                {
                    w = 0;
                    h += FontSize;
                }
            }
        }
    }
}
