/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
using Kernel.Driver;
using Kernel.Misc;

namespace Kernel
{
    public class PIT
    {
        public static void Initialise()
        {
            ushort timerCount = 1193182 / 1000;

            Native.Out8(0x43, 0x36);
            Native.Out8(0x40, (byte)(timerCount & 0xFF));
            Native.Out8(0x40, (byte)((timerCount & 0xFF00) >> 8));

            Interrupts.EnableInterrupt(0x20);
        }
    }
}
