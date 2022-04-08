/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
using Kernel.Misc;

namespace Kernel.Driver
{
    public static unsafe class LocalAPIC
    {
        private static int LAPIC_ID = 0x0020;
        private static int LAPIC_VER = 0x0030;
        private static int LAPIC_TPR = 0x0080;
        private static int LAPIC_APR = 0x0090;
        private static int LAPIC_PPR = 0x00a0;
        private static int LAPIC_EOI = 0x00b0;
        private static int LAPIC_RRD = 0x00c0;
        private static int LAPIC_LDR = 0x00d0;
        private static int LAPIC_DFR = 0x00e0;
        private static int LAPIC_SVR = 0x00f0;
        private static int LAPIC_ISR = 0x0100;
        private static int LAPIC_TMR = 0x0180;
        private static int LAPIC_IRR = 0x0200;
        private static int LAPIC_ESR = 0x0280;
        private static int LAPIC_ICRLO = 0x0300;
        private static int LAPIC_ICRHI = 0x0310;
        private static int LAPIC_TIMER = 0x0320;
        private static int LAPIC_THERMAL = 0x0330;
        private static int LAPIC_PERF = 0x0340;
        private static int LAPIC_LINT0 = 0x0350;
        private static int LAPIC_LINT1 = 0x0360;
        private static int LAPIC_ERROR = 0x0370;
        private static int LAPIC_TICR = 0x0380;
        private static int LAPIC_TCCR = 0x0390;
        private static int LAPIC_TDCR = 0x03e0;

        private static int ICR_FIXED = 0x00000000;
        private static int ICR_LOWEST = 0x00000100;
        private static int ICR_SMI = 0x00000200;
        private static int ICR_NMI = 0x00000400;
        private static int ICR_INIT = 0x00000500;
        private static int ICR_STARTUP = 0x00000600;

        private static int ICR_PHYSICAL = 0x00000000;
        private static int ICR_LOGICAL = 0x00000800;

        private static int ICR_IDLE = 0x00000000;
        private static int ICR_SEND_PENDING = 0x00001000;

        private static int ICR_DEASSERT = 0x00000000;
        private static int ICR_ASSERT = 0x00004000;

        private static int ICR_EDGE = 0x00000000;
        private static int ICR_LEVEL = 0x00008000;

        private static int ICR_NO_SHORTHAND = 0x00000000;
        private static int ICR_SELF = 0x00040000;
        private static int ICR_ALL_INCLUDING_SELF = 0x00080000;
        private static int ICR_ALL_EXCLUDING_SELF = 0x000c0000;

        private static int ICR_DESTINATION_SHIFT = 24;

        static uint In(uint reg)
        {
            return MMIO.In32((uint*)(ACPI.MADT->LocalAPICAddress + reg));
        }

        static void Out(uint reg, uint data)
        {
            MMIO.Out32((uint*)(ACPI.MADT->LocalAPICAddress + reg), data);
        }

        public static void EndOfInterrupt()
        {
            Out((uint)LAPIC_EOI, 0);
        }

        public static void Initialize(uint CPUForIRQHandlingIndex = 0)
        {
            //Enable All Interrupts
            Out((uint)LAPIC_TPR, 0);

            // Logical Destination Mode
            Out((uint)LAPIC_DFR, 0xffffffff);   // Flat mode
            Out((uint)LAPIC_LDR, CPUForIRQHandlingIndex << 24);   // All cpus use logical id CPUforIRQHandlingIndex

            // Configure Spurious Interrupt Vector Register
            Out((uint)LAPIC_SVR, 0x100 | 0xff);

            Console.WriteLine("[Local APIC] Local APIC initialized");
        }

        public static uint GetId()
        {
            return In((uint)LAPIC_ID) >> 24;
        }

        public static void SendInit(uint apic_id)
        {
            Out((uint)LAPIC_ICRHI, apic_id << ICR_DESTINATION_SHIFT);
            Out((uint)LAPIC_ICRLO, (uint)(ICR_INIT | ICR_PHYSICAL
                | ICR_ASSERT | ICR_EDGE | ICR_NO_SHORTHAND));

            while ((In((uint)LAPIC_ICRLO) & ICR_SEND_PENDING) != 0) ;
        }

        public static void SendStartup(uint apic_id, uint vector)
        {
            Out((uint)LAPIC_ICRHI, apic_id << ICR_DESTINATION_SHIFT);
            Out((uint)LAPIC_ICRLO, vector | (uint)ICR_STARTUP
                | (uint)ICR_PHYSICAL | (uint)ICR_ASSERT | (uint)ICR_EDGE | (uint)ICR_NO_SHORTHAND);

            while ((In((uint)LAPIC_ICRLO) & ICR_SEND_PENDING) != 0) ;
        }
    }
}