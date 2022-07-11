using MOOS.Misc;

namespace MOOS.Driver
{
    public static partial class LocalAPIC
    {
        public static ulong Ticks => In(0x390);

        public static void StartTimer(ulong hz, uint irq)
        {
            hz *= 16;
            Out(0x320, 0x00020000 | irq);
            Out(0x3e0, 0x3);
            Out(0x380, (uint)(Timer.CPU_Clock / hz));
            Interrupts.EnableInterrupt(0x20);
        }

        public static void StopTimer()
        {
            Out(0x320, 0x00020000 | 0x10000);
        }
    }
}