namespace System
{
    public static class Math
    {
        public static int Abs(int value) 
        {
            return value < 0 ? value * -1 : value;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static int Min(int a, int b)
        {
            return (a <= b) ? a : b;
        }

        public static int Max(int a, int b)
        {
            return (a >= b) ? a : b;
        }
    }
}
