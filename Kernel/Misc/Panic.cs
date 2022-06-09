/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
using MOOS.Driver;

namespace MOOS.Misc
{
    public static class Panic
    {
        public static int CPUHalted;

        public static void Error(string msg,bool skippable = false)
        {
            //Kill all CPUs
            Panic.CPUHalted = 0;
            LocalAPIC.SendAllInterrupt(0xFD);
            while (Panic.CPUHalted != (SMP.NumCPU - 1)) Native._pause();

            IDT.Disable();
            Framebuffer.TripleBuffered = false;
            Console.Write("PANIC: ");
            Console.WriteLine(msg);
            Console.WriteLine("All CPU Halted Now!");
            if (!skippable)
            {
                Framebuffer.Update();
                for (; ; );
            }
        }
    }
}
