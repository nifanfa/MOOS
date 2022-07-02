using MOOS.Misc;

namespace MOOS.Driver
{
    public static class Timer
    {
        public static ulong Ticks = 0;

        internal static void OnInterrupt()
        {
            //This method is only for bootstrap CPU
            if(SMP.ThisCPU == 0)
            {
                Ticks++;

                if (ThreadPool.Locked)
                {
                    Ticks--;
                }
            }
        }

        public static void Wait(ulong millisecond)
        {
            ulong T = Ticks;
            while (Ticks < (T + millisecond)) Native.Hlt();
        }
    }
}