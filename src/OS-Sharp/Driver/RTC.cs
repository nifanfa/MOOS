// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence
namespace OS_Sharp
{
    public static class RTC
    {
        private static byte B;

        public static byte Get(byte index)
        {
            Native.Out8(0x70, index);
            byte result = Native.In8(0x71);

            return result;
        }

        public static void Set(byte index, byte value)
        {
            Native.Out8(0x70, index);
            Native.Out8(0x71, value);
        }

        private static void Delay()
        {
            Native.In8(0x80);
            Native.Out8(0x80, 0);
        }

        public static byte Second
        {
            get
            {
                B = Get(0);
                return (byte)((B & 0x0F) + ((B / 16) * 10));
            }
        }

        public static byte Minute
        {
            get
            {
                B = Get(2);
                return (byte)((B & 0x0F) + ((B / 16) * 10));
            }
        }

        public static byte Hour
        {
            get
            {
                B = Get(4);
                return (byte)(((B & 0x0F) + ((B & 0x70) / 16 * 10)) | (B & 0x80));
            }
        }

        public static byte Century
        {
            get
            {
                B = Get(0x32);
                return (byte)((B & 0x0F) + ((B / 16) * 10));
            }
        }

        public static byte Year
        {
            get
            {
                B = Get(9);
                return (byte)((B & 0x0F) + ((B / 16) * 10));
            }
        }

        public static byte Month
        {
            get
            {
                B = Get(8);
                return (byte)((B & 0x0F) + ((B / 16) * 10));
            }
        }

        public static byte Day
        {
            get
            {
                B = Get(7);
                return (byte)((B & 0x0F) + ((B / 16) * 10));
            }
        }

        public static bool BCD => (Get(0x0B) & 0x04) == 0x00;
    }
}
