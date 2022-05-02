using Kernel.Driver;

namespace Kernel.Misc
{
    public static class Interrupts
    {
        public static void EndOfInterrupt(byte irq) 
        {
#if UseAPIC
            LocalAPIC.EndOfInterrupt();
#else
            PIC.EndOfInterrupt(irq);
#endif
        }

        public static void EnableInterrupt(byte irq) 
        {
#if UseAPIC
            IOAPIC.SetEntry(irq);
#else
            PIC.ClearMask(irq);
#endif
        }
    }
}
