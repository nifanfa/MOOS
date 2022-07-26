
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
    public static class Debug
    {
        public static void WriteLine(string s) 
        {
            for(int i = 0; i < s.Length; i++) 
            {
                DebugWrite(s[i]);
            }
            DebugWriteLine();
            s.Dispose();
        }

        public static void WriteLine()
        {
            DebugWriteLine();
        }

        public static void Write(char c)
        {
            DebugWrite(c);
        }

        public static void Write(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                DebugWrite(s[i]);
            }
            s.Dispose();
        }

        [DllImport("*")]
        static extern void DebugWrite(char c);

        [DllImport("*")]
        static extern void DebugWriteLine();
    }
}
