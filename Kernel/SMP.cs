using MOOS.Driver;
using MOOS.Misc;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MOOS
{
    public static unsafe class SMP
    {
        public const ulong BaseAddress = 0x50000;

        //https://wiki.osdev.org/Memory_Map_(x86)
        public const ulong APMain = BaseAddress + 0x0;
        public const ulong Stacks = BaseAddress + 0x8;
        public const ulong SharedGDT = BaseAddress + 0x16;
        public const ulong SharedIDT = BaseAddress + 0x24;
        public const ulong SharedPageTable = BaseAddress + 0x1000;
        public const ulong Trampoline = BaseAddress + 0x10000;

        public static ulong NumActivedProcessors = 0;

        private const int StackSizeForEachCPU = 1048576;

        public static int NumCPU { get => ACPI.LocalAPIC_CPUIDs.Count; }

        public static uint ThisCPU => LocalAPIC.GetId();


        //Method for other CPU cores
        //GDT, IDT, PageTable has been configured in Trampoline. so we don't need to set it here
        public static void ApplicationProcessorMain(int Core)
        {
            *(ulong*)Stacks += StackSizeForEachCPU;
            SSE.enable_sse();
            LocalAPIC.Initialize();
            LocalAPICTimer.StartTimer(1000, 0x20);
            ThreadPool.Initialize();
            NumActivedProcessors++;
            for (; ; ) Native.Hlt();
        }

        public static void Initialize(uint trampoline)
        {
            if (ThisCPU != 0) Panic.Error("Error: Bootstrap CPU is not CPU 0");

            NumActivedProcessors = 1;

            ulong* apMain = (ulong*)APMain;
            *apMain = (ulong)(delegate*<int,void>)&ApplicationProcessorMain;

            ulong* stacks = (ulong*)Stacks;
            *stacks = (ulong)Allocator.Allocate((ulong)(ACPI.LocalAPIC_CPUIDs.Count * StackSizeForEachCPU));
            *stacks += StackSizeForEachCPU;

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

            Console.WriteLine("[SMP] Starting all CPUs");
            for (int i = 0; i < NumCPU; ++i)
            {
                uint id = ACPI.LocalAPIC_CPUIDs[i]; 
                if (id != ThisCPU)
                {
                    ulong last = NumActivedProcessors;
                    LocalAPIC.SendInit(id); 
                    LocalAPIC.SendStartup(id, (trampoline >> 12));
                    while (last == NumActivedProcessors) Native.Nop();
                }
            }
            Console.WriteLine($"[SMP] {NumCPU} CPUs started");
        }
    }
}