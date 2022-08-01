using System.Runtime.InteropServices;

namespace System
{
    public static unsafe class MathF
    {
        [DllImport("*")]
        static extern void _fsin(float* x);

        [DllImport("*")]
        static extern void _fcos(float* x);

        [DllImport("*")]
        static extern void _fsqrt(float* x);

        [DllImport("*")]
        static extern void _frndint(float* x);

        public static float Sin(float x) 
        {
            _fsin(&x);
            return x;
        }

        public static float Cos(float x)
        {
            _fcos(&x);
            return x;
        }

        public static float Tan(float x)
        {
            return Sin(x) / Cos(x);
        }

        public static float Sqrt(float x)
        {
            _fsqrt(&x);
            return x;
        }

        public static float Round(float x)
        {
            _frndint(&x);
            return x;
        }

        public static float Floor(float x)
        {
            if (x >= 0.0)
            {
                if (x < ((int.MaxValue / 2 + 1) * 2.0))
                {
                    return (float)(int)x;
                }
                return x;
            }
            else if (x < 0.0)
            {
                if (x >= int.MinValue)
                {
                    int ix = (int)x;
                    return (ix == x) ? x : (float)(ix - 1);
                }
                return x;
            }
            return x;
        }
    }
}
