namespace Kernel
{
    public static unsafe class Framebuffer
    {
        public static uint* VideoMemory;
        public static ushort Width;
        public static ushort Height;
        public static uint* Buffer;

        public static void Setup()
        {
            for (int i = 0; i < PCI.Devices.Length; i++)
            {
                if (PCI.Devices[i].VendorID == 0x1234)
                {
                    VideoMemory = (uint*)PCI.Devices[i].Bar0;
                    return;
                }
            }
        }

        public static void Clear(uint Color)
        {
            Native.Stosd(Buffer, Color, (ulong)(Width * Height));
        }

        public static void DrawPoint(int X, int Y, uint Color)
        {
            if (X > 0 && Y > 0)
                Buffer[Width * Y + X] = Color;
        }

        public static void Update()
        {
            Native.Movsd(VideoMemory, Buffer, (ulong)(Width * Height));
        }

        public static void WriteRegister(ushort IndexValue, ushort DataValue)
        {
            Native.Out16(0x01CE, IndexValue);
            Native.Out16(0x01CF, DataValue);
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
            Buffer = (uint*)Platform.kmalloc((ulong)(XRes * YRes * 4));
        }
    }
}
