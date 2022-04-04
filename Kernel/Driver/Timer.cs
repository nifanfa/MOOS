namespace Kernel.Driver
{
    public static class Timer
    {
        public static ulong Ticks => PIT.Ticks;

        public static void Wait(ulong ms) 
        {
            PIT.Wait(ms);
        }
    }
}
