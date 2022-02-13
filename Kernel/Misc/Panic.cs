namespace Kernel.Misc
{
    public static class Panic
    {
        public static void Error(string msg) 
        {
            Console.Write("PANIC: ");
            Console.WriteLine(msg);
            IDT.Disable();
            for (; ; );
        }
    }
}
