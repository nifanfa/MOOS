#if Kernel
using Internal.Runtime.CompilerServices;
using MOOS;
using MOOS.Driver;
using MOOS.Misc;

namespace System.Threading
{
    public static unsafe class Monitor
    {
        public static void Enter(object obj)
        {
            if (ThreadPool.CanLock)
            {
                ThreadPool.Lock();
            }
        }

        public static void Exit(object obj)
        {
            if (ThreadPool.CanLock)
            {
                ThreadPool.UnLock();
            }
        }
    }
}
#endif