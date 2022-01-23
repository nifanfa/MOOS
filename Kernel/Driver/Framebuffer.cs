using Kernel.Driver;

namespace Kernel
{
    public static unsafe class Framebuffer
    {
        public static uint* VideoMemory;
        public static ushort Width;
        public static ushort Height;
        public static uint* Buffer;

        /// <summary>
        /// Since you enabled DoubleBuffered you have to call Framebuffer.Update() in order to make it display
        /// </summary>
        public static bool DoubleBuffered = false;

        public static void Setup()
        {
            if (VBE.Info->PhysBase == 0)
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
        }

        public static void Clear(uint Color)
        {
            Native.Stosd(DoubleBuffered ? Buffer : VideoMemory, Color, (ulong)(Width * Height));
        }

        public static void DrawPoint(int X, int Y, uint Color)
        {
            if (X > 0 && Y > 0 && X < Width && Y < Height)
            {
                if(DoubleBuffered)
                    Buffer[Width * Y + X] = Color;
                else
                    VideoMemory[Width * Y + X] = Color;
            }
        }

        public static void Update()
        {
            if(DoubleBuffered)
                Native.Movsd(VideoMemory, Buffer, (ulong)(Width * Height));
        }

        public static void WriteRegister(ushort IndexValue, ushort DataValue)
        {
            if (VBE.Info->PhysBase == 0)
            {
                Native.Out16(0x01CE, IndexValue);
                Native.Out16(0x01CF, DataValue);
            }
        }

        public static void SetVideoMode(ushort XRes, ushort YRes)
        {
            Width = XRes;
            Height = YRes;
            Buffer = (uint*)Platform.kmalloc((ulong)(XRes * YRes * 4));

            if(VBE.Info->PhysBase == 0) 
            {
                WriteRegister(4, 0);
                WriteRegister(1, XRes);
                WriteRegister(2, YRes);
                WriteRegister(3, 32);
                WriteRegister(4, (ushort)(1 | 0x40));
            }
        }

        internal static void Fill(int X, int Y, int Width, int Height, uint Color)
        {
            for(int w = 0; w < Width; w++) 
            {
                for(int h = 0; h < Height; h++) 
                {
                    DrawPoint(X + w, Y + h, Color);
                }
            }
        }
    }
}
