using System.Runtime.InteropServices;
using MOOS.Misc;
using static Native;

namespace MOOS
{
    public unsafe class AC97
    {
        private static uint NAM, NABM;
        public static int NumDescriptors;
        private static BufferDescriptor* BufferDescriptors;

        public static unsafe void Initialize()
        {
            PCIDevice device = PCI.GetDevice(0x8086, 0x2415);

            if (device == null)
            {
                return;
            }

            Console.WriteLineInfo("AC97", "Intel 82801AA AC97 Audio Controller Found");
            device.WriteRegister(0x04, 0x04 | 0x02 | 0x01);

            NumDescriptors = 31;

            NAM = device.Bar0 & ~0xFU;
            NABM = device.Bar1 & ~0xFU;

            Out8((ushort)(NABM + 0x2C), 0x2);

            Out32((ushort)(NAM + 0x00), 0x6166696E);

            Out16((ushort)(NAM + 0x02), 0x0F0F);
            Out16((ushort)(NAM + 0x018), 0x0F0F);

            BufferDescriptors = (BufferDescriptor*)Allocator.Allocate((ulong)(sizeof(BufferDescriptor) * 32));

            for (int i = 0; i < NumDescriptors; i++)
            {
                ulong ptr = (ulong)Allocator.Allocate(Audio.SampleRate * 2);
                if (ptr > 0xFFFFFFFF)
                {
                    Panic.Error("Invalid buf");
                }

                BufferDescriptors[i].Address = (uint)ptr;
                //48Khz dual channel
                BufferDescriptors[i].SampleRate = Audio.SampleRate;
                BufferDescriptors[i].Arribute = 1 << 15;
            }

            Interrupts.EnableInterrupt(device.IRQ, &OnInterrupt);
            Index = 0;

            Out16((ushort)(NAM + 0x2C), Audio.SampleRate);
            Out16((ushort)(NAM + 0x32), Audio.SampleRate);

            Out8((ushort)(NABM + 0x1B), 0x02);
            Out32((ushort)(NABM + 0x10), (uint)BufferDescriptors);
            Out8((ushort)(NABM + 0x15), Index);
            Out8((ushort)(NABM + 0x1B), 0x19);

            Audio.HasAudioDevice = true;
        }

        public static byte Index = 0;

        public static void OnInterrupt()
        {
            ushort Status = In16((ushort)(NABM + 0x16));
            if ((Status & (1 << 3)) != 0)
            {
                //Clear last buffer
                int LastIndex = Index;
                Native.Stosb((void*)BufferDescriptors[Index].Address, 0, Audio.SampleRate * 2);

                Index++;
                Index %= (byte)NumDescriptors;

                if (Audio.Queue.Length > 0)
                {
                    byte[] buffer = Audio.Queue.Dequeue();
                    fixed (byte* ptr = buffer)
                    {
                        Native.Movsb((void*)BufferDescriptors[Index].Address, ptr, Audio.SampleRate * 2);
                    }

                    buffer.Dispose();
                }

                Out8((ushort)(NABM + 0x15), Index);
            }
            //Ack
            Out16((ushort)(NABM + 0x16), 0x1C);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct BufferDescriptor
        {
            public uint Address;
            public ushort SampleRate;
            public ushort Arribute;
        }
    }
}