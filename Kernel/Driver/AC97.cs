/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
using MOOS;
using MOOS.Driver;
using MOOS.Misc;
using System;
using System.Runtime.InteropServices;
using static Native;

namespace MOOS
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
            device.WriteRegister16(0x04, 0x04 | 0x02 | 0x01);

            NumDescriptors = 31;

            NAM = device.Bar0 & ~(0xFU);
            NABM = device.Bar1 & ~(0xFU);

            Out8((ushort)(NABM + 0x2C), 0x2);

            Out32((ushort)(NAM + 0x00), 0x6166696E);

            Out16((ushort)(NAM + 0x02), 0x0F0F);
            Out16((ushort)(NAM + 0x018), 0x0F0F);

            BufferDescriptors = (BufferDescriptor*)Allocator.Allocate((ulong)(sizeof(BufferDescriptor) * 32));

            for(int i = 0; i < NumDescriptors; i++)
            {
                ulong ptr = (ulong)Allocator.Allocate(Audio.SampleRate*2);
                if (ptr > 0xFFFFFFFF) Panic.Error("[AC97] Invalid buf");
                BufferDescriptors[i].Address = (uint)ptr;
                //48Khz dual channel
                BufferDescriptors[i].SampleRate = Audio.SampleRate;
                BufferDescriptors[i].Arribute = 1 << 15;
            }

            IRQ = device.IRQ;
            Interrupts.EnableInterrupt((byte)IRQ);
            Index = 0;

            Out8((ushort)(NABM + 0x1B), 0x02);
            Out32((ushort)(NABM + 0x10), (uint)BufferDescriptors);
            Out8((ushort)(NABM + 0x15), Index);
            Out8((ushort)(NABM + 0x1B), 0x19);
        }

        public static byte Index = 0;

        public static void OnInterrupt()
        {
            ushort Status = In16((ushort)(NABM + 0x16));
            if((Status & (1 << 3)) != 0)
            {
                //Clear last buffer
                int LastIndex = Index;
                Native.Stosb((void*)BufferDescriptors[Index].Address, 0, Audio.SampleRate * 2);

                Index++;
                Index %= (byte)NumDescriptors;

                if(Audio.Queue.Length > 0)
                {
                    byte[] buffer = Audio.Queue.Dequeue();
                    fixed (byte* ptr = buffer)
                        Native.Movsb((void*)BufferDescriptors[Index].Address, ptr, Audio.SampleRate * 2);
                    buffer.Dispose();
                }

                Out8((ushort)(NABM + 0x15), Index);
            }
            //Ack
            Out16((ushort)(NABM + 0x16), 0x1C);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct BufferDescriptor
        {
            public uint Address;
            public ushort SampleRate;
            public ushort Arribute;
        }
    }
}