using MOOS.Driver;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MOOS
{
    public static unsafe class SMP
    {
        public const ulong NumActivedProcessors = 0x80000;
        public const ulong APMain = 0x80008;
        public const ulong Stacks = 0x800016;
        public const ulong SharedGDT = 0x800024;
        public const ulong SharedIDT = 0x800032;
        public const ulong SharedPageTable = 0x81000;
        public const ulong Trampoline = 0x90000;

        public static Queue<ulong> WorkGroups;

        public static void RunOnAnyCPU(delegate*<void> method) => WorkGroups.Enqueue((ulong)method);

        public static bool Untakable;

        public static int NumFreeCPU;
        public static int NumCPU { get => ACPI.LocalAPIC_CPUIDs.Count; }

        public static delegate*<void> Take() 
        {
            NumFreeCPU++;
            while (WorkGroups.Length == 0 || Untakable) ;
            Untakable = true;
            var addr = (delegate*<void>)WorkGroups.Dequeue();
            Untakable = false;
            NumFreeCPU--;
            return addr; 
        }

        public static uint ThisCPU => LocalAPIC.GetId();


        //Method for other CPU cores
        //GDT, IDT, PageTable has been configured in Trampoline. so we don't need to set it here
        public static void ApplicationProcessorMain(int Core)
        {
            Native.Cli();
            SSE.enable_sse();
            //Console.WriteLine("Hello from Application Processor");
            for (; ; ) SMP.Take()();
        }

        public static void Initialize(uint trampoline)
        {
            ushort* activedProcessor = (ushort*)NumActivedProcessors;
            *activedProcessor = 1;

            ulong* apMain = (ulong*)APMain;
            *apMain = (ulong)(delegate*<int,void>)&ApplicationProcessorMain;

            ulong* stacks = (ulong*)Stacks;
            *stacks = (ulong)Allocator.Allocate((ulong)(ACPI.LocalAPIC_CPUIDs.Count * 1048576));

            fixed(GDT.GDTDescriptor* gdt = &GDT.gdtr) 
            {
                ulong* sgdt = (ulong*)SharedGDT;
                *sgdt = (ulong)gdt;
            }

            fixed (IDT.IDTDescriptor* idt = &IDT.idtr)
            {
                ulong* sidt = (ulong*)SharedIDT;
                *sidt = (ulong)idt;
            }

            Untakable = false;
            WorkGroups = new Queue<ulong>();
            NumFreeCPU = 0;

            for (int i = 0; i < NumCPU; ++i)
            {
                uint id = ACPI.LocalAPIC_CPUIDs[i]; 
                Console.WriteLine($"Starting CPU{id.ToString()}");
                if (id != ThisCPU)
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
