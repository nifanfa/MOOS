using MOOS;
using MOOS.Driver;
using MOOS.Graph;
using System;
using System.Collections.Generic;


namespace System
{
    public unsafe class UIFramebuffer
    {
        public static Dictionary<int, UIFramebuffer> Frames = new Dictionary<int, UIFramebuffer>();
        public int UID { set; get; }
        public int X { set; get; }
        public int Y { set; get; }
        public int Width { set; get; }
        public int Height { set; get; }
        public uint* FirstBuffer { set; get; }

        public UIFramebuffer(int x, int y, int w, int h)
        {
            FirstBuffer = (uint*)Allocator.Allocate((ulong)(Framebuffer.Width * Framebuffer.Height * 4));
            Native.Stosd(FirstBuffer, 0, (ulong)(Framebuffer.Width * Framebuffer.Height));

            X = x;
            Y = y;
            Width = w;
            Height = h;
            UID = (Frames.Count + 1);
            Frames.Add(UID, this);
        }
    }
}
