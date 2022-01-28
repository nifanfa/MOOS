using Kernel.Driver;
using System.Windows.Forms;

namespace Kernel
{
    public static unsafe class Framebuffer
    {
        public static ushort Width;
        public static ushort Height;

        public static uint* VideoMemory;

        public static uint* FirstBuffer;
        public static uint* SecondBuffer;

        static bool _TripleBuffered = false;

        /// <summary>
        /// Since you enabled TripleBuffered you have to call Framebuffer.Update() in order to make it display
        /// </summary>
        public static bool TripleBuffered 
        {
            get 
            {
                return _TripleBuffered;
            }
            set 
            {
                Clear(0x0);
                _TripleBuffered = value;
            }
        }

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

        public static void Copy(int dX,int dY,int sX,int sY,int Width,int Height)
        {
            for(int w = 0; w < Width; w++) 
            {
                for(int h = 0; h < Height; h++) 
                {
                    DrawPoint(dX + w, dY + h, GetPoint(sX + w, sY + h));
                }
            }
        }

        public static void Clear(uint Color)
        {
            Native.Stosd(TripleBuffered ? FirstBuffer : VideoMemory, Color, (ulong)(Width * Height));
        }

        public static void DrawPoint(int X, int Y, uint Color)
        {
            if (X > 0 && Y > 0 && X < Width && Y < Height)
            {
                if(TripleBuffered)
                    FirstBuffer[Width * Y + X] = Color;
                else
                    VideoMemory[Width * Y + X] = Color;
            }
        }

        public static uint GetPoint(int X, int Y)
        {
            if (X > 0 && Y > 0 && X < Width && Y < Height)
            {
                if (TripleBuffered)
                    return FirstBuffer[Width * Y + X];
                else
                    return VideoMemory[Width * Y + X];
            }
            return 0;
        }

        public static void Update()
        {
            if (TripleBuffered)
            {
                for(int i = 0; i < Width * Height; i++) 
                {
                    if(FirstBuffer[i] != SecondBuffer[i]) 
                    {
                        VideoMemory[i] = FirstBuffer[i];
                    }
                }
                Native.Movsd(SecondBuffer, FirstBuffer, (ulong)(Width * Height));
            }
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
            FirstBuffer = (uint*)Heap.Allocate((ulong)(XRes * YRes * 4));
            SecondBuffer = (uint*)Heap.Allocate((ulong)(XRes * YRes * 4));
            Native.Stosd(FirstBuffer, 0, (ulong)(XRes * YRes));
            Native.Stosd(SecondBuffer, 0, (ulong)(XRes * YRes));
            Control.MousePosition.X = XRes / 2;
            Control.MousePosition.Y = YRes / 2;

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
