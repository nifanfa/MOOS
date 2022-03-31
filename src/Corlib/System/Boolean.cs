/*
* Copyright (c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
*/

namespace System
{
    public unsafe struct Boolean
    {
        public override string ToString()
        {
            return this ? "true" : "false";
        }

        public static implicit operator bool(byte value)
        {
            return value != 0;
        }

        public static implicit operator bool(sbyte value)
        {
            return value != 0;
        }

        public static implicit operator bool(short value)
        {
            return value != 0;
        }

        public static implicit operator bool(ushort value)
        {
            return value != 0;
        }

        public static implicit operator bool(int value)
        {
            return value != 0;
        }

        public static implicit operator bool(uint value)
        {
            return value != 0;
        }

        public static implicit operator bool(long value)
        {
            return value != 0;
        }

        public static implicit operator bool(ulong value)
        {
            return value != 0;
        }

        public static implicit operator bool(float value)
        {
            return value != 0;
        }

        public static implicit operator bool(double value)
        {
            return value != 0;
        }

        public static implicit operator bool(void* value)
        {
            return value != 0;
        }
    }
}
