using Kernel.Driver;
using System.Runtime.InteropServices;

namespace Kernel
{
    public static unsafe class SMP
    {
        public const ulong NumActivedProcessors = 0x80000;
        public const ulong APMain = 0x80008;
        public const ulong Stacks = 0x800016;
        public const ulong SharedPageTable = 0x81000;
        public const ulong ReommendedTrampoline = 0x90000;

        public static void Initialize(uint trampoline)
        {
            ushort* activedProcessor = (ushort*)NumActivedProcessors;
            *activedProcessor = 1;

            ulong* apMain = (ulong*)APMain;
            *apMain = (ulong)(delegate*<int,void>)&Program.APMain;

            ulong* stacks = (ulong*)Stacks;
            *stacks = (ulong)Allocator.Allocate((ulong)(ACPI.LocalAPIC_CPUIDs.Count * 32768));

            int NumCPU = ACPI.LocalAPIC_CPUIDs.Count;
            uint LocalID = LocalAPIC.GetId();
            for (int i = 0; i < NumCPU; ++i)
            {
                uint id = ACPI.LocalAPIC_CPUIDs[i]; 
                Console.WriteLine($"Starting CPU{id.ToString()}");
                if (id != LocalID)
                {
                    LocalAPIC.SendInit(id); 
                    LocalAPIC.SendStartup(id, (trampoline >> 12));
                }
            }
            while (*activedProcessor != NumCPU);
            Console.WriteLine("All CPUs started");
        }
    }
}
