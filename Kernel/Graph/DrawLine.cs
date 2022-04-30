//Xiaolin Wu's line algorithm

namespace Kernel.Graph
{
    public unsafe partial class Graphics
    {
        // swaps two numbers
        static void Swap(int* a, int* b)
        {
            int temp = *a;
            *a = *b;
            *b = temp;
        }

        // returns absolute value of number
        static float Absolute(float x)
        {
            if (x < 0) return -x;
            else return x;
        }

        //returns integer part of a floating point number
        static int IPartOfNumber(float x)
        {
            return (int)x;
        }

        //rounds off a number
        static int RoundNumber(float x)
        {
            return IPartOfNumber(x + 0.5f);
        }

        //returns fractional part of a number
        static float FPartOfNumber(float x)
        {
            if (x > 0) return x - IPartOfNumber(x);
            else return x - (IPartOfNumber(x) + 1);

        }

        //returns 1 - fractional part of number
        static float RFPartOfNumber(float x)
        {
            return 1 - FPartOfNumber(x);
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
    }
}
