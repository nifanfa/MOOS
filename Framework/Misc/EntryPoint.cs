using System;
using System.Runtime;
using System.Runtime.InteropServices;
using Internal.Runtime.CompilerHelpers;
using MOOS.Driver;
using MOOS.FS;

namespace MOOS.Misc
{
	internal static unsafe class EntryPoint
	{
		[RuntimeExport("Entry")]
		public static void Entry(MultibootInfo* Info, IntPtr Modules, IntPtr Trampoline)
		{
			Allocator.Initialize((IntPtr)0x20000000);

			StartupCodeHelpers.InitializeModules(Modules);

#if HasGC
			GC.AllowCollect = false;
#endif

			PageTable.Initialise();

			ASC16.Initialize();

			VBEInfo* info = (VBEInfo*)Info->VBEInfo;
			if (info->PhysBase != 0)
			{
				Framebuffer.Initialize(info->ScreenWidth, info->ScreenHeight, (uint*)info->PhysBase);
				Framebuffer.Graphics.Clear(0x0);
			} else
			{
				for (; ; )
				{
					Native.Hlt();
				}
			}

			Console.Setup();
			IDT.Disable();
			GDT.Initialise();
			IDT.Initialize();
			Interrupts.Initialize();
			IDT.Enable();

			SSE.enable_sse();
			//AVX.init_avx();

			ACPI.Initialize();
#if UseAPIC
			PIC.Disable();
			LocalAPIC.Initialize();
			IOAPIC.Initialize();
#else
	PIC.Enable();
#endif
			Timer.Initialize();

			Keyboard.Initialize();

			Serial.Initialise();

			PS2Controller.Initialize();

			SMBIOS.Initialise();

			PCI.Initialise();

			IDE.Initialize();
			SATA.Initialize();

			ThreadPool.Initialize();

			Console.WriteLineInfo("SMP", $"Trampoline: 0x{((ulong)Trampoline).ToString("x2")}");
			Native.Movsb((byte*)SMP.Trampoline, (byte*)Trampoline, 512);

			SMP.Initialize((uint)SMP.Trampoline);

#if HasGC
			GC.AllowCollect = true;
#endif

			//Only fixed size vhds are supported!
			Console.WriteInfo("Initrd", "Initrd: 0x");
			Console.WriteLine(Info->Mods[0].ToString("x2"));
			Console.WriteLineInfo("Initrd", "Initializing Ramdisk");
			_ = new Ramdisk((IntPtr)Info->Mods[0]);
			_ = new FATFS();

			Hub.Initialize();
			HID.Initialize();
			EHCI.Initialize();

			KMain();
		}

		[DllImport("*")]
		public static extern void KMain();
	}
}