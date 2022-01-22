using Internal.Runtime.CompilerHelpers;
using Kernel;
using System;
using System.Runtime;

unsafe class Program
{
    public const int ImageBase = 0x110000; //Do not modify

    static void Main() { }

    [RuntimeExport("Main")]
    static void Main(MultibootInfo* info)
    {
        #region Initializations
        DOSHeader* doshdr = (DOSHeader*)ImageBase;
        NtHeaders64* nthdr = (NtHeaders64*)(ImageBase + doshdr->e_lfanew);
        SectionHeader* sections = ((SectionHeader*)(ImageBase + doshdr->e_lfanew + sizeof(NtHeaders64)));
        IntPtr moduleSeg = IntPtr.Zero;
        for (int i = 0; i < nthdr->FileHeader.NumberOfSections; i++)
        {
            if (*(ulong*)sections[i].Name == 0x73656C75646F6D2E) moduleSeg = (IntPtr)(ImageBase + sections[i].VirtualAddress);
            Native.Movsb((void*)(ImageBase + sections[i].VirtualAddress), (void*)(ImageBase + sections[i].PointerToRawData), sections[i].SizeOfRawData);
        }

        //                 10MiB                 512MiB                1MiB
        for (uint i = 1024 * 1024 * 10; i < 1024 * 1024 * 512; i += 1024 * 1024)
        {
            //                                      1MiB / 4KiB
            Allocator.AddFreePages((System.IntPtr)(i), 256);
        }

        StartupCodeHelpers.InitialiseRuntime(moduleSeg);
        #endregion

        Console.Setup();
        IDT.Disable();
        GDT.Initialise();
        IDT.Initialise();
        IDT.Enable();
        Serial.Initialise();
        PageTable.Initialise();
        PCI.Initialise();
        PIT.Initialise();
        PS2Mouse.Initialise();
        ASC16.Initialise();

        Serial.WriteLine("Hello World");
        Console.WriteLine("Hello, World!");
        Console.WriteLine("Use Native AOT (Core RT) Technology.");

        /*
        VBEInfo* vbe = (VBEInfo*)info->VBEInfo;
        if (vbe->PhysBase != 0)
        {
            Framebuffer.VideoMemory = (uint*)vbe->PhysBase;
            Framebuffer.SetVideoMode(vbe->ScreenWidth, vbe->ScreenHeight);
        }
        else 
        {
            Framebuffer.Setup();
            Framebuffer.SetVideoMode(800, 600);
        }

        int[] cursor = new int[]
            {
                1,0,0,0,0,0,0,0,0,0,0,0,
                1,1,0,0,0,0,0,0,0,0,0,0,
                1,2,1,0,0,0,0,0,0,0,0,0,
                1,2,2,1,0,0,0,0,0,0,0,0,
                1,2,2,2,1,0,0,0,0,0,0,0,
                1,2,2,2,2,1,0,0,0,0,0,0,
                1,2,2,2,2,2,1,0,0,0,0,0,
                1,2,2,2,2,2,2,1,0,0,0,0,
                1,2,2,2,2,2,2,2,1,0,0,0,
                1,2,2,2,2,2,2,2,2,1,0,0,
                1,2,2,2,2,2,2,2,2,2,1,0,
                1,2,2,2,2,2,2,2,2,2,2,1,
                1,2,2,2,2,2,2,1,1,1,1,1,
                1,2,2,2,1,2,2,1,0,0,0,0,
                1,2,2,1,0,1,2,2,1,0,0,0,
                1,2,1,0,0,1,2,2,1,0,0,0,
                1,1,0,0,0,0,1,2,2,1,0,0,
                0,0,0,0,0,0,1,2,2,1,0,0,
                0,0,0,0,0,0,0,1,2,2,1,0,
                0,0,0,0,0,0,0,1,2,2,1,0,
                0,0,0,0,0,0,0,0,1,1,0,0
            };

        for (; ; )
        {
            Framebuffer.Clear(0x0);
            ASC16.DrawString("FPS: ", 10, 10, 0xFFFFFFFF);
            ASC16.DrawString(((ulong)FPSMeter.FPS).ToString(), 42, 10, 0xFFFFFFFF);
            DrawCursor(cursor, PS2Mouse.X, PS2Mouse.Y);
            Framebuffer.Update();
            FPSMeter.Update();
        }
        */
    }

    private static void DrawCursor(int[] cursor, int x, int y)
    {
        for (int h = 0; h < 21; h++)
            for (int w = 0; w < 12; w++)
            {
                if (cursor[h * 12 + w] == 1)
                    Framebuffer.DrawPoint(w + x, h + y, 0xFFFFFFFF);

                if (cursor[h * 12 + w] == 2)
                    Framebuffer.DrawPoint(w + x, h + y, 0x0);
            }
    }
}
