namespace MOOS
{
    public class Serial
    {
        public const ushort COM1 = 0x3F8;

        public static void Initialise()
        {
            Native.Out8(COM1 + 1, 0x00);    // Disable all interrupts
            Native.Out8(COM1 + 3, 0x80);    // Enable DLAB (set baud rate divisor)
            Native.Out8(COM1 + 0, 0x03);    // Set divisor to 3 (lo byte) 38400 baud
            Native.Out8(COM1 + 1, 0x00);    //                  (hi byte)
            Native.Out8(COM1 + 3, 0x03);    // 8 bits, no parity, one stop bit
            Native.Out8(COM1 + 2, 0xC7);    // Enable FIFO, clear them, with 14-byte threshold
            Native.Out8(COM1 + 4, 0x0B);    // IRQs enabled, RTS/DSR set
        }

        public static void Write(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                Write(s[i]);
            }
            s.Dispose();
        }

        public static void Write(char c)
        {
            while ((Native.In8(COM1 + 5) & 0x20) == 0) { }
            Native.Out8(COM1, (byte)(c & 0xFF));
        }

        public static void WriteLine(string s)
        {
            Write(s);
            WriteLine();
            s.Dispose();
        }

        public static void WriteLine()
        {
            Write('\r');
            Write('\n');
        }
    }
}