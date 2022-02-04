using Kernel.Driver;
using System.Windows.Forms;

namespace Kernel
{
    public static unsafe class Framebuffer
    {
        public static int Width;
        public static int Height;

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

        public static void SetVideoMode(uint XRes, uint YRes)
        {
            Width = (int)XRes;
            Height = (int)YRes;
            FirstBuffer = (uint*)Allocator.Allocate((ulong)(XRes * YRes * 4));
            SecondBuffer = (uint*)Allocator.Allocate((ulong)(XRes * YRes * 4));
            Native.Stosd(FirstBuffer, 0, (ulong)(XRes * YRes));
            Native.Stosd(SecondBuffer, 0, (ulong)(XRes * YRes));
            Control.MousePosition.X = (int)(XRes / 2);
            Control.MousePosition.Y = (int)(YRes / 2);
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
