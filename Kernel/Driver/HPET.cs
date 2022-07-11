//https://wiki.osdev.org/HPET#Interrupt_routing

using MOOS.Misc;
using System.Diagnostics;
using static MOOS.Misc.MMIO;

namespace MOOS.Driver
{
    public static unsafe class HPET
    {
        public static ulong Clock;

        public static void Initialize()
        {
            if (ACPI.HPET == null) 
            {
                Console.WriteLine("[HPET] HPET not found!");
                return;
            }

            //1 Femtosecond= 1e-15 sec
            Clock = (ReadRegister(0) >> 32);
            WriteRegister(0x10, 0);
            WriteRegister(0xF0, 0);
            WriteRegister(0x10, 1);
            Console.WriteLine("[HPET] HPET Initialized");
        }

        public static void WriteRegister(ulong reg,ulong value)
        {
            Out64((ulong*)(ACPI.HPET->Addresses.Address + reg), value);
        }

        public static ulong ReadRegister(ulong reg)
        {
            return In64((ulong*)(ACPI.HPET->Addresses.Address + reg));
        }

        public static ulong GetTickCount()
        {
            return ReadRegister(0xF0);
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