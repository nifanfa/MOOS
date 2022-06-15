using MOOS.Driver;
using System.Collections.Generic;

namespace MOOS.Misc
{
    public static class Interrupts
    {
        public unsafe class INT 
        {
            public int IRQ;
            public delegate*<void> Handler;
        }

        public static List<INT> INTs;

        public static void Initialize() 
        {
            INTs = new List<INT>();
        }

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

        public static unsafe void EnableInterrupt(byte irq,delegate* <void> handler)
        {
#if UseAPIC
            IOAPIC.SetEntry(irq);
#else
            PIC.ClearMask(irq);
#endif
            INTs.Add(new INT() { IRQ = irq, Handler = handler });
        }

        public static unsafe void HandleInterrupt(int irq) 
        {
            for(int i = 0; i < INTs.Count; i++) 
            {
                if (INTs[i].IRQ == irq) INTs[i].Handler();
            }
        }
    }
}