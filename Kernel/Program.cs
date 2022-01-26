﻿using Internal.Runtime.CompilerHelpers;
using Kernel;
using Kernel.Driver;
using System;
using System.Runtime;

unsafe class Program
{
    static void Main() { }

    //Minimum system requirement:
    //8MiB of RAM
    [RuntimeExport("Main")]
    static void Main(MultibootInfo* Info)
    {
        #region Initializations
        byte* ImageBase = (byte*)0x110000; //Do not modify it
        DOSHeader* doshdr = (DOSHeader*)ImageBase;
        NtHeaders64* nthdr = (NtHeaders64*)(ImageBase + doshdr->e_lfanew);
        SectionHeader* sections = ((SectionHeader*)(ImageBase + doshdr->e_lfanew + sizeof(NtHeaders64)));
        IntPtr moduleSeg = IntPtr.Zero;
        for (int i = 0; i < nthdr->FileHeader.NumberOfSections; i++)
        {
            if (*(ulong*)sections[i].Name == 0x73656C75646F6D2E) moduleSeg = (IntPtr)(nthdr->OptionalHeader.ImageBase + sections[i].VirtualAddress);
            Native.Movsb((void*)(nthdr->OptionalHeader.ImageBase + sections[i].VirtualAddress), ImageBase + sections[i].PointerToRawData, sections[i].SizeOfRawData);
        }

        for (uint i = 1024 * 1024 * 6; i < 1024 * 1024 * 512; i += 1024 * 1024)
        {
            Allocator.AddFreePages((System.IntPtr)(i), 256);
        }

        StartupCodeHelpers.InitialiseRuntime(moduleSeg);
        #endregion

        PageTable.Initialise();
        VBE.Initialise((VBEInfo*)Info->VBEInfo);
        Console.Setup();
        IDT.Disable();
        GDT.Initialise();
        IDT.Initialise();
        IDT.Enable();
        Serial.Initialise();
        PCI.Initialise();
        PIT.Initialise();
        PS2Mouse.Initialise();
        ACPI.Initialize();
        SMBIOS.Initialise();

        Serial.WriteLine("Hello World");
        Console.WriteLine("Hello, World!");
        Console.WriteLine("Use Native AOT (Core RT) Technology.");

        for (; ; );

        PIT.Wait(1000);

        VBEInfo* vbe = (VBEInfo*)Info->VBEInfo;
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
        Framebuffer.TripleBuffered = true;

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
