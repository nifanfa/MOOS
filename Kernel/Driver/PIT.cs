/*
 * Copyright(c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
 */
using Kernel.Driver;

namespace Kernel
{
    public class PIT
    {
        public static ulong Ticks = 0;

        public static void Initialise()
        {
            ushort timerCount = 1193182 / 1000;

            Native.Out8(0x43, 0x36);
            Native.Out8(0x40, (byte)(timerCount & 0xFF));
            Native.Out8(0x40, (byte)((timerCount & 0xFF00) >> 8));

            IOAPIC.SetEntry(0x20);
        }

        internal static void OnInterrupt()
        {
            Ticks = Ticks + 1;
        }

        public static void Wait(ulong millisecond)
        {
            ulong T = Ticks;
            while (Ticks < (T + millisecond)) Native.Hlt();
        }
    }
}
