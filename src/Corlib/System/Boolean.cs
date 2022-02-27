// Copywrite (C) 2021 Contributors of nifanfa/Solution1. Licensed under the  MIT licence
namespace System
{   public unsafe struct Boolean
    {
        public override string ToString()
            => this ? "true" : "false";

        public static implicit operator bool(byte value) => value != 0;
        public static implicit operator bool(sbyte value) => value != 0;
        public static implicit operator bool(short value) => value != 0;
        public static implicit operator bool(ushort value) => value != 0;
        public static implicit operator bool(int value) => value != 0;
        public static implicit operator bool(uint value) => value != 0;
        public static implicit operator bool(long value) => value != 0;
        public static implicit operator bool(ulong value) => value != 0;
        public static implicit operator bool(float value) => value != 0;
        public static implicit operator bool(double value) => value != 0;
        public static implicit operator bool(void* value) => value != 0;
    }
}
