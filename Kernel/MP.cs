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
    }
}
