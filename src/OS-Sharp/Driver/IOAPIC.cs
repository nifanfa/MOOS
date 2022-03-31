/*
* Copyright (c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
*/

using OS_Sharp.Misc;

namespace OS_Sharp.Driver
{
    public static unsafe class IOAPIC
    {
        private const int IOREGSEL = 0x00;
        private const int IOWIN = 0x10;

        private const int IOAPICID = 0x00;
        private const int IOAPICVER = 0x01;
        private const int IOAPICARB = 0x02;
        private const int IOREDTBL = 0x10;

        public static bool Initialize()
        {
            if (ACPI.IO_APIC == null)
            {
                return false;
            }
            uint value = In(IOAPICVER);
            uint count = ((value >> 16) & 0xFF) + 1;

            //Disable All Entries
            for (uint i = 0; i < count; ++i)
            {
                SetEntry((byte)i, 1 << 16);
            }
            return true;
        }

        public static uint In(byte reg)
        {
            MMIO.Out32((uint*)(ACPI.IO_APIC->IOApicAddress + IOREGSEL), reg);
            return MMIO.In32((uint*)(ACPI.IO_APIC->IOApicAddress + IOWIN));
        }

        public static void Out(byte reg, uint value)
        {
            MMIO.Out32((uint*)(ACPI.IO_APIC->IOApicAddress + IOREGSEL), reg);
            MMIO.Out32((uint*)(ACPI.IO_APIC->IOApicAddress + IOWIN), value);
        }

        public static void SetEntry(byte index, ulong data)
        {
            Out((byte)(IOREDTBL + index * 2), (uint)data);
            Out((byte)(IOREDTBL + index * 2 + 1), (uint)(data >> 32));
        }

        public static void SetEntry(uint irq)
        {
            IOAPIC.SetEntry((byte)ACPI.RemapIRQ(irq - 0x20), irq);
        }
    }
}
