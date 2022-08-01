using MOOS.Driver;

namespace MOOS.Misc
{
	public static class Panic
	{
		public static void Error(string msg, bool skippable = false)
		{
			//Kill all CPUs
			LocalAPIC.SendAllInterrupt(0xFD);
			GC.Collect();
			GC.AllowCollect = false;
			IDT.Disable();
			Framebuffer.DoubleBuffered = false;
			Console.BackgroundColor = System.ConsoleColor.Blue;
			Console.Clear();
			Console.Write("A problem has been detected and MOOS has been shut down to prevent damage to your computer.\n\n");
			Console.Write($"{msg}\n\n");
			Console.Write("If this is the first time you've seen this Stop error screen, restart\n");
			Console.Write("your computer. If this screen appears again, follow these steps:\n\n");
			Console.Write("Check to make sure any new hardware or software is properly installed.\n");
			Console.Write("If this is a new installation, ask your hardware or software manufacturer\n");
			Console.Write("for any MOOS updates you might need.\n\n");
			Console.Write("If problems continue, disable or remove any newly installed hardware or\n");
			Console.Write("software. Disable BIOS memory options such as caching or shadowing. If you\n");
			Console.Write("need to use Safe Mode to remove or disable components, restart your computer,\n");
			Console.Write("press F8, select Advanced Startup options, and then select Safe Mode.\n\n");
			Console.Write("Technical information:\n\n");

			if (!skippable)
			{
				Framebuffer.Update();
				for (; ; )
				{
					;
				}
			}
		}
	}
}