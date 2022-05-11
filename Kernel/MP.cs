namespace Kernel
{
    public static unsafe class MP
    {
        public struct MPPointer
        {
            public uint Signature;
            public uint MPConfig;
            public byte Length;
            public byte Version;
            public byte Checksum;
            public byte Feature1;
            public uint Feature2_3;
        }

        public static void Initialize()
        {
            uint* ptr = (uint*)0xF0000;
            while(*ptr != 0x5f504d5f && ptr < (uint*)0xfffff)
            {
                ptr++;
            }
            Console.WriteLine($"ptr:{((uint)ptr).ToString("x2")}");
            Console.ReadKey();
        }
    }
}
