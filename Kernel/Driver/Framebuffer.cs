using Kernel.Driver;
using System.Drawing;
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
                if (_TripleBuffered == value) return;
                Clear(0x0);
                _TripleBuffered = value;
                if (!_TripleBuffered)
                {
                    Console.Clear();
                    Clear(0x0);
                }
            }
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

        public static void SetVideoMode(ushort XRes, ushort YRes)
        {
            Width = XRes;
            Height = YRes;
            FirstBuffer = (uint*)Allocator.Allocate((ulong)(XRes * YRes * 4));
            SecondBuffer = (uint*)Allocator.Allocate((ulong)(XRes * YRes * 4));
            Native.Stosd(FirstBuffer, 0, (ulong)(XRes * YRes));
            Native.Stosd(SecondBuffer, 0, (ulong)(XRes * YRes));
            Control.MousePosition.X = XRes / 2;
            Control.MousePosition.Y = YRes / 2;
        }

        //

        public static void Clear(uint Color)
        {
            Native.Stosd(TripleBuffered ? FirstBuffer : VideoMemory, Color, (ulong)(Width * Height));
        }

        public static void Copy(int dX, int dY, int sX, int sY, int Width, int Height)
        {
            for (int w = 0; w < Width; w++)
            {
                for (int h = 0; h < Height; h++)
                {
                    DrawPoint(dX + w, dY + h, GetPoint(sX + w, sY + h));
                }
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
        
        public static void DrawPoint(int X, int Y, uint Color)
        {
            if (X > 0 && Y > 0 && X < Width && Y < Height)
            {
                if (TripleBuffered)
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

        public static void ADrawPoint(int X, int Y, uint color)
        {
            uint bg = Framebuffer.GetPoint(X, Y);
            uint foreground = color;
            uint alpha = foreground & 0xFF000000 >> 24;
            byte R = (byte)((((((byte)((foreground >> 16) & 0xFF)) * alpha) + ((255 - alpha) * ((bg & 0x00FF0000) >> 16))) >> 8) & 0xFF);
            byte G = (byte)((((((byte)((foreground >> 8) & 0xFF)) * alpha) + ((255 - alpha) * ((bg & 0x0000FF00) >> 8))) >> 8) & 0xFF);
            byte B = (byte)((((((byte)((foreground) & 0xFF)) * alpha) + ((255 - alpha) * ((bg & 0x000000FF) >> 0))) >> 8) & 0xFF);
            DrawPoint(X, Y, Color.ToArgb(R, G, B));
        }

        public static void DrawImage(int X, int Y, Image image,bool AlphaBlending = true)
        {
            for (int h = 0; h < image.Height; h++)
                for (int w = 0; w < image.Width; w++) 
                {
                    if (AlphaBlending) 
                    {

                        uint foreground = image.RawData[image.Width * h + w];
                        int fA = (byte)((foreground >> 24) & 0xFF);
                        int fR = (byte)((foreground >> 16) & 0xFF);
                        int fG = (byte)((foreground >> 8) & 0xFF);
                        int fB = (byte)((foreground) & 0xFF);

                        uint background = GetPoint(X + w, Y + h);
                        int bA = (byte)((background >> 24) & 0xFF);
                        int bR = (byte)((background >> 16) & 0xFF);
                        int bG = (byte)((background >> 8) & 0xFF);
                        int bB = (byte)((background) & 0xFF);

                        int alpha = fA;
                        int inv_alpha = 255 - alpha;

                        int newR = (fR * alpha + inv_alpha * bR) >> 8;
                        int newG = (fG * alpha + inv_alpha * bG) >> 8;
                        int newB = (fB * alpha + inv_alpha * bB) >> 8;

                        if (fA != 0)
                        {
                            DrawPoint(X + w, Y + h, Color.ToArgb((byte)newR, (byte)newG, (byte)newB));
                        }
                    }
                    else
                    {
                        DrawPoint(X + w, Y + h, image.RawData[image.Width * h + w]);
                    }
                }
        }
    }
}
