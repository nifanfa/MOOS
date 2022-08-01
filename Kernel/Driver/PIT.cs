using System;
using MOOS.Driver;
using MOOS.Misc;

namespace MOOS
{
    [Obsolete("Use ACPI Timer or Local APIC Timer")]
    public class PIT
    {
        public const int Clock = 1193182;

        public static void Initialise(int hz)
        {
            ushort timerCount = (ushort)(Clock / hz);

            Native.Out8(0x43, 0x36);
            Native.Out8(0x40, (byte)(timerCount & 0xFF));
            Native.Out8(0x40, (byte)((timerCount & 0xFF00) >> 8));

            Interrupts.EnableInterrupt(0x20);
        }
    }
}