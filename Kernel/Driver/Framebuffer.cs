/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
using Kernel.Driver;
using Kernel.Graph;
using Kernel.Misc;
using System.Diagnostics;
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

        public static bool NoVerification = false;

        public static Graphics Graphics;

        static bool _TripleBuffered = false;

        /// <summary>
        /// Since you enabled TripleBuffered you have to call Framebuffer.Graphics.Update() in order to make it display
        /// </summary>
        public static bool TripleBuffered 
        {
            get 
            {
                return _TripleBuffered;
            }
            set 
            {
                if (Graphics == null) return;
                if (_TripleBuffered == value) return;
                Graphics.VideoMemory = value ? FirstBuffer : VideoMemory;
                Graphics.Clear(0x0);
                _TripleBuffered = value;
                if (!_TripleBuffered)
                {
                    Console.Clear();
                    Graphics.Clear(0x0);
                }
            }
        }

        /// <summary>
        /// Enable TripleBuffered first to get DoubleBuffered work
        /// </summary>
        public static bool DoubleBuffered = false;

        public static void Update()
        {
            if (TripleBuffered)
            {
                if (DoubleBuffered)
                {
                    if (NoVerification) 
                    {
                        Native.Movsd(VideoMemory, FirstBuffer, (ulong)(Width * Height));
                    }
                    else
                    {
                        for (int i = 0; i < Width * Height; i++)
                        {
                            if (VideoMemory[i] != FirstBuffer[i])
                            {
                                VideoMemory[i] = FirstBuffer[i];
                            }
                        }
                    }
                }
                else
                {
                    if (NoVerification)
                    {
                        Native.Movsd(VideoMemory, FirstBuffer, (ulong)(Width * Height));
                    }
                    else
                    {
                        for (int i = 0; i < Width * Height; i++)
                        {
                            if (FirstBuffer[i] != SecondBuffer[i])
                            {
                                VideoMemory[i] = FirstBuffer[i];
                            }
                        }
                    }
                    Native.Movsd(SecondBuffer, FirstBuffer, (ulong)(Width * Height));
                }
            }
            Graphics.Update();
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
            Graphics = new Graphics(Width, Height, VideoMemory);
        }
    }
}
