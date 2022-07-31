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
            UnLock();
        }

        [DllImport("*")]
        private static extern void Lock();

        [DllImport("*")]
        private static extern void UnLock();
    }
}