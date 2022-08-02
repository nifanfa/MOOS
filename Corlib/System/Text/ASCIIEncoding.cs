using System;
using System.Collections.Generic;
using System.Text;

namespace System.Text
{
    /// <summary>
    /// Implementation of the "ASCIIEncoding" class.
    /// </summary>
    public class ASCIIEncoding : Encoding
    {
        public string CharacterSet { get; } = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";

        // Decode a buffer of bytes into a string.
        public unsafe override string GetString(byte[] bytes, int byteIndex, int count)
        {
            if (count == 0)
                return String.Empty;

            string result = "";

            for (int index = byteIndex; index < byteIndex + count; index++)
            {
                byte b = bytes[index];
                result += new string((b <= 0x7F) ? (char)b : '?', 1);
            }

            return result;
        }

        // Decode a byte into a character.
        public char GetChar(byte b)
        {
            if (b - 0x20 > CharacterSet.Length || b < 0x20)
                return CharacterSet[0x3F - 0x20];

            return CharacterSet[b - 0x20];
        }
    }
}
