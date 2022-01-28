using Internal.Runtime.CompilerHelpers;
using Kernel;
using Kernel.Driver;
using Kernel.NET;
using System;
using System.Runtime;
using System.Net;
using Kernel.GUI;
using System.Windows.Forms;

unsafe class Program
{
    static void Main() { }

    /*
     * Minimum system requirement:
     * 64MiB of RAM
     * Memory Map:
     * 1MiB  - ???    -> Load data
     * 32MiB - 48MiB  -> System
     * 48MiB - ∞     -> Free to use
     */
    [RuntimeExport("Main")]
    static void Main(MultibootInfo* Info, byte* RAW, IntPtr Modules)
    {
        #region Initializations
        if (Modules == IntPtr.Zero) 
        {
            DOSHeader* doshdr = (DOSHeader*)RAW;
            NtHeaders64* nthdr = (NtHeaders64*)(RAW + doshdr->e_lfanew);
            SectionHeader* sections = ((SectionHeader*)(RAW + doshdr->e_lfanew + sizeof(NtHeaders64)));
            IntPtr moduleSeg = IntPtr.Zero;
            for (int i = 0; i < nthdr->FileHeader.NumberOfSections; i++)
            {
                if (*(ulong*)sections[i].Name == 0x73656C75646F6D2E) moduleSeg = (IntPtr)(nthdr->OptionalHeader.ImageBase + sections[i].VirtualAddress);
                Native.Movsb((void*)(nthdr->OptionalHeader.ImageBase + sections[i].VirtualAddress), RAW + sections[i].PointerToRawData, sections[i].SizeOfRawData);
            }
            delegate*<MultibootInfo*, byte*, IntPtr, void> p = (delegate*<MultibootInfo*, byte*, IntPtr, void>)(nthdr->OptionalHeader.ImageBase + nthdr->OptionalHeader.AddressOfEntryPoint);
            p(Info, null, moduleSeg);
            return;
        }
        
        for (uint i = 1024 * 1024 * 6; i < 1024 * 1024 * 512; i += 4096)
        {
            liballoc.AddFreePages((IntPtr)i, 1);
        }

        StartupCodeHelpers.InitializeRuntime(Modules);
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

        /*
        ARP.Initialise();
        Network.Initialise(IPAddress.Parse(192, 168, 137, 188), IPAddress.Parse(192, 168, 137, 1));
        RTL8139.Initialise();
        ARP.Require(Network.Gateway);

        for (; ; );
        */

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

        Form.Initialize();
        new Form(100, 100, 300, 200);

        for (; ; )
        {
            Framebuffer.Clear(0xFF323232);
            Form.UpdateAll();
            /*
            ASC16.DrawString("FPS: ", 10, 10, 0xFFFFFFFF);
            ASC16.DrawString(((ulong)FPSMeter.FPS).ToString(), 42, 10, 0xFFFFFFFF);
            */
            DrawCursor(cursor, Control.MousePosition.X, Control.MousePosition.Y);
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
