using MOOS.Driver;
using System;

namespace MOOS.Misc
{
    public static class Panic
    {
        public static void Error(string msg,bool skippable = false)
        {
            //Kill all CPUs
            LocalAPIC.SendAllInterrupt(0xFD);
            IDT.Disable();
            Framebuffer.TripleBuffered = false;

            ConsoleColor color = Console.ForegroundColor;

            Console.ForegroundColor = System.ConsoleColor.Red;
            Console.Write("PANIC: ");
            Console.WriteLine(msg);
            Console.WriteLine("All CPU Halted Now!");

            Console.ForegroundColor = color;

            if (!skippable)
            {
                Framebuffer.Update();
                for (; ; );
            }
        }
    }
}