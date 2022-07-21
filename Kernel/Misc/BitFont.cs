using MOOS;
using MOOS.Misc;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MOOS.Misc
{
    public class BitFontDescriptor
    {
        public string Charset;
        public byte[] Raw;
        public int Size;
        public string Name;

        public BitFontDescriptor(string aName, string aCharset, byte[] aRaw, int aSize)
        {
            Charset = aCharset;
            Raw = aRaw;
            Size = aSize;
            Name = aName;
        }
    }

    public static class BitFont
    {
        public static List<BitFontDescriptor> RegisteredBitFont;

        public static void Initialize()
        {
            RegisteredBitFont = new List<BitFontDescriptor>();
        }

        public static void RegisterBitFont(BitFontDescriptor bitFontDescriptor)
        {
            RegisteredBitFont.Add(bitFontDescriptor);
        }

        private const int FontAlpha = 96;
        private static bool AtEdge = false;

        private static int DrawChar(byte[] Raw, int Size, int Size8, uint Color, int Index, int X, int Y, bool Calculate = false)
        {
            if (Index < 0)
            {
                return Size / 2;
            }

            int MaxX = 0;
            int SizePerFont = Size * Size8 * Index;
            AtEdge = false;

            for (int h = 0; h < Size; h++)
            {
                for (int aw = 0; aw < Size8; aw++)
                {
                    for (int ww = 0; ww < 8; ww++)
                    {
                        if ((Raw[SizePerFont + (h * Size8) + aw] & (0x80 >> ww)) != 0)
                        {
                            int max = (aw * 8) + ww;

                            int x = X + max;
                            int y = Y + h;

                            if (!Calculate)
                            {
                                Framebuffer.Graphics.DrawPoint(x, y, Color);

                                //AA

                                AtEdge = false;
                            }

                            if (max > MaxX)
                            {
                                MaxX = max;
                            }
                        }
                        else
                        {
                            AtEdge = true;
                        }
                    }
                }
            }

            return MaxX;
        }

        private static BitFontDescriptor GetBitFontDescriptor(string FontName)
        {
            for (int i = 0; i < RegisteredBitFont.Count; i++)
            {
                if (RegisteredBitFont[i].Name == FontName)
                {
                    return RegisteredBitFont[i];
                }
            }

            Panic.Error("BitFont Descriptor Not Found");
            return null;
        }

        public static int MeasureString(string FontName, string Text, int Divide = 0)
        {
            BitFontDescriptor bitFontDescriptor = GetBitFontDescriptor(FontName);

            int Size = bitFontDescriptor.Size;
            int Size8 = Size / 8;

            int UsedX = 0;
            for (int i = 0; i < Text.Length; i++)
            {
                char c = Text[i];
                UsedX += BitFont.DrawChar(bitFontDescriptor.Raw, Size, Size8, 0, bitFontDescriptor.Charset.IndexOf(c), 0, 0, true) + 2 + Divide;
            }

            return UsedX;
        }

        public static int DrawString(string FontName, uint color, string Text, int X, int Y, int LineWidth = -1, int Divide = 0)
        {
            BitFontDescriptor bitFontDescriptor = GetBitFontDescriptor(FontName);

            int Size = bitFontDescriptor.Size;
            int Size8 = Size / 8;

            int Line = 0;
            int UsedX = 0;
            for (int i = 0; i < Text.Length; i++)
            {
                char c = Text[i];
                if (c == '\n' || (LineWidth != -1 && UsedX + bitFontDescriptor.Size > LineWidth))
                {
                    Line++;
                    UsedX = 0;
                    continue;
                }
                UsedX += BitFont.DrawChar(bitFontDescriptor.Raw, Size, Size8, color, bitFontDescriptor.Charset.IndexOf(c), UsedX + X, Y + bitFontDescriptor.Size * Line, false) + 2 + Divide;
            }

            return UsedX;
        }
    }
}