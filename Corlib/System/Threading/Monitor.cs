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
            if (Unsafe.As<long, ulong>(ref ThreadPool.Locker))
            {
                ThreadPool.Locked = true;
            }
        }

        public static void Exit(object obj)
        {
            if (Unsafe.As<long, ulong>(ref ThreadPool.Locker))
            {
                ThreadPool.Locked = false;
            }
        }
    }
}
#endif