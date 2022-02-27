//https://wiki.osdev.org/HPET#Interrupt_routing

using System.Diagnostics;
using static Kernel.Misc.MMIO;

namespace Kernel.Driver
{
    public static unsafe class HPET
    {
        public static ulong Clock;

        public static bool Initialize()
        {
            if (ACPI.HPET == null) 
            {
                return false;
            }

            Clock = (In64((ulong*)(ACPI.HPET->Addresses.Address + 0)) >> 32);
            Out64((ulong*)(ACPI.HPET->Addresses.Address + 0x10), 0);
            Out64((ulong*)(ACPI.HPET->Addresses.Address + 0xF0), 0);
            Out64((ulong*)(ACPI.HPET->Addresses.Address + 0x10), 1);
            return true;
        }

        public static ulong GetTickCount()
        {
            return In64((ulong*)(ACPI.HPET->Addresses.Address + 0xF0));
        }

        public static void Wait(ulong Millionseconds) 
        {
            WaitMicrosecond(Millionseconds * 10000);
        }

        public static void WaitMicrosecond(ulong Microsecond)
        {
            ulong Until = GetTickCount() + (Microsecond * 1000000000) / Clock;
            while ((GetTickCount()) < Until) Native.Nop();
        }
    }
}
