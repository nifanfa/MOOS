namespace Cosmos.HAL
{
    internal class IOPort
    {
        private ushort Port;

        public IOPort(ushort port)
        {
            this.Port = port;
        }

        public uint DWord
        {
            get => Native.In32(Port);
            set => Native.Out32(Port, value);
        }

        public ushort Word
        {
            get => Native.In16(Port);
            set => Native.Out16(Port, value);
        }

        public byte Byte
        {
            get => Native.In8(Port);
            set => Native.Out8(Port, value);
        }
    }
}
