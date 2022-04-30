#if Kernel
using Kernel;
using Kernel.Misc;

namespace System.Threading
{
    public static class Monitor
    {
        public static void Enter(object obj)
        {
            ThreadPool.Locked = true;
        }

        public static void Exit(object obj)
        {
            ThreadPool.Locked = false;
        }
    }
}
#endif