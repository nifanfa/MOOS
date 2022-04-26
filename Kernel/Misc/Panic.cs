/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
namespace Kernel.Misc
{
    public static class Panic
    {
        public static void Error(string msg,bool skippable = false) 
        {
            IDT.Disable();
            Framebuffer.TripleBuffered = false;
            Console.Write("PANIC: ");
            Console.WriteLine(msg);
            if(!skippable)
                for (; ; );
        }
    }
}
