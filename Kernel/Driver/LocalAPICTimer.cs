using MOOS.Misc;

namespace MOOS.Driver
{
    public static partial class LocalAPIC
    {
        public static ulong Ticks => ReadRegister(0x390);

        public static void StartTimer(ulong hz, uint irq)
        {
            WriteRegister(0x320, 0x00020000 | irq);
            //Divide 16
            WriteRegister(0x3e0, 0x3);
            WriteRegister(0x380, (uint)((Timer.Bus_Clock / 16) / hz));
            Interrupts.EnableInterrupt(0x20);
        }

        public static uint EstimateBusSpeed() 
        {
            WriteRegister(0x320, 0x10000);
            WriteRegister(0x3e0, 0x3);
            WriteRegister(0x320, 0);
            uint T0 = 0xFFFFFFFF;
            WriteRegister(0x380, T0);
            //0.1 second
            ACPI.Sleep(100000);
            WriteRegister(0x320, 0x10000);
            ulong Freq = (T0 - Ticks) * 16;
            Freq = Freq * 1000000 / 100000;
            return (uint)Freq;
        }

        public static void StopTimer()
        {
            WriteRegister(0x320, 0x00020000 | 0x10000);
        }
    }
}