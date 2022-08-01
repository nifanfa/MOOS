namespace MOOS.Misc
{
    public static unsafe class MMIO
    {
        public static byte In8(byte* p) 
        {
            return *p;
        }

        public static ushort In16(ushort* p)
        {
            return *p;
        }

        public static uint In32(uint* p)
        {
            return *p;
        }

        public static ulong In64(ulong* p)
        {
            return *p;
        }

        public static void Out8(byte* p,byte value) 
        {
            *p = value;
        }

        public static void Out16(ushort* p, ushort value)
        {
            *p = value;
        }

        public static void Out32(uint* p, uint value)
        {
            *p = value;
        }

        public static void Out64(ulong* p, ulong value)
        {
            *p = value;
        }
    }
}