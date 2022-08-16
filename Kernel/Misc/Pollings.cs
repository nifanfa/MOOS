using System;
using System.Collections.Generic;
using System.Text;

namespace MOOS.Misc
{
    public static unsafe class Pollings
    {
        public static void AddPoll(delegate*<void> method)
        {
            Interrupts.EnableInterrupt(0x20, method);
        }
    }
}
