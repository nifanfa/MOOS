/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
using Kernel;
using Kernel.Driver;
using System;
using System.Runtime.InteropServices;
using static Native;

namespace Kernel
{
    public unsafe class AC97
    {
        private static uint NAM, NABM;
        public static int NumDescriptors;
        public static int IRQ;

        static BufferDescriptor* BufferDescriptors;

        public static unsafe void Initialize()
        {
            PCIDevice device = PCI.GetDevice(0x8086, 0x2415);

            if (device == null) return;

            Console.WriteLine("Intel 82801AA AC97 Audio Controller Found");
            device.WriteRegister(0x04, 0x04 | 0x02 | 0x01);

            NumDescriptors = 31;

            NAM = device.Bar0 & ~(0xFU);
            NABM = device.Bar1 & ~(0xFU);

            Out8((ushort)(NABM + 0x2C), 0x2);

            Out32((ushort)(NAM + 0x00), 0x6166696E);

            Out16((ushort)(NAM + 0x02), 0x0F0F);
            Out16((ushort)(NAM + 0x018), 0x0F0F);

            BufferDescriptors = (BufferDescriptor*)Allocator.Allocate((ulong)(sizeof(BufferDescriptor) * 32));

            IRQ = 0x20;
            Index = 0;
        }

        public static byte Index = 0;

        public static void OnInterrupt() 
        {
            ushort Status = In16((ushort)(NABM + 0x16));
            if((Status & (1 << 3)) != 0)
            {
                Out8((ushort)(NABM + 0x15), Index++);
                if (Index > NumDescriptors) Index = 0;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct BufferDescriptor
        {
            public uint Address;
            public ushort SampleRate;
            public ushort Arribute;
        }

        public static unsafe void Play(byte[] PCM, ushort SampleRate = 48000, bool DualChannel = true)
        {
            int index = 0;
            fixed (byte* buffer = PCM) 
            {
                for (int i = 0; i < PCM.Length; i += SampleRate * (DualChannel ? 2 : 1))
                {
                    BufferDescriptors[index].Address = (uint)(buffer + i);
                    BufferDescriptors[index].SampleRate = SampleRate;
                    BufferDescriptors[index].Arribute = 1 << 15;
                    if (i + SampleRate > PCM.Length || index > NumDescriptors)
                    {
                        BufferDescriptors[index].Arribute |= 1 << 14;
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