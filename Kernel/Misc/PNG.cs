using System.Drawing;
using System.Runtime.InteropServices;

namespace Kernel.Misc
{
    public unsafe class PNG : Image
    {
        public PNG(byte[] file)
        {
            fixed (byte* p = file)
            {
                uint w, h;
                uint* _out = null;
                lodepng_decode32(&_out, &w, &h, p, file.Length);
                if (_out == null) for (; ; ) Native.Hlt();
                RawData = new uint[w * h];
                fixed (uint* pdata = RawData)
                    Native.Movsd(pdata, _out, w * h);
                Allocator.Free((System.IntPtr)_out);
                Width = (int)w;
                Height = (int)h;
                Bpp = 4;
            }
        }

        [DllImport("*")]
        public extern static uint lodepng_decode32(uint** _out, uint* w, uint* h, byte* _in, int insize);
    }
}
