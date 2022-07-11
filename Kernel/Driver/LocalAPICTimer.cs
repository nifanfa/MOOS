using MOOS.Misc;

namespace MOOS.Driver
{
    public static partial class LocalAPIC
    {
        public static ulong Ticks => ReadRegister(0x390);

        public static void StartTimer(ulong freq, uint irq)
        {
            WriteRegister(0x320, 0x00020000 | irq);
            WriteRegister(0x3e0, 0x3);
            WriteRegister(0x380, (uint)(freq));
            Interrupts.EnableInterrupt(0x20);
        }

        public static void StopTimer()
        {
            WriteRegister(0x320, 0x00020000 | 0x10000);
        }
    }
}