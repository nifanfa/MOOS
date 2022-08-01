#define ASCII

using MOOS.Driver;
using System;
using System.Drawing;

namespace MOOS
{
    public static unsafe class Console
    {
        public static int Width { get => Framebuffer.Width / 8; }
        public static int Height { get => Framebuffer.Height / 16; }

        public static int CursorX = 0;
        public static int CursorY = 0;

        public delegate void OnWriteHandler(char chr);
        public static event OnWriteHandler OnWrite;

        static Color[] ColorsFramebuffer;

        public static ConsoleColor ForegroundColor;
        public static ConsoleColor BackgroundColor;

        internal static void Setup()
        {
            OnWrite = null;

            ColorsFramebuffer = new Color[16]
            {
                Color.Black,
                Color.Blue,
                Color.Green,
                Color.Cyan,
                Color.Red,
                Color.Purple,
                Color.Brown,
                Color.Gray,
                Color.DarkGray,
                Color.LightBlue,
                Color.LightGreen,
                Color.LightCyan,
                Color.MediumVioletRed,
                Color.MediumPurple,
                Color.Yellow,
                Color.White,
            };

            ForegroundColor = ConsoleColor.White;
            BackgroundColor = ConsoleColor.Black;

            Clear();
        }

        public static void Wait(ref bool b)
        {
            int phase = 0;
            while (!b)
            {
                switch (phase)
                {
                    case 0:
                        Console.Write('/', true);
                        break;
                    case 1:
                        Console.Write('-', true);
                        break;
                    case 2:
                        Console.Write('\\', true);
                        break;
                    case 3:
                        Console.Write('|', true);
                        break;
                    case 4:
                        Console.Write('/', true);
                        break;
                    case 5:
                        Console.Write('-', true);
                        break;
                    case 6:
                        Console.Write('\\', true);
                        break;
                    case 7:
                        Console.Write('|', true);
                        break;
                }
                phase++;
                phase %= 8;
                Console.CursorX--;
                ACPITimer.Sleep(100000);
            }
        }

        public static void Wait(uint* provider, int bit)
        {
            int phase = 0;
            while (!BitHelpers.IsBitSet(*provider, bit))
            {
                switch (phase)
                {
                    case 0:
                        Console.Write('/', true);
                        break;
                    case 1:
                        Console.Write('-', true);
                        break;
                    case 2:
                        Console.Write('\\', true);
                        break;
                    case 3:
                        Console.Write('|', true);
                        break;
                    case 4:
                        Console.Write('/', true);
                        break;
                    case 5:
                        Console.Write('-', true);
                        break;
                    case 6:
                        Console.Write('\\', true);
                        break;
                    case 7:
                        Console.Write('|', true);
                        break;
                }
                phase++;
                phase %= 8;
                Console.CursorX--;
                ACPITimer.Sleep(100000);
            }
        }

        public static bool Wait(delegate*<bool> func, int timeOutMS = -1)
        {
            ulong prev = Timer.Ticks;

            int phase = 0;
            while (!func())
            {
                if (timeOutMS >= 0 && Timer.Ticks > (prev + (uint)timeOutMS))
                {
                    return false;
                }
                switch (phase)
                {
                    case 0:
                        Console.Write('/', true);
                        break;
                    case 1:
                        Console.Write('-', true);
                        break;
                    case 2:
                        Console.Write('\\', true);
                        break;
                    case 3:
                        Console.Write('|', true);
                        break;
                    case 4:
                        Console.Write('/', true);
                        break;
                    case 5:
                        Console.Write('-', true);
                        break;
                    case 6:
                        Console.Write('\\', true);
                        break;
                    case 7:
                        Console.Write('|', true);
                        break;
                }
                phase++;
                phase %= 8;
                Console.CursorX--;
                ACPITimer.Sleep(100000);
            }
            return true;
        }

        public static void Write(string s)
        {
            ConsoleColor col = Console.ForegroundColor;
            for (byte i = 0; i < s.Length; i++)
            {
                if (s[i] == '[')
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                Console.Write(s[i]);
                if (s[i] == ']')
                {
                    Console.ForegroundColor = col;
                }
            }
            s.Dispose();
        }

        public static void Back()
        {
            if (CursorX == 0) return;
            WriteFramebuffer(' ');
            CursorX--;
            WriteFramebuffer(' ');
            UpdateCursor();
        }

        public static void Write(char chr, bool dontInvoke = false)
        {
            if (chr == '\n')
            {
                WriteLine();
                return;
            }
#if ASCII
            if (chr >= 0x20 && chr <= 0x7E)
#else
            unsafe
#endif
            {
                if (!dontInvoke)
                {
                    OnWrite?.Invoke(chr);
                }

                WriteFramebuffer(chr);

                CursorX++;
                if (CursorX == Width)
                {
                    CursorX = 0;
                    CursorY++;
                }
                MoveUp();
                UpdateCursor();
            }
        }

        private static void WriteFramebuffer(char chr)
        {
            if (Framebuffer.FirstBuffer != null && !Framebuffer.DoubleBuffered)
            {
                int X = (Framebuffer.Graphics.Width / 2) - ((Width * 8) / 2) + (CursorX * 8);
                int Y = (Framebuffer.Graphics.Height / 2) - ((Height * 16) / 2) + (CursorY * 16);
                Framebuffer.Graphics.FillRectangle(ColorsFramebuffer[(int)BackgroundColor], X, Y, 8, 16);
                ASC16.DrawChar(ColorsFramebuffer[(int)ForegroundColor],chr, X, Y);
            }
        }

        public static ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            Keyboard.CleanKeyInfo(true);
            while (Keyboard.KeyInfo.KeyChar == '\0') Native.Hlt();
            if (!intercept)
            {
                switch (Keyboard.KeyInfo.Key)
                {
                    case ConsoleKey.Enter:
                        Console.WriteLine();
                        break;
                    case ConsoleKey.Delete:
                    case ConsoleKey.Backspace:
                        Console.Back();
                        break;
                    default:
                        Console.Write(Keyboard.KeyInfo.KeyChar);
                        break;
                }
            }
            return Keyboard.KeyInfo;
        }

        public static string ReadLine()
        {
            string s = string.Empty;
            ConsoleKeyInfo key;
            while ((key = Console.ReadKey()).Key != ConsoleKey.Enter)
            {
                switch (key.Key)
                {
                    case ConsoleKey.Delete:
                    case ConsoleKey.Backspace:
                        if (s.Length == 0) continue;
                        s.Length -= 1;
                        break;
                    default:
                        string cache1 = key.KeyChar.ToString();
                        string cache2 = s + cache1;
                        s.Dispose();
                        cache1.Dispose();
                        s = cache2;
                        break;

                }
                Native.Hlt();
            }
            return s;
        }

        private static void MoveUp()
        {
            if (CursorY >= Height - 1)
            {
                MoveUpFramebuffer();
                CursorY--;
            }
        }

        private static void MoveUpFramebuffer()
        {
            if (Framebuffer.FirstBuffer != null && !Framebuffer.DoubleBuffered)
            {
                Framebuffer.Graphics.CopyFromScreen(
                    (Framebuffer.Graphics.Width / 2) - (Width * 8 / 2),
                    (Framebuffer.Graphics.Height / 2) - (Height * 16 / 2) + 16,

                    (Framebuffer.Graphics.Width / 2) - (Width * 8 / 2),
                    (Framebuffer.Graphics.Height / 2) - (Height * 16 / 2),

                    new Size(
                    Width * 8,
                    Height * 16)
                    );
            }
        }

        private static void UpdateCursor()
        {
            UpdateCursorFramebuffer();
        }

        private static void UpdateCursorFramebuffer()
        {
            if (Framebuffer.FirstBuffer != null && !Framebuffer.DoubleBuffered)
            {
                ASC16.DrawChar(
                            Color.White,
                            '_',
                            (Framebuffer.Graphics.Width / 2) - ((Width * 8) / 2) + ((CursorX) * 8),
                            (Framebuffer.Graphics.Height / 2) - ((Height * 16) / 2) + (CursorY * 16)
                            );
            }
        }

        public static void WriteInfo(string catagory, string message)
		{
			ConsoleColor originalFG = ForegroundColor;
			ForegroundColor = ConsoleColor.Yellow;
			Console.Write($"[{catagory}] ");
			ForegroundColor = originalFG;
			Console.Write(message);
		}

		public static void WriteLineInfo(string catagory, string message)
		{
			ConsoleColor originalFG = ForegroundColor;
			ForegroundColor = ConsoleColor.Yellow;
			Console.Write($"[{catagory}] ");
			ForegroundColor = originalFG;
			Console.WriteLine(message);
		}

        public static void WriteLine(string s)
        {
            Write(s);
            OnWrite?.Invoke('\n');
            WriteFramebuffer(' ');
            CursorX = 0;
            CursorY++;
            MoveUp();
            UpdateCursor();
            s.Dispose();
        }

        public static void WriteLine()
        {
            OnWrite?.Invoke('\n');
            WriteFramebuffer(' ');
            CursorX = 0;
            CursorY++;
            MoveUp();
            UpdateCursor();
        }

        public static void Clear()
        {
            CursorX = 0;
            CursorY = 0;
            ClearFramebuffer();
        }

        private static void ClearFramebuffer()
        {
            if (Framebuffer.FirstBuffer != null && !Framebuffer.DoubleBuffered)
            {
                Framebuffer.Graphics.FillRectangle
                    (
                            ColorsFramebuffer[(int)BackgroundColor],
                            (Framebuffer.Graphics.Width / 2) - ((Width * 8) / 2) + ((CursorX) * 8),
                            (Framebuffer.Graphics.Height / 2) - ((Height * 16) / 2) + (CursorY * 16),
                            Width * 8,
                            Height * 16
                    );
            }
        }
    }
}