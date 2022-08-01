using System.Collections.Generic;

namespace MOOS.Misc
{
	public static unsafe class Audio
	{
		public const int SampleRate = 44100;

		public static Queue<byte[]> Queue;

		public static bool HasAudioDevice;

		public static void Initialize()
		{
			Queue = new();
			HasAudioDevice = false;
		}

		/// <summary>
		/// Play a 48khz dual channel PCM
		/// ---how to convert mp3 to pcm?---
		/// ffmpeg -i input.mp3 -ar 48000 -ac 2 -f s16le output.pcm
		/// </summary>
		/// <param name="pcm"></param>
		public static void Play(byte[] pcm)
		{
			fixed (byte* ptr_pcm = pcm)
			{
				//Align down
				int i = pcm.Length - (pcm.Length % (SampleRate * 2));
				if (i < 0)
				{
					return;
				}

				for (; i >= 0; i -= SampleRate * 2)
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