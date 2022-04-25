namespace Kernel.Misc
{
    public static class Interrupts
    {
        public static void EndOfInterrupt(byte irq) 
        {
            PIC.EndOfInterrupt(irq);
        }

        public static void EnableInterrupt(byte irq) 
        {
            PIC.ClearMask(irq);
        }
    }
}
