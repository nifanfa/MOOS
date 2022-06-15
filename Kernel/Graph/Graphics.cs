using System.Drawing;

namespace MOOS.Graph
{
    public unsafe class Graphics
    {
        public uint* VideoMemory;
        public int Width;
        public int Height;

        public Graphics(int width,int height,uint* vm)
        {
            this.Width = width;
            this.Height = height;
            this.VideoMemory = vm;
        }

        public virtual void Update() { }

        public virtual void Clear(uint Color)
        {
            Native.Stosd(VideoMemory, Color, (ulong)(Width * Height));
        }

        public virtual void Copy(int dX, int dY, int sX, int sY, int Width, int Height)
        {
            for (int w = 0; w < Width; w++)
            {
                for (int h = 0; h < Height; h++)
                {
                    DrawPoint(dX + w, dY + h, GetPoint(sX + w, sY + h));
                }
            }
        }

        public virtual void FillRectangle(int X, int Y, int Width, int Height, uint Color)
        {
            for (int w = 0; w < Width; w++)
            {
                for (int h = 0; h < Height; h++)
                {
                    DrawPoint(X + w, Y + h, Color);
                }
            }
        }

        public virtual void AFillRectangle(int X, int Y, int Width, int Height, uint Color)
        {
            for (int w = 0; w < Width; w++)
            {
                for (int h = 0; h < Height; h++)
                {
                    DrawPoint(X + w, Y + h, Color, true);
                }
            }
        }

        public virtual uint GetPoint(int X, int Y)
        {
            if (X > 0 && Y > 0 && X < Width && Y < Height)
            {
                return VideoMemory[Width * Y + X];
            }
            return 0;
        }

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
                VideoMemory[Width * Y + X] = color;
            }
        }

        public virtual void DrawRectangle(int X, int Y, int Width, int Height, uint Color, int Weight = 1)
        {
            FillRectangle(X, Y, Width, Weight, Color);

            FillRectangle(X, Y, Weight, Height, Color);
            FillRectangle(X + (Width - Weight), Y, Weight, Height, Color);

            FillRectangle(X, Y + (Height - Weight), Width, Weight, Color);
        }

        public virtual Image Save()
        {
            Image image = new Image(Width, Height);
            fixed (uint* ptr = image.RawData)
            {
                Native.Movsd(ptr, VideoMemory, (ulong)(Width * Height));
            }
            return image;
        }

        public virtual void ADrawImage(int X, int Y, Image image, byte alpha)
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

        public virtual void DrawImage(int X, int Y, Image image, bool AlphaBlending = true)
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

        #region Xiaolin Wu's line algorithm
        // swaps two numbers
        void Swap(int* a, int* b)
        {
            int temp = *a;
            *a = *b;
            *b = temp;
        }

        // returns absolute value of number
        float Absolute(float x)
        {
            if (x < 0) return -x;
            else return x;
        }

        //returns integer part of a floating point number
        int IPartOfNumber(float x)
        {
            return (int)x;
        }

        //rounds off a number
        int RoundNumber(float x)
        {
            return IPartOfNumber(x + 0.5f);
        }

        //returns fractional part of a number
        float FPartOfNumber(float x)
        {
            if (x > 0) return x - IPartOfNumber(x);
            else return x - (IPartOfNumber(x) + 1);

        }

        //returns 1 - fractional part of number
        float RFPartOfNumber(float x)
        {
            return 1 - FPartOfNumber(x);
        }

        // draws a pixel on screen of given brightness
        // 0<=brightness<=1. We can use your own library
        // to draw on screen
        public virtual void DrawPoint(int X, int Y, uint Color, float Brightness)
        {
            byte A = (byte)((Color >> 24) & 0xFF);
            byte R = (byte)((Color >> 16) & 0xFF);
            byte G = (byte)((Color >> 8) & 0xFF);
            byte B = (byte)((Color) & 0xFF);
            A = ((byte)(A * (1f - Brightness)));
            DrawPoint(X, Y, System.Drawing.Color.ToArgb(A, R, G, B), true);
        }

        public virtual void DrawLine(int x0, int y0, int x1, int y1, uint color)
        {
            bool steep = Absolute(y1 - y0) > Absolute(x1 - x0);

            // swap the co-ordinates if slope > 1 or we
            // draw backwards
            if (steep)
            {
                Swap(&x0, &y0);
                Swap(&x1, &y1);
            }
            if (x0 > x1)
            {
                Swap(&x0, &x1);
                Swap(&y0, &y1);
            }

            //compute the slope
            float dx = x1 - x0;
            float dy = y1 - y0;
            float gradient = dy / dx;
            if (dx == 0.0)
                gradient = 1;

            int xpxl1 = x0;
            int xpxl2 = x1;
            float intersectY = y0;

            // main loop
            if (steep)
            {
                int x;
                for (x = xpxl1; x <= xpxl2; x++)
                {
                    // pixel coverage is determined by fractional
                    // part of y co-ordinate
                    DrawPoint(IPartOfNumber(intersectY), x, color,
                                RFPartOfNumber(intersectY));
                    DrawPoint(IPartOfNumber(intersectY) - 1, x, color,
                                FPartOfNumber(intersectY));
                    intersectY += gradient;
                }
            }
            else
            {
                int x;
                for (x = xpxl1; x <= xpxl2; x++)
                {
                    // pixel coverage is determined by fractional
                    // part of y co-ordinate
                    DrawPoint(x, IPartOfNumber(intersectY), color,
                                RFPartOfNumber(intersectY));
                    DrawPoint(x, IPartOfNumber(intersectY) - 1, color,
                                  FPartOfNumber(intersectY));
                    intersectY += gradient;
                }
            }

        }
        #endregion
    }
}