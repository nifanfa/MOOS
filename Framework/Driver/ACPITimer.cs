namespace MOOS.Driver
{
    public static unsafe class ACPITimer
    {
        public const int Clock = 3579545;

        public static void Sleep(ulong Microseconds)
        {
            if (ACPI.FADT->PMTimerLength != 4)
            {
                return;
            }

            ulong Clock;
            ulong Counter;
            ulong Last;

            Clock = ACPITimer.Clock * Microseconds / 1000000;

            Last = Native.In32(ACPI.FADT->PMTimerBlock);
            Counter = 0;
            while (Counter < Clock)
            {
                ulong Current = Native.In32(ACPI.FADT->PMTimerBlock);
                if (Current < Last)
                {
                    Counter += (((ACPI.FADT->Flags >> 8) & 0x01) ? 0x100000000ul : 0x1000000) + Current - Last;
                } else
                {
                    Counter += Current - Last;
                }
                Last = Current;
                Native.Nop();
            }
        }
    }
}
