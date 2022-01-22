namespace ConsoleApp1
{
    /// <summary>
    /// Bochs VBE Extensions.
    /// Warning: This framebuffer device is only for virtual machine.
    /// Map the bar0 before use!
    /// </summary>
    public static unsafe class BGA
    {
        public static uint* Ptr;
        public static ushort Width;
        public static ushort Height;
        public static uint* Buffer;

        public static void Setup()
        {
            Ptr = (uint*)0xFD000000;
        }

        public static void Clear(uint Color)
        {
            for (int i = 0; i < Width * Height; i++) Ptr[i] = Color;
        }

        public static void DrawPoint(int X, int Y, uint Color)
        {
            if (X > 0 && Y > 0)
                Buffer[Width * Y + X] = Color;
        }

        public static void WriteRegister(ushort IndexValue, ushort DataValue)
        {
            Native.outw(0x01CE, IndexValue);
            Native.outw(0x01CF, DataValue);
        }

        public static void SetVideoMode(ushort XRes, ushort YRes)
        {
            Width = XRes;
            Height = YRes;
            WriteRegister(4, 0);
            WriteRegister(1, XRes);
            WriteRegister(2, YRes);
            WriteRegister(3, 32);
            WriteRegister(4, (ushort)(1 | 0x40));
            uint* p = stackalloc uint[XRes * YRes];
            Buffer = p;
        }
    }
}
