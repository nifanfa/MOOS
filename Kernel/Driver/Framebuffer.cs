using System.Drawing;
using System.Windows.Forms;
using MOOS.Graph;

namespace MOOS
{
    public static unsafe class Framebuffer
    {
#pragma warning disable CA2211 // Non-constant fields should not be visible
        public static ushort Width;
#pragma warning restore CA2211 // Non-constant fields should not be visible
#pragma warning disable CA2211 // Non-constant fields should not be visible
        public static ushort Height;
#pragma warning restore CA2211 // Non-constant fields should not be visible

#pragma warning disable CA2211 // Non-constant fields should not be visible
        public static uint* FirstBuffer;
#pragma warning restore CA2211 // Non-constant fields should not be visible
#pragma warning disable CA2211 // Non-constant fields should not be visible
        public static uint* SecondBuffer;
#pragma warning restore CA2211 // Non-constant fields should not be visible

#pragma warning disable CA2211 // Non-constant fields should not be visible
        public static Graphics Graphics;
#pragma warning restore CA2211 // Non-constant fields should not be visible
        private static bool _DoubleBuffered = false;

        public static bool DoubleBuffered
        {
            get => _DoubleBuffered;
            set
            {
                if (Graphics == null)
                {
                    return;
                }

                if (_DoubleBuffered == value)
                {
                    return;
                }
                if (value == false)
                {
                    Console.Clear();
                }
                Graphics.Clear(Color.FromArgb(0x0));
                _DoubleBuffered = value;
                Graphics.VideoMemory = value ? SecondBuffer : FirstBuffer;
                Update();
            }
        }

        public static void Update()
        {
            if (_DoubleBuffered)
            {
                for (int i = 0; i < Width * Height; i++)
                {
                    if (FirstBuffer[i] != SecondBuffer[i])
                    {
                        FirstBuffer[i] = SecondBuffer[i];
                    }
                }
            }
        }

        public static void Initialize(ushort XRes, ushort YRes, uint* FramebufferHook)
        {
            Width = XRes;
            Height = YRes;

            FirstBuffer = (uint*)Allocator.Allocate((ulong)(XRes * YRes * 4));
            SecondBuffer = (uint*)Allocator.Allocate((ulong)(XRes * YRes * 4));

            Native.Stosd(FirstBuffer, 0, (ulong)(XRes * YRes));
            Native.Stosd(SecondBuffer, 0, (ulong)(XRes * YRes));

            FirstBuffer = FramebufferHook;

            Graphics = new Graphics(Width, Height, FirstBuffer);

            Control.MousePosition.X = XRes / 2;
            Control.MousePosition.Y = YRes / 2;

            // Setup console so its at first char, Graphics.Clear(0x0) in this method is not the point of this call
            Console.Clear();
            XRes.Dispose();
            YRes.Dispose();
        }
    }
}