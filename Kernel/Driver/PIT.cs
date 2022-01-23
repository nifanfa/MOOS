namespace Kernel
{
    public class PIT
    {
        public static ulong Tick = 0;

        public static void Initialise()
        {
            ushort timerCount = 1193182 / 1000;

            Native.Out8(0x43, 0x36);
            Native.Out8(0x40, (byte)(timerCount & 0xFF));
            Native.Out8(0x40, (byte)((timerCount & 0xFF00) >> 8));

            PIC.ClearMask(0x20);
        }

        internal static void OnInterrupt()
        {
            Tick = Tick + 1;
        }

        public static void Wait(ulong millisecond)
        {
            ulong T = Tick;
            while (Tick < (T + millisecond)) Native.Hlt();
        }
    }
}
