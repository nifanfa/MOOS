// Copywrite (C) 2021 Contributors of nifanfa/Solution1. Licensed under the  MIT licence
using Kernel;

namespace System.Diagnostics
{
    public static class Debug
    {
        public static void WriteLine(string s)
        {
            Serial.WriteLine(s);
            s.Dispose();
        }

        public static void WriteLine()
        {
            Serial.WriteLine();
        }

        public static void Write(char c)
        {
            Serial.Write(c);
        }

        public static void Write(string s)
        {
            Serial.Write(s);
            s.Dispose();
        }
    }
}
