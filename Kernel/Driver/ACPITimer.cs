namespace MOOS.Driver
{
    public static unsafe class ACPITimer
    {
        public const int Clock = 3579545;

        public static uint Ticks => Native.In32(ACPI.FADT->PMTimerBlock);

        public static bool Is32Bit => (ACPI.FADT->Flags >> 8) & 0x01;

        public static void Sleep(ulong Microseconds)
        {
            if (ACPI.FADT->PMTimerLength != 4) return;

            ulong Clock;
            ulong Counter;
            ulong Last;

            Clock = ACPITimer.Clock * Microseconds / 1000000;

            Last = Ticks;
            Counter = 0;
            while (Counter < Clock)
            {
                ulong Current = Ticks;
                if (Current < Last)
                {
                    Counter += (((Is32Bit) & 0x01) ? 0x100000000ul : 0x1000000) + Current - Last;
                }
                else
                {
                    Counter += Current - Last;
                }
                Last = Current;
                Native.Nop();
            }
        }
    }
}
