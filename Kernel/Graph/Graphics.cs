using System;
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

        /*
        Copyright © 2018-2022 SMNX & contributors
        Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
        The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
        THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
         */
        ushort[] stackblur_mul = {
            512, 512, 456, 512, 328, 456, 335, 512, 405, 328, 271, 456, 388, 335, 292, 512,
            454, 405, 364, 328, 298, 271, 496, 456, 420, 388, 360, 335, 312, 292, 273, 512,
            482, 454, 428, 405, 383, 364, 345, 328, 312, 298, 284, 271, 259, 496, 475, 456,
            437, 420, 404, 388, 374, 360, 347, 335, 323, 312, 302, 292, 282, 273, 265, 512,
            497, 482, 468, 454, 441, 428, 417, 405, 394, 383, 373, 364, 354, 345, 337, 328,
            320, 312, 305, 298, 291, 284, 278, 271, 265, 259, 507, 496, 485, 475, 465, 456,
            446, 437, 428, 420, 412, 404, 396, 388, 381, 374, 367, 360, 354, 347, 341, 335,
            329, 323, 318, 312, 307, 302, 297, 292, 287, 282, 278, 273, 269, 265, 261, 512,
            505, 497, 489, 482, 475, 468, 461, 454, 447, 441, 435, 428, 422, 417, 411, 405,
            399, 394, 389, 383, 378, 373, 368, 364, 359, 354, 350, 345, 341, 337, 332, 328,
            324, 320, 316, 312, 309, 305, 301, 298, 294, 291, 287, 284, 281, 278, 274, 271,
            268, 265, 262, 259, 257, 507, 501, 496, 491, 485, 480, 475, 470, 465, 460, 456,
            451, 446, 442, 437, 433, 428, 424, 420, 416, 412, 408, 404, 400, 396, 392, 388,
            385, 381, 377, 374, 370, 367, 363, 360, 357, 354, 350, 347, 344, 341, 338, 335,
            332, 329, 326, 323, 320, 318, 315, 312, 310, 307, 304, 302, 299, 297, 294, 292,
            289, 287, 285, 282, 280, 278, 275, 273, 271, 269, 267, 265, 263, 261, 259};
        byte[] stackblur_shr = {
            9, 11, 12, 13, 13, 14, 14, 15, 15, 15, 15, 16, 16, 16, 16, 17,
            17, 17, 17, 17, 17, 17, 18, 18, 18, 18, 18, 18, 18, 18, 18, 19,
            19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 20, 20, 20,
            20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 21,
            21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21,
            21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 22, 22, 22, 22, 22, 22,
            22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22,
            22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 23,
            23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23,
            23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23,
            23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23,
            23, 23, 23, 23, 23, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
            24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
            24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
            24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
            24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24};
        public byte[] blur_stack = new byte[((255 * 2u) + 1) * 3];
        public void Blur(
            uint X,
            uint Y,
            uint Width,
            uint Height,
            byte Radius)
        {
            byte* src = (byte*)VideoMemory;
            uint w = (uint)this.Width;
            uint h = (uint)this.Height;

            Width += X;
            Height += Y;

            uint x, y, xp, yp, i;
            uint sp;
            uint stack_start;
            byte* stack_ptr;

            byte* src_ptr;
            byte* dst_ptr;

            ulong sum_r;
            ulong sum_g;
            ulong sum_b;
            ulong sum_in_r;
            ulong sum_in_g;
            ulong sum_in_b;
            ulong sum_out_r;
            ulong sum_out_g;
            ulong sum_out_b;

            uint wm = Width - X - 1;
            uint hm = Height - Y - 1;
            uint w4 = w * 4;
            uint div = (Radius * 2u) + 1;
            uint mul_sum = stackblur_mul[Radius];
            byte shr_sum = stackblur_shr[Radius];

            fixed(byte* p = blur_stack)
            {
                {
                    for (y = Y; y < Height; y++)
                    {
                        sum_r = sum_g = sum_b =
                            sum_in_r = sum_in_g = sum_in_b =
                                sum_out_r = sum_out_g = sum_out_b = 0;

                        src_ptr = src + w4 * y + (X * 4); // start of line (0,y)

                        for (i = 0; i <= Radius; i++)
                        {
                            stack_ptr = &p[3 * i];
                            stack_ptr[0] = src_ptr[0];
                            stack_ptr[1] = src_ptr[1];
                            stack_ptr[2] = src_ptr[2];
                            sum_r += src_ptr[0] * (i + 1);
                            sum_g += src_ptr[1] * (i + 1);
                            sum_b += src_ptr[2] * (i + 1);
                            sum_out_r += src_ptr[0];
                            sum_out_g += src_ptr[1];
                            sum_out_b += src_ptr[2];
                        }

                        for (i = 1; i <= Radius; i++)
                        {
                            if (i <= wm)
                                src_ptr += 4;
                            stack_ptr = &p[3 * (i + Radius)];
                            stack_ptr[0] = src_ptr[0];
                            stack_ptr[1] = src_ptr[1];
                            stack_ptr[2] = src_ptr[2];
                            sum_r += src_ptr[0] * (Radius + 1u - i);
                            sum_g += src_ptr[1] * (Radius + 1u - i);
                            sum_b += src_ptr[2] * (Radius + 1u - i);
                            sum_in_r += src_ptr[0];
                            sum_in_g += src_ptr[1];
                            sum_in_b += src_ptr[2];
                        }

                        sp = Radius;
                        xp = Radius;

                        if (xp > wm)
                        {
                            xp = wm;
                        }

                        src_ptr = src + 4 * (xp + y * w) + (X * 4);
                        dst_ptr = src + y * w4 + (X * 4);
                        for (x = X; x < Width; x++)
                        {
                            uint alpha = dst_ptr[3];
                            dst_ptr[0] = (byte)Math.Clamp((long)(sum_r * mul_sum) >> shr_sum, 0, (long)alpha);
                            dst_ptr[1] = (byte)Math.Clamp((long)(sum_g * mul_sum) >> shr_sum, 0, (long)alpha);
                            dst_ptr[2] = (byte)Math.Clamp((long)(sum_b * mul_sum) >> shr_sum, 0, (long)alpha);
                            dst_ptr += 4;

                            sum_r -= sum_out_r;
                            sum_g -= sum_out_g;
                            sum_b -= sum_out_b;

                            stack_start = sp + div - Radius;
                            if (stack_start >= div)
                                stack_start -= div;
                            stack_ptr = &p[3 * stack_start];

                            sum_out_r -= stack_ptr[0];
                            sum_out_g -= stack_ptr[1];
                            sum_out_b -= stack_ptr[2];

                            if (xp < wm)
                            {
                                src_ptr += 4;
                                ++xp;
                            }

                            stack_ptr[0] = src_ptr[0];
                            stack_ptr[1] = src_ptr[1];
                            stack_ptr[2] = src_ptr[2];

                            sum_in_r += src_ptr[0];
                            sum_in_g += src_ptr[1];
                            sum_in_b += src_ptr[2];
                            sum_r += sum_in_r;
                            sum_g += sum_in_g;
                            sum_b += sum_in_b;

                            ++sp;
                            if (sp >= div)
                                sp = 0;
                            stack_ptr = &p[sp * 3];

                            sum_out_r += stack_ptr[0];
                            sum_out_g += stack_ptr[1];
                            sum_out_b += stack_ptr[2];
                            sum_in_r -= stack_ptr[0];
                            sum_in_g -= stack_ptr[1];
                            sum_in_b -= stack_ptr[2];
                        }
                    }
                }

                {
                    for (x = X; x < Width; x++)
                    {
                        sum_r = sum_g = sum_b =
                            sum_in_r = sum_in_g = sum_in_b =
                                sum_out_r = sum_out_g = sum_out_b = 0;

                        src_ptr = src + 4 * x + Y * w4; // x,0
                        for (i = 0; i <= Radius; i++)
                        {
                            stack_ptr = &p[i * 3];
                            stack_ptr[0] = src_ptr[0];
                            stack_ptr[1] = src_ptr[1];
                            stack_ptr[2] = src_ptr[2];
                            sum_r += src_ptr[0] * (i + 1);
                            sum_g += src_ptr[1] * (i + 1);
                            sum_b += src_ptr[2] * (i + 1);
                            sum_out_r += src_ptr[0];
                            sum_out_g += src_ptr[1];
                            sum_out_b += src_ptr[2];
                        }
                        for (i = 1; i <= Radius; i++)
                        {
                            if (i <= hm)
                                src_ptr += w4; // +stride

                            stack_ptr = &p[3 * (i + Radius)];
                            stack_ptr[0] = src_ptr[0];
                            stack_ptr[1] = src_ptr[1];
                            stack_ptr[2] = src_ptr[2];
                            sum_r += src_ptr[0] * (Radius + 1u - i);
                            sum_g += src_ptr[1] * (Radius + 1u - i);
                            sum_b += src_ptr[2] * (Radius + 1u - i);
                            sum_in_r += src_ptr[0];
                            sum_in_g += src_ptr[1];
                            sum_in_b += src_ptr[2];
                        }

                        sp = Radius;
                        yp = Radius;

                        if (yp > hm)
                        {
                            yp = hm;
                        }

                        src_ptr = src + 4 * (x + yp * w) + Y * w4;
                        dst_ptr = src + 4 * x + Y * w4;

                        for (y = Y; y < Height; y++)
                        {
                            uint alpha = dst_ptr[3];
                            dst_ptr[0] = (byte)Math.Clamp((long)(sum_r * mul_sum) >> shr_sum, 0, (long)alpha);
                            dst_ptr[1] = (byte)Math.Clamp((long)(sum_g * mul_sum) >> shr_sum, 0, (long)alpha);
                            dst_ptr[2] = (byte)Math.Clamp((long)(sum_b * mul_sum) >> shr_sum, 0, (long)alpha);
                            dst_ptr += w4;

                            sum_r -= sum_out_r;
                            sum_g -= sum_out_g;
                            sum_b -= sum_out_b;

                            stack_start = sp + div - Radius;
                            if (stack_start >= div)
                                stack_start -= div;
                            stack_ptr = &p[3 * stack_start];

                            sum_out_r -= stack_ptr[0];
                            sum_out_g -= stack_ptr[1];
                            sum_out_b -= stack_ptr[2];

                            if (yp < hm)
                            {
                                src_ptr += w4; // stride
                                ++yp;
                            }

                            stack_ptr[0] = src_ptr[0];
                            stack_ptr[1] = src_ptr[1];
                            stack_ptr[2] = src_ptr[2];

                            sum_in_r += src_ptr[0];
                            sum_in_g += src_ptr[1];
                            sum_in_b += src_ptr[2];
                            sum_r += sum_in_r;
                            sum_g += sum_in_g;
                            sum_b += sum_in_b;

                            ++sp;
                            if (sp >= div)
                                sp = 0;
                            stack_ptr = &p[sp * 3];

                            sum_out_r += stack_ptr[0];
                            sum_out_g += stack_ptr[1];
                            sum_out_b += stack_ptr[2];
                            sum_in_r -= stack_ptr[0];
                            sum_in_g -= stack_ptr[1];
                            sum_in_b -= stack_ptr[2];
                        }
                    }
                }
            }
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
