using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime;

namespace MOOS.Misc
{
    public static unsafe class Audio
    {
        public const int SampleRate = 44100;
        public static bool HasAudioDevice;

        const int CacheSize = 1024 * 64;

        public const int SizePerPacket = SampleRate * 2;

        public static void Initialize() 
        {
            HasAudioDevice = false;

            cache = (byte*)Allocator.Allocate(CacheSize);
            bytesWritten = 0;
        }

        /// <summary>
        /// Play a 48khz dual channel PCM
        /// ---how to convert mp3 to pcm?---
        /// ffmpeg -i input.mp3 -ar 48000 -ac 2 -f s16le output.pcm
        /// </summary>
        /// <param name="pcm"></param>
        public static void Play(byte[] pcm) 
        {
            fixed(byte* ptr_pcm = pcm)
            {
                snd_write(ptr_pcm, pcm.Length);
            }
            pcm.Dispose();
        }

        public static byte* cache;
        public static int bytesWritten;

        [RuntimeExport("snd_write")]
        public static int snd_write(byte* buffer, int len)
        {
            if (bytesWritten + len > CacheSize)
            {
                Native.Movsb(cache + bytesWritten - len, cache + bytesWritten, len);
                bytesWritten -= len;
            }

            Native.Movsb(cache + bytesWritten, buffer, len);
            bytesWritten += len;
            //Native.Hlt();
            return len;
        }

        [RuntimeExport("snd_clear")]
        public static void snd_clear()
        {
            bytesWritten = 0;
        }

        public static bool require(byte* buffer)
        {
            if (bytesWritten > 0)
            {
                int size = SizePerPacket > bytesWritten ? bytesWritten : SizePerPacket;

                Native.Movsb(buffer, cache, size);
                bytesWritten -= size;
                if(bytesWritten > SizePerPacket)
                {
                    Native.Movsb(cache, cache + size, bytesWritten);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}