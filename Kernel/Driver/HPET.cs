/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
//https://wiki.osdev.org/HPET#Interrupt_routing

using Kernel.Misc;
using System.Diagnostics;
using static Kernel.Misc.MMIO;

namespace Kernel.Driver
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
            Clock = (In(0) >> 32);
            Out(0x10, 0);
            Out(0xF0, 0);
            Out(0x10, 1);
            Console.WriteLine("[HPET] HPET Initialized");
        }

        public static void Out(ulong reg,ulong value)
        {
            Out64((ulong*)(ACPI.HPET->Addresses.Address + reg), value);
        }

        public static ulong In(ulong reg)
        {
            return In64((ulong*)(ACPI.HPET->Addresses.Address + reg));
        }

        //Broken
        public static void ConfigureTimer(int Index,ulong Millionseconds, ulong IRQ)
        {
            ulong time = (Millionseconds * 10000000000000) / Clock;
            Out((ulong)(0x100 + 0x20 * Index), (IRQ << 9) | (1 << 2) | (1 << 3) | (1 << 6));
            Out((ulong)(0x108 + 0x20 * Index), In(0) + time);
            Out((ulong)(0x108 + 0x20 * Index), time);
            Interrupts.EnableInterrupt((byte)IRQ);
        }

        public static ulong GetTickCount()
        {
            return In(0xF0);
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
