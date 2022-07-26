using MOOS.Driver;
using MOOS.Graph;

namespace MOOS.Misc
{
	public static class Panic
	{
		public static void Error(string msg, bool skippable = false)
		{
			//Kill all CPUs
			LocalAPIC.SendAllInterrupt(0xFD);
			IDT.Disable();
			Framebuffer.TripleBuffered = false;
			Console.Write("PANIC: ");
			Console.WriteLine(msg);
			Console.WriteLine("All CPU Halted Now!");
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