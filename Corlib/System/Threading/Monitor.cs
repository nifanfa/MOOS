using Internal.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Threading
{
    public static unsafe class Monitor
    {
        public static void Enter(object obj)
        {
            Lock();
        }

        public static void Exit(object obj)
        {
            Unlock();
        }

        [DllImport("*")]
        static extern void Lock();

        [DllImport("*")]
        static extern void Unlock();
    }
}
