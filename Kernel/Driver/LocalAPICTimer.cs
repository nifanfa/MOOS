using MOOS.Misc;

namespace MOOS.Driver
{
    public static class LocalAPICTimer
    {
        public static ulong Ticks => LocalAPIC.ReadRegister(0x390);

        public static void StartTimer(ulong hz, uint irq)
        {
            LocalAPIC.WriteRegister(0x320, 0x00020000 | irq);
            //Divide 16
            LocalAPIC.WriteRegister(0x3e0, 0x3);
            LocalAPIC.WriteRegister(0x380, (uint)((Timer.Bus_Clock / 16) / hz));
            Interrupts.EnableInterrupt(0x20);
        }

        public static uint EstimateBusSpeed() 
        {
            LocalAPIC.WriteRegister(0x320, 0x10000);
            LocalAPIC.WriteRegister(0x3e0, 0x3);
            LocalAPIC.WriteRegister(0x320, 0);
            uint T0 = 0xFFFFFFFF;
            LocalAPIC.WriteRegister(0x380, T0);
            //0.1 second
            ACPITimer.SleepMicroseconds(100000);
            LocalAPIC.WriteRegister(0x320, 0x10000);
            ulong Freq = (T0 - Ticks) * 16;
            Freq = Freq * 1000000 / 100000;
            return (uint)Freq;
        }

        public static void StopTimer()
        {
            LocalAPIC.WriteRegister(0x320, 0x00020000 | 0x10000);
        }
    }
}