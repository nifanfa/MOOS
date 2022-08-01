using MOOS.Driver;
using MOOS.Misc;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MOOS
{
    public static unsafe class SMP
    {
        //https://wiki.osdev.org/Memory_Map_(x86)
        public const ulong APMain = 0x50000;
        public const ulong Stacks = 0x50008;
        public const ulong SharedGDT = 0x50016;
        public const ulong SharedIDT = 0x50024;
        public const ulong SharedPageTable = 0x51000;
        public const ulong Trampoline = 0x60000;

        public static ulong NumActivedProcessors = 0;

        public static int NumCPU { get => ACPI.LocalAPIC_CPUIDs.Count; }

        public static uint ThisCPU => LocalAPIC.GetId();


        //Method for other CPU cores
        //GDT, IDT, PageTable has been configured in Trampoline. so we don't need to set it here
        public static void ApplicationProcessorMain(int Core)
        {
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