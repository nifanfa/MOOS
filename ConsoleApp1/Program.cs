using System.Runtime;
using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    static class Program
    {
        [DllImport("ConsoleWriteLine")]
        public static extern void WriteLine(string s);

        [RuntimeExport("Main")]
        public static void Main() 
        {
            WriteLine("Hello World!");
        }
    }
}
