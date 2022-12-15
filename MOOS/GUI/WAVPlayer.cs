using MOOS.Driver;
using MOOS.Misc;

namespace MOOS.GUI
{
    internal unsafe class WAVPlayer : Window
    {
        static byte[] _pcm;
        static int _index;
        static WAV.Header _header;
        public static WAVPlayer _player;

        public WAVPlayer(int X, int Y) : base(X, Y, 200, 0)
        {
            Title = "WAV Player";
            _pcm = null;
            _index = 0;
            Interrupts.EnableInterrupt(0x20, &DoPlay);
            _player = this;
        }

        public void Play(byte[] wav)
        {
            _index = 0;
            WAV.Decode(wav, out var pcm, out var hdr);
            wav.Dispose();
            _pcm = pcm;
            _header = hdr;
        }

        public static void DoPlay()
        {
            if (_pcm != null && _player.Visible)
            {
                if(Audio.bytesWritten != 0)return;
                
                if (_index + Audio.CacheSize > _pcm.Length) _index = 0;

                fixed (byte* buffer = _pcm)
                {
                    _index += Audio.CacheSize;
                    Audio.snd_write(buffer + _index, Audio.CacheSize);
                }
            }
        }
    }
}
