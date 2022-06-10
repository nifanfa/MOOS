using System;
using System.Collections.Generic;

namespace MOOS.Misc
{
    public static unsafe class Audio
    {
        public const int SampleRate = 48000;

        public static Queue<byte[]> Queue;

        public static void Initialize() 
        {
            Queue = new();
        }

        /// <summary>
        /// Play a 48khz dual channel PCM
        /// </summary>
        /// <param name="pcm"></param>
        public static void Play(byte[] pcm) 
        {
            fixed(byte* ptr_pcm = pcm)
            {
                for (int i = pcm.Length; i >= 0; i -= SampleRate * 2)
                {
                    byte[] pack = new byte[SampleRate * 2];
                    fixed (byte* ptr = pack)
                    {
                        Native.Movsb(ptr, ptr_pcm + i, (ulong)pack.Length);
                    }
                    Queue.Enqueue(pack);
                }
            }
        }
    }
}
