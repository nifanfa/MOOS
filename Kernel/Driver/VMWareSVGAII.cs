using MOOS;
using System;

namespace MOOS.Driver
{
    [Obsolete("This driver does not support unbuffered mode")]
    public unsafe class VMWareSVGAII
    {
        public enum Register : ushort
        {
            ID = 0,
            Enable = 1,
            Width = 2,
            Height = 3,
            BitsPerPixel = 7,
            FrameBufferStart = 13,
            VRamSize = 15,
            MemStart = 18,
            MemSize = 19,
            ConfigDone = 20,
            Sync = 21,
            Busy = 22,
            FifoNumRegisters = 293
        }

        private enum ID : uint
        {
            Magic = 0x900000,
            V2 = (Magic << 8) | 2,
        }

        public enum FIFO : uint
        {
            Min = 0,
            Max = 4,
            NextCmd = 8,
            Stop = 12
        }

        private enum FIFOCommand
        {
            Update = 1
        }

        private enum IOPortOffset : byte
        {
            Index = 0,
            Value = 1,
        }

        private ushort IndexPort;
        private ushort ValuePort;
        public uint* Video_Memory;
        private byte* FIFO_Memory;
        private PCIDevice device;
        public uint height;
        public uint width;
        public uint depth;


        public VMWareSVGAII()
        {
            device = PCI.GetDevice(0x15AD, 0x0405);
            device.WriteRegister(0x04, 0x04 | 0x02 | 0x01);
            uint basePort = device.Bar0 & ~(0xFU);
            IndexPort = (ushort)(basePort + (uint)IOPortOffset.Index);
            ValuePort = (ushort)(basePort + (uint)IOPortOffset.Value);
            WriteRegister(Register.ID, (uint)ID.V2);
            if (ReadRegister(Register.ID) != (uint)ID.V2)
                return;
            Video_Memory = (uint*)ReadRegister(Register.FrameBufferStart);
            InitializeFIFO();
        }

        protected void InitializeFIFO()
        {
            FIFO_Memory = (byte*)ReadRegister(Register.MemStart);
            *(uint*)&FIFO_Memory[(uint)FIFO.Min] = (uint)Register.FifoNumRegisters * 4;
            *(uint*)&FIFO_Memory[(uint)FIFO.Max] = (uint)ReadRegister(Register.MemSize);
            *(uint*)&FIFO_Memory[(uint)FIFO.NextCmd] = (uint)FIFO.Min;
            *(uint*)&FIFO_Memory[(uint)FIFO.Stop] = *(uint*)&FIFO_Memory[(uint)FIFO.Min];
            WriteRegister(Register.ConfigDone, 1);
        }

        public void SetMode(uint width, uint height, uint depth = 32)
        {
            Disable();
            this.depth = (depth / 8);
            this.width = width;
            this.height = height;
            WriteRegister(Register.Width, width);
            WriteRegister(Register.Height, height);
            WriteRegister(Register.BitsPerPixel, depth);
            Enable();
            InitializeFIFO();
        }

        protected void WriteRegister(Register register, uint value)
        {
            Native.Out32(IndexPort, (uint)register);
            Native.Out32(ValuePort, value);
        }

        protected uint ReadRegister(Register register)
        {
            Native.Out32(IndexPort, (uint)register);
            return Native.In32(ValuePort);
        }

        protected void SetFIFO(FIFO cmd, uint value)
        {
            *(uint*)&FIFO_Memory[(uint)cmd] = value;
        }

        uint nextcmd = 1172;

        public void Update()
        {
            if (nextcmd == 1212) { nextcmd = 1172; }

            SetFIFO((FIFO)(nextcmd), (uint)FIFOCommand.Update);
            SetFIFO(FIFO.NextCmd, nextcmd + 4);
            nextcmd += 4;

            SetFIFO((FIFO)(nextcmd), 0);
            SetFIFO(FIFO.NextCmd, nextcmd + 4);
            nextcmd += 4;

            SetFIFO((FIFO)(nextcmd), 0);
            SetFIFO(FIFO.NextCmd, nextcmd + 4);
            nextcmd += 4;

            SetFIFO((FIFO)(nextcmd), width);
            SetFIFO(FIFO.NextCmd, nextcmd + 4);
            nextcmd += 4;

            SetFIFO((FIFO)(nextcmd), height);
            SetFIFO(FIFO.NextCmd, nextcmd + 4);
            nextcmd += 4;
        }

        public void Enable()
        {
            WriteRegister(Register.Enable, 1);
        }

        public void Disable()
        {
            WriteRegister(Register.Enable, 0);
        }
    }
}