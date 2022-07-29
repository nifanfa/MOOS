namespace System
{
    public static class MathF
    {
        public static double Floor(double x)
        {
            if (x >= 0.0)
            {
                if (x < ((long.MaxValue / 2 + 1) * 2.0))
                {
                    return (double)(long)x;
                }
                return x;
            }
            else if (x < 0.0)
            {
                if (x >= long.MinValue)
                {
                    long ix = (long)x;
                    return (ix == x) ? x : (double)(ix - 1);
                }
                return x;
            }
            return x;
        }
    }
}
