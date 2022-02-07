using Internal.Runtime.CompilerHelpers;
using Kernel;
using System;
using System.Runtime;
using System.Runtime.InteropServices;

unsafe class Program
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct BootInfo
    {
        public ulong Framebuffer;
        public uint Width;
        public uint Height;
        public ulong MemoryStart;
        public ulong NumPages;
        public ulong Modules;
    }

    static void Main() { }

    [RuntimeExport("Main")]
    static void Main(BootInfo* bootinfo)
    {
        Allocator.Initialize((IntPtr)bootinfo->MemoryStart);

        StartupCodeHelpers.InitializeModules((IntPtr)bootinfo->Modules);

        ASC16.Initialise();

        Framebuffer.VideoMemory = (uint*)bootinfo->Framebuffer;
        Framebuffer.SetVideoMode(bootinfo->Width, bootinfo->Height);
        Framebuffer.Clear(0x0);

        Console.Setup();
        Console.WriteLine("Hello ARM!");
        for (; ; );
    }
}
