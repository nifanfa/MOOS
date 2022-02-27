// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the  MIT licence
namespace System
{
    public static class Math
    {
        public static int Abs(int value)
        {
            return value < 0 ? value * -1 : value;
        }
        public static int GetMedian(int low, int hi)
        {
            return low + ((hi - low) >> 1);
        }
        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
            {
                return min;
            }

            if (value > max)
            {
                return max;
            }

            return value;
        }
    }
}
