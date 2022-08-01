using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace System.Windows.Media
{
    public static class ColorConverter
    {
        public static uint ConvertFromString(string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                return 0;
            }

            hex = hex.ToUpper();

            if (hex[0] == '#')
            {
                hex = hex.Remove(0);
            }

            int i = hex.Length > 1 && hex[0] == '0' && (hex[1] == 'x' || hex[1] == 'X') ? 2 : 0;
            uint value = 0;

            while (i < hex.Length)
            {
                uint x = hex[i++];

                if (x >= '0' && x <= '9') x = x - '0';
                else if (x >= 'A' && x <= 'F') x = (x - 'A') + 10;
                else if (x >= 'a' && x <= 'f') x = (x - 'a') + 10;
                else return 0;

                value = 16 * value + x;
            }

            return value;
        }

        public static uint ConvertPixel(uint pixel, uint color)
        {
            Color _base = Color.FromArgb(pixel);
            Color _color = Color.FromArgb(color);

            int a = (_base.A * _color.A) / 255;
            int r = (_base.R * _color.R) / 255;
            int g = (_base.G * _color.G) / 255;
            int b = (_base.B * _color.B) / 255;

           return (uint)(a << 24 | r << 16 | g << 8 | b);
        }

        public static Brush FromARGB(int a, int r, int g, int b)
        {
            uint color = (uint)(a << 24 | r << 16 | g << 8 | b);
            return new Brush(color);
        }
    }
}
