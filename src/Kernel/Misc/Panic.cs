namespace Kernel.Misc
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
            for (; ; );
        }
    }
}
