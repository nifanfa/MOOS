using Kernel.Driver;
using System.Runtime.InteropServices;

namespace Kernel
{
    public static unsafe class MP
    {
        public struct MPPointer
        {
            public uint Signature;
            public uint MPConfig;
            public byte Length;
            public byte Version;
            public byte Checksum;
            public byte Feature1;
            public uint Feature2_3;
        }
        
        public struct MPConfigurationTable
        {
        	public uint Signature;
        	public ushort Length;
        	public byte Revision;
        	public byte Checksum;
        	public ulong OEM;
        	public fixed byte ProductID[12];
        	public uint OEMTablePointer;
        	public ushort OEMTableSize;
        	public ushort EntryCount;
        	public uint LAPIC;
        	public ushort ExtendedLength;
        	public byte ExtendedChecksum;
        }
        
        public struct MPEntry
        {
        	public fixed byte Processor[20];
        	public ulong Bus;
        	public ulong IOAPIC;
        	public ulong IOInterruptAssignment;
        	public ulong LocalInterruptAssignment;
        }

        /*
        public static void Initialize()
        {
            uint* ptr = (uint*)0xF0000;
            while(*ptr != 0x5f504d5f && ptr < (uint*)0xfffff)
            {
                ptr++;
            }
            Console.WriteLine($"ptr:{((uint)ptr).ToString("x2")}");
            Console.ReadKey();
        }
        */

        public static int NumCPU;

        public static void Initialize(uint trampoline)
        {
            ushort* activedProcessor = (ushort*)0x6000;

            Console.WriteLine("Waking Up All CPU(s)");

            NumCPU = ACPI.LocalAPIC_CPUIDs.Count;
            uint LocalID = LocalAPIC.GetId();
            for (int i = 0; i < NumCPU; ++i)
            {
                uint APICID = ACPI.LocalAPIC_CPUIDs[i];
                if (APICID != LocalID) LocalAPIC.SendInit(APICID);
            }

            PIT.Wait(10);

            for (int i = 0; i < NumCPU; ++i)
            {
                uint apicId = ACPI.LocalAPIC_CPUIDs[i];
                if (apicId != LocalID)
                {
                    LocalAPIC.SendStartup(apicId, (trampoline >> 12));
                }
            }
            PIT.Wait(100);
            Console.WriteLine("Waiting for CPUs");
            while (*activedProcessor != NumCPU) Native.Hlt();
            Console.WriteLine("All CPU(s) Actived");
        }
    }
}
