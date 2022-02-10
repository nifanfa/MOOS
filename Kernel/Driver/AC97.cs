using Kernel;
using System;
using System.Runtime.InteropServices;
using static Native;

namespace Kernel
{
    public unsafe class AC97
    {
        private static uint NAM, NABM;

        static BufferDescriptor* BufferDescriptors;

        public static unsafe void Initialize()
        {
            PCIDevice device = PCI.GetDevice(0x8086, 0x2415);

            if (device == null) return;

            Console.WriteLine("Intel 82801AA AC97 Audio Controller Found");

            device.WriteRegister(0x04, 0x04 | 0x02 | 0x01);

            NAM = device.Bar0 & ~(0xFU);
            NABM = device.Bar1 & ~(0xFU);

            Out8((ushort)(NABM + 0x2C), 0x2);

            Out32((ushort)(NAM + 0x00), 0x6166696E);

            Out16((ushort)(NAM + 0x02), 0x0F0F);
            Out16((ushort)(NAM + 0x018), 0x0F0F);

            BufferDescriptors = (BufferDescriptor*)Allocator.Allocate((ulong)(sizeof(BufferDescriptor) * 32));
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct BufferDescriptor
        {
            public uint Addr;
            public ushort Sample;
            public ushort Attr;
        }

        public static unsafe void Play(byte[] PCM)
        {
            ushort sample = 0xFFFE;

            int index = 0;
            fixed (byte* buffer = PCM) 
            {
                for(int i = 0; i < PCM.Length; i+= sample)
                {
                    BufferDescriptors[index].Addr = (uint)(buffer + i);
                    BufferDescriptors[index].Sample = sample;
                    //BufferDescriptors[index].Attr = 1 << 15;
                    if (i + sample > PCM.Length || index > 31)
                    {
                        BufferDescriptors[index].Attr = 1 << 14;
                        break;
                    }
                    index++;
                }
            }

            Out8((ushort)(NABM + 0x1B), 0x02);
            Out32((ushort)(NABM + 0x10), (uint)BufferDescriptors);
            Out8((ushort)(NABM + 0x15), 0);
            Out8((ushort)(NABM + 0x1B), 0x01);
        }
    }
}