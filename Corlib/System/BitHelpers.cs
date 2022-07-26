namespace System
{
    public static class BitHelpers
    {
        public static bool IsBitSet(sbyte val, int pos) => (((byte)val) & (1U << pos)) != 0;
        public static bool IsBitSet(byte val, int pos) => (val & (1U << pos)) != 0;
        public static bool IsBitSet(short val, int pos) => (((ushort)val) & (1U << pos)) != 0;
        public static bool IsBitSet(ushort val, int pos) => (val & (1U << pos)) != 0;
        public static bool IsBitSet(int val, int pos) => (((uint)val) & (1U << pos)) != 0;
        public static bool IsBitSet(uint val, int pos) => (val & (1U << pos)) != 0;
        public static bool IsBitSet(long val, int pos) => (((ulong)val) & (1UL << pos)) != 0;
        public static bool IsBitSet(ulong val, int pos) => (val & (1UL << pos)) != 0;
    }
}
