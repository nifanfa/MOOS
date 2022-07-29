using MOOS;
using System.Drawing;

namespace MOOS.Misc
{
    public unsafe class Bitmap : Image
    {
        public Bitmap(byte[] data)
        {
            fixed (byte* p = data)
            {

                uint _Size;
                uint _DataSectionOffset;
                uint _Width;
                uint _Height;
                uint _Bpp;

                _Size = *(uint*)(p + 2);
                _DataSectionOffset = *(uint*)(p + 0xA);
                _Width = *(uint*)(p + 0x12);
                _Height = *(uint*)(p + 0x16);
                _Bpp = *(p + 0x1C);

                if (p[0] != (byte)'B' && p[1] != (byte)'M')
                {
                    Console.WriteLine("This is not a bitmap");
                    return;
                }

                if (_Bpp != 24 && _Bpp != 32)
                {
                    Console.WriteLine("Only support 24bit or 32bit bitmap");
                    return;
                }

                this.Width = (int)_Width;
                this.Height = (int)_Height;
                this.Bpp = (int)_Bpp;
                this.RawData = new int[_Width * _Height];

                int[] temp = new int[Width];
                uint w = 0;
                uint h = (uint)Height - 1;
                for (uint i = 0; i < this.Width * this.Height * (_Bpp / 8); i += (_Bpp / 8))
                {
                    if (w == Width)
                    {
                        for (uint k = 0; k < temp.Length; k++)
                        {
                            RawData[Width * h + k] = temp[k];
                        }
                        w = 0;
                        h--;
                    }
                    switch (_Bpp)
                    {
                        case 24:
                            temp[w] = (int)((*(uint*)(p + _DataSectionOffset + i) & 0x00FFFFFF) | 0xFF000000);
                            break;
                        case 32:
                            temp[w] = (int)*(uint*)(p + _DataSectionOffset + i);
                            break;

                    }
                    w++;
                }

                temp.Dispose();
            }
        }
    }
}