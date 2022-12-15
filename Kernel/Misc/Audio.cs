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

        public const int CacheSize = 1024 * 86;

        public const int SizePerPacket = SampleRate * 2;

        public static bool CanTake;

        public static void Initialize() 
        {
            CanTake = true;
            HasAudioDevice = false;

            cache = (byte*)Allocator.Allocate(CacheSize);
            bytesWritten = 0;
        }

        public static byte* cache;
        public static int bytesWritten;

        [RuntimeExport("snd_write")]
        public static int snd_write(byte* buffer, int len)
        {
            CanTake = false;
            if (bytesWritten + len > CacheSize)
            {
                Native.Movsb(cache + bytesWritten - len, cache + bytesWritten, len);
                bytesWritten -= len;
            }

            Native.Movsb(cache + bytesWritten, buffer, len);
            bytesWritten += len;
            CanTake = true;
            return len;
        }

        [RuntimeExport("snd_clear")]
        public static void snd_clear()
        {
            bytesWritten = 0;
        }

        public static bool require(byte* buffer)
        {
            if (CanTake && bytesWritten > 0)
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