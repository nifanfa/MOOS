using MOOS.Driver;
using MOOS.FS;
using MOOS.Misc;
using System.Drawing;
using System.Windows.Forms;

namespace MOOS.GUI
{
    internal unsafe class WAVPlayer : Window
    {
        static byte[] _pcm;
        static int _index;
        static WAV.Header _header;
        public static WAVPlayer _player;
        public static string _song_name;

        Image audiopause;
        Image audioplay;

        public static bool playing;

        public WAVPlayer(int X, int Y) : base(X, Y, 200, 200)
        {
            audiopause = new PNG(File.ReadAllBytes("Images/audiopause.png"));
            audioplay = new PNG(File.ReadAllBytes("Images/audioplay.png"));
            Title = "WAV Player";
            _pcm = null;
            _index = 0;
            Interrupts.EnableInterrupt(0x20, &DoPlay);
            _player = this;
            _song_name = null;
            playing = false;
            clickLock = false;
        }

        bool clickLock;

        public override void OnInput()
        {
            base.OnInput();

            if (Control.MouseButtons.HasFlag(MouseButtons.Left))
            {
                if (IsUnderMouse() && !clickLock)
                {
                    playing = !playing;
                    clickLock = true;
                }
            }
            else
            {
                clickLock = false;
            }
        }

        public override void OnDraw()
        {
            base.OnDraw();

            string s = $"Playing: {_song_name}";
            int len = WindowManager.font.MeasureString(s);
            WindowManager.font.DrawString(X + (Width / 2 - len / 2), Y + 25, s);
            s.Dispose();

            Framebuffer.Graphics.DrawImage(X + (Width / 2 - audioplay.Width / 2), Y + (Height / 2 - audioplay.Height / 2), playing ? audiopause : audioplay);
        }

        public void Play(byte[] wav,string name = "unknown")
        {
            _index = 0;
            WAV.Decode(wav, out var pcm, out var hdr);
            wav.Dispose();
            _pcm = pcm;
            _header = hdr;
            _song_name?.Dispose();
            _song_name = name;

            playing = true;
        }

        public static void DoPlay()
        {
            if (_pcm != null && _player.Visible && playing)
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
