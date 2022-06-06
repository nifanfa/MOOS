namespace MOOS.Driver
{
    public static class Timer
    {
        public static ulong Ticks = 0;

        internal static void OnInterrupt()
        {
            Ticks = Ticks + 1;
        }

        public static void Wait(ulong millisecond)
        {
            ulong T = Ticks;
            while (Ticks < (T + millisecond)) Native.Hlt();
        }
    }
}
