/*
* Copyright (c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
*/

using OS_Sharp.Misc;

namespace OS_Sharp.Driver
{
    public static unsafe class LocalAPIC
    {
        public const int LAPIC_ID = 0x0020;
        public const int LAPIC_VER = 0x0030;
        public const int LAPIC_TPR = 0x0080;
        public const int LAPIC_APR = 0x0090;
        public const int LAPIC_PPR = 0x00a0;
        public const int LAPIC_EOI = 0x00b0;
        public const int LAPIC_RRD = 0x00c0;
        public const int LAPIC_LDR = 0x00d0;
        public const int LAPIC_DFR = 0x00e0;
        public const int LAPIC_SVR = 0x00f0;
        public const int LAPIC_ISR = 0x0100;
        public const int LAPIC_TMR = 0x0180;
        public const int LAPIC_IRR = 0x0200;
        public const int LAPIC_ESR = 0x0280;
        public const int LAPIC_ICRLO = 0x0300;
        public const int LAPIC_ICRHI = 0x0310;
        public const int LAPIC_TIMER = 0x0320;
        public const int LAPIC_THERMAL = 0x0330;
        public const int LAPIC_PERF = 0x0340;
        public const int LAPIC_LINT0 = 0x0350;
        public const int LAPIC_LINT1 = 0x0360;
        public const int LAPIC_ERROR = 0x0370;
        public const int LAPIC_TICR = 0x0380;
        public const int LAPIC_TCCR = 0x0390;
        public const int LAPIC_TDCR = 0x03e0;
        public const int ICR_FIXED = 0x00000000;
        public const int ICR_LOWEST = 0x00000100;
        public const int ICR_SMI = 0x00000200;
        public const int ICR_NMI = 0x00000400;
        public const int ICR_INIT = 0x00000500;
        public const int ICR_STARTUP = 0x00000600;
        public const int ICR_PHYSICAL = 0x00000000;
        public const int ICR_LOGICAL = 0x00000800;
        public const int ICR_IDLE = 0x00000000;
        public const int ICR_SEND_PENDING = 0x00001000;
        public const int ICR_DEASSERT = 0x00000000;
        public const int ICR_ASSERT = 0x00004000;
        public const int ICR_EDGE = 0x00000000;
        public const int ICR_LEVEL = 0x00008000;
        public const int ICR_NO_SHORTHAND = 0x00000000;
        public const int ICR_SELF = 0x00040000;
        public const int ICR_ALL_INCLUDING_SELF = 0x00080000;
        public const int ICR_ALL_EXCLUDING_SELF = 0x000c0000;
        public const int ICR_DESTINATION_SHIFT = 24;

        private static uint In(uint reg)
        {
            return MMIO.In32((uint*)(ACPI.MADT->LocalAPICAddress + reg));
        }

        private static void Out(uint reg, uint data)
        {
            MMIO.Out32((uint*)(ACPI.MADT->LocalAPICAddress + reg), data);
        }

        public static void EndOfInterrupt()
        {
            Out(LAPIC_EOI, 0);
        }

        public static bool Initialize(uint CPUForIRQHandlingIndex = 0)
        {
            //Enable All Interrupts
            Out(LAPIC_TPR, 0);

            // Logical Destination Mode
            Out(LAPIC_DFR, 0xffffffff);   // Flat mode
            Out(LAPIC_LDR, CPUForIRQHandlingIndex << 24);   // All cpus use logical id CPUforIRQHandlingIndex

            // Configure Spurious Interrupt Vector Register
            Out(LAPIC_SVR, 0x100 | 0xff);

            return true;
        }

        public static uint GetId()
        {
            return In(LAPIC_ID) >> 24;
        }

        public static void SendInit(uint apic_id)
        {
            Out(LAPIC_ICRHI, apic_id << ICR_DESTINATION_SHIFT);
            Out(LAPIC_ICRLO, ICR_INIT | ICR_PHYSICAL
                | ICR_ASSERT | ICR_EDGE | ICR_NO_SHORTHAND);

            while ((In(LAPIC_ICRLO) & ICR_SEND_PENDING) != 0)
            {
                ;
            }
        }

        public static void SendStartup(uint apic_id, uint vector)
        {
            Out(LAPIC_ICRHI, apic_id << ICR_DESTINATION_SHIFT);
            Out(LAPIC_ICRLO, vector | ICR_STARTUP
                | ICR_PHYSICAL | ICR_ASSERT | ICR_EDGE | ICR_NO_SHORTHAND);

            while ((In(LAPIC_ICRLO) & ICR_SEND_PENDING) != 0)
            {
                ;
            }
        }
    }
}
