/*
* Copyright (c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
*/


namespace System
{
    internal static class BitHelpers
    {
        public static bool IsBitSet(sbyte val, int pos)
        {
            return (((byte)val) & (1U << pos)) != 0;
        }

        public static bool IsBitSet(byte val, int pos)
        {
            return (val & (1U << pos)) != 0;
        }

        public static bool IsBitSet(short val, int pos)
        {
            return (((ushort)val) & (1U << pos)) != 0;
        }

        public static bool IsBitSet(ushort val, int pos)
        {
            return (val & (1U << pos)) != 0;
        }

        public static bool IsBitSet(int val, int pos)
        {
            return (((uint)val) & (1U << pos)) != 0;
        }

        public static bool IsBitSet(uint val, int pos)
        {
            return (val & (1U << pos)) != 0;
        }

        public static bool IsBitSet(long val, int pos)
        {
            return (((ulong)val) & (1UL << pos)) != 0;
        }

        public static bool IsBitSet(ulong val, int pos)
        {
            return (val & (1UL << pos)) != 0;
        }
    }
}
