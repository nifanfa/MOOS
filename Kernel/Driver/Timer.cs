using MOOS.Misc;

namespace MOOS.Driver
{
    public static class Timer
    {
        public static ulong Bus_Clock;
        public static ulong CPU_Clock;

        public static void Initialize()
        {
            //PIT.Initialise(1000);
            HPET.Initialize();

            CPU_Clock = EstimateCPUSpeed();
            Console.WriteLine($"[Timer] CPU clock is {CPU_Clock / 1048576}mhz");
            Bus_Clock = LocalAPICTimer.EstimateBusSpeed();
            Console.WriteLine($"[Timer] Bus clock is {Bus_Clock / 1048576}mhz");

            LocalAPICTimer.StartTimer(1000, 0x20);
        }

        private static ulong EstimateCPUSpeed()
        {
            ulong prev = Native.Rdtsc();
            ACPITimer.SleepMicroseconds(100000);
            ulong next = Native.Rdtsc();
            ulong cpuclock = 0;
            if (next > prev) 
            {
                cpuclock = next - prev;
            }
            else 
            {
                //Overflow
                cpuclock = prev - next;
            }
            cpuclock *= 10;
            return cpuclock;
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