using Kernel.Driver;
using System.Runtime.InteropServices;

namespace Kernel
{
    public static unsafe class SMP
    {
        public static void Initialize(uint trampoline)
        {
            ushort* activedProcessor = (ushort*)0x6000;
            *activedProcessor = 1;

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
