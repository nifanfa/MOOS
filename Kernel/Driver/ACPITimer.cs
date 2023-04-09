namespace MOOS.Driver
{
    public static unsafe class ACPITimer
    {
        const int Clock = 3579545;

        public static void Sleep(ulong ms)
        {
            if (ACPI.FADT->PMTimerLength != 4)
            {
                Console.Write("ACPI Timer is not present!\n");
                for (; ; );
            }

            ulong delta = 0;
            ulong count = ms * (Clock / 1000);
            ulong last = Native.In32((ushort)ACPI.FADT->PMTimerBlock) & 0xFFFFFF;
            while (count != 0)
            {
                ulong curr = Native.In32((ushort)ACPI.FADT->PMTimerBlock) & 0xFFFFFF;
                if (curr > last)
                {
                    delta = curr - last;
                }
                if (last > curr)
                {
                    delta = (curr + 0xFFFFFF) - last;
                }
                last = curr;

                if (count > delta)
                {
                    count -= delta;
                }
                else
                {
                    count = 0;
                }
            }
        }

        public static void SleepMicroseconds(ulong Microseconds)
        {
            if (ACPI.FADT->PMTimerLength != 4) return;

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
                    Counter += ((((ACPI.FADT->Flags >> 8) & 0x01) & 0x01) ? 0x100000000ul : 0x1000000) + Current - Last;
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
