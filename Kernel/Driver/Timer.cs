using MOOS.Misc;

namespace MOOS.Driver
{
    public static class Timer
    {
        public static ulong CPU_Clock;

        public static void Initialize() 
        {
            ulong prev = Native.Rdtsc();
            Timer.Sleep(10);
            ulong next = Native.Rdtsc();
            CPU_Clock = next - prev;
            CPU_Clock *= 100;
            Console.WriteLine($"[Timer] CPU clock is {CPU_Clock / 1048576}mhz");
        }

        public static ulong Ticks { get; private set; }

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

        public static void Sleep(ulong millisecond)
        {
            ulong T = Ticks;
            while (Ticks < (T + millisecond)) Native.Hlt();
        }
    }
}