#define ASCII

using System;
using System.Drawing;
using MOOS.Driver;

namespace MOOS
{
    public static unsafe class Console
    {
        private static int cursorX = 0;
        private static int cursorY = 0;
        private static uint foregroundColor = (uint)ConsoleColor.White;
        private static uint backgroundColor = (uint)ConsoleColor.Black;
        public static int CursorX { get => cursorX; set => cursorX = value; }
        public static int CursorY { get => cursorY; set => cursorY = value; }
        public static ConsoleColor ForegroundColor { get => (ConsoleColor)foregroundColor; set => foregroundColor = (uint)value; }
        public static ConsoleColor BackgroundColor { get => (ConsoleColor)backgroundColor; set => backgroundColor = (uint)value; }


        public delegate void OnWriteHandler(char chr);
        public static event OnWriteHandler OnWrite;

        private static Color[] ColorsFB;

        internal static void Setup()
        {
            OnWrite = null;

            Clear();

            ColorsFB = new Color[16]
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
                Color.White
            };

            ForegroundColor = ConsoleColor.White;
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
                Console.CursorX -= 8;
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
                Console.CursorX -= 8;
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
                Console.CursorX -= 8;
                ACPITimer.Sleep(100000);
            }
            return true;
        }

        public static void Write(string s)
        {
            for (byte i = 0; i < s.Length; i++)
            {
                Console.Write(s[i]);
            }
            s.Dispose();
        }
        public static void Write(object obj)
        {
            string s = obj.ToString();
            for (byte i = 0; i < s.Length; i++)
            {
                Console.Write(s[i]);
            }
            s.Dispose();
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

        public static void Back()
        {
            if (CursorX == 0)
            {
                if (CursorY == 0)
                {
                    return;
                }
                CursorX = Framebuffer.Width;
                CursorY -= 16;
            }
            Framebuffer.Graphics.FillRectangle(ColorsFB[(int)BackgroundColor], CursorX, CursorY, 16, 16);
            CursorX -= 8;
            UpdateCursor();
        }

        public static void Write(char c, bool dontInvoke = false)
        {
            if (c == '\r')
            {
                return;
            }

            if (c == '\n')
            {
                WriteLine();
                return;
            }
#if ASCII
            if (c >= 0x20 && c <= 0x7E)
#else
	unsafe
#endif
            {
                if (!dontInvoke)
                {
                    OnWrite?.Invoke(c);
                }

                WriteFramebuffer(c);

                CursorX += 8;
                if (CursorX == Framebuffer.Width)
                {
                    CursorX = 0;
                    CursorY += 16;
                }
                MoveUp();
                UpdateCursor();
            }
        }

        private static void WriteFramebuffer(char chr)
        {
            if (Framebuffer.FirstBuffer != null && !Framebuffer.DoubleBuffered)
            {
                Framebuffer.Graphics.FillRectangle(ColorsFB[(int)BackgroundColor], CursorX, CursorY, 8, 16);
                ASC16.DrawChar(ColorsFB[(int)ForegroundColor], chr, CursorX, CursorY);
            }
        }

        public static ConsoleKeyInfo ReadKey()
        {
            Keyboard.CleanKeyInfo(true);
            while (Keyboard.KeyInfo.KeyChar == '\0')
            {
                Native.Hlt();
            }
            return Keyboard.KeyInfo;
        }

        public static string ReadLine()
        {
            string s = string.Empty;
            ConsoleKeyInfo key;
            while ((key = ReadKey()).Key != ConsoleKey.Enter)
            {
                switch (key.Key)
                {
                    case ConsoleKey.Delete:
                    case ConsoleKey.Backspace:
                        if (s.Length == 0)
                        {
                            continue;
                        }

                        s.Length -= 1;
                        Back();
                        break;
                    default:
                        s += key.KeyChar.ToString();
                        Console.Write(key.KeyChar.ToString());
                        break;

                }
                Native.Hlt();
            }
            Console.WriteLine();
            return s;
        }

        private static void MoveUp()
        {
            if (CursorY >= Framebuffer.Height - 16)
            {
                CursorY -= 16;
                Native.Movsb((void*)0xb8000, (void*)0xB80A0, 0xF00);
                if (Framebuffer.FirstBuffer != null && !Framebuffer.DoubleBuffered)
                {
                    Framebuffer.Graphics.FillRectangle(ColorsFB[(int)BackgroundColor], 0, Framebuffer.Height - 16, Framebuffer.Width, 16);
                    Framebuffer.Graphics.CopyFromScreen(0, -16, 0, 0, new Size(Framebuffer.Width, Framebuffer.Height));
                }
                UpdateCursor();
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
                ASC16.DrawChar(Color.White, '_', CursorX, CursorY);
            }
        }

        public static void WriteLine(string s)
        {
            Write(s);
            WriteLine();
            s.Dispose();
        }

        public static void WriteLine(object o)
        {
            string s = o.ToString();
            Write(s);
            WriteLine();
            s.Dispose();
        }

        public static void WriteLine()
        {
            WriteFramebuffer(' ');
            OnWrite?.Invoke('\n');
            CursorX = 0;
            CursorY += 16;
            MoveUp();
            UpdateCursor();
        }

        public static void WriteAt(char chr, int x, int y)
        {
            CursorX = x * 8;
            CursorY = y * 16;
            Write(chr);
        }

        public static void Clear(uint background)
        {
            CursorX = 0;
            CursorY = 0;
            Framebuffer.Graphics.Clear(Color.FromArgb(background));
            Framebuffer.Update();
        }
        public static void Clear()
        {
            Clear(0x0);
        }
    }
}
