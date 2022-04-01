/*
* Copyright (c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
*/

namespace OS_Sharp.Misc
{
    public static class Panic
    {
        public static void Error(string msg)
        {
            Framebuffer.TripleBuffered = false;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("PANIC: ");
            Console.WriteLine(msg);
            IDT.Disable();
            for (; ; )
            {
                ;
            }
        }
    }
}
