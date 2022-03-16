// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence

using System;

namespace OS_Sharp
{
    public static unsafe class Console
    {
        private static int width = Framebuffer.Width;
        private static int height = Framebuffer.Height;
        private static int cursorX = 0;
        private static int cursorY = 0;
        private static uint foregroundColor = ConsoleColor.White;
        private static uint backgroundColor = ConsoleColor.Black;
        public static int Width { get => width; set => width = value; }
        public static int Height { get => height; set => height = value; }
        public static int CursorX { get => cursorX; set => cursorX = value; }
        public static int CursorY { get => cursorY; set => cursorY = value; }
        public static uint ForegroundColor { get => foregroundColor; set => foregroundColor = value; }
        public static uint BackgroundColor { get => backgroundColor; set => backgroundColor = value; }

        public delegate void OnWriteHandler(char chr);
        public static event OnWriteHandler OnWrite;

        internal static void Initialize()
        {
            Clear();
            EnableCursor();
            SetCursorStyle(0b1110);
        }

        private static void SetCursorStyle(byte style)
        {
            Native.Out8(0x3D4, 0x0A);
            Native.Out8(0x3D5, style);
        }

        private static void EnableCursor()
        {
            Native.Out8(0x3D4, 0x0A);
            Native.Out8(0x3D5, (byte)((Native.In8(0x3D5) & 0xC0) | 0));

            Native.Out8(0x3D4, 0x0B);
            Native.Out8(0x3D5, (byte)((Native.In8(0x3D5) & 0xE0) | 15));
        }



        public static void Back()
        {
            if (CursorX == 0)
            {
                if (CursorY == 0)
                {
                    return;
                }
                else
                {
                    CursorY--;
                    CursorX = Width / 8;
                }
            }

            WriteFramebuffer(' ');
            CursorX--;
            WriteAt(' ', CursorX, CursorY);
            WriteFramebuffer(' ');
            UpdateCursor();
        }

        public static void WriteStrAt(string s, byte line)
        {
            for (byte i = 0; i < s.Length; i++)
            {
                Console.WriteAt(s[i], i, line);
            }
        }

        public static void ResetColor()
        {
            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.White;
        }

        private static void WriteFramebuffer(char chr)
        {
            if (Framebuffer.VideoMemory != null && !Framebuffer.TripleBuffered)
            {
                int X = CursorX * 8;
                int Y = CursorY * 16;
                Framebuffer.Fill(X, Y, 8, 16, BackgroundColor);
                ASC16.DrawChar(chr, X, Y, ForegroundColor);
            }
        }
        private static void WriteFramebuffer(char chr, int x, int y)
        {
            if (Framebuffer.VideoMemory != null && !Framebuffer.TripleBuffered)
            {
                Framebuffer.Fill(x, y, 8, 16, BackgroundColor);
                ASC16.DrawChar(chr, x, y, ForegroundColor);
            }
        }

        public static ConsoleKeyInfo ReadKey()
        {
            PS2Keyboard.CleanKeyInfo(true);
            while (PS2Keyboard.KeyInfo.KeyChar == '\0')
            {
                Native.Hlt();
            }
            return PS2Keyboard.KeyInfo;
        }

        public static string ReadLine()
        {
            int sx = CursorX;
            string s = string.Empty;
            ConsoleKeyInfo key;
            while ((key = ReadKey()).Key != ConsoleKey.Enter)
            {
                switch (key.Key)
                {
                    case ConsoleKey.Delete:
                    case ConsoleKey.Backspace:
                        if (s.Length.ToString() == "0")
                        {
                            continue;
                        }
                        Back();
                        s.Length -= 1;
                        break;
                    default:
                        Write((PS2Keyboard.KeyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift) ? key.KeyChar.ToUpper() : key.KeyChar).ToString());
                        s += (PS2Keyboard.KeyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift) ? key.KeyChar.ToUpper() : key.KeyChar).ToString();
                        break;

                }
                //Native.Hlt();
            }
            Console.WriteLine();
            return s;
        }

        private static void MoveUp()
        {
            if (CursorY * 16 >= Height - 16)
            {
                Native.Movsb((void*)0xb8000, (void*)0xB80A0, 0xF00);
                if (Framebuffer.VideoMemory != null && !Framebuffer.TripleBuffered)
                {
                    Framebuffer.Fill(0, 0, Width, 16, 0x000000);
                    Framebuffer.Copy(0, -16, 0, 0, Width, Height);
                }
                CursorY--;
            }
        }

        private static void UpdateCursor()
        {
            int pos = (CursorY * (Width / 8)) + CursorX;
            Native.Out8(0x3D4, 0x0F);
            Native.Out8(0x3D5, (byte)(pos & 0xFF));
            Native.Out8(0x3D4, 0x0E);
            Native.Out8(0x3D5, (byte)((pos >> 8) & 0xFF));
            UpdateCursorFramebuffer();
        }

        private static void UpdateCursorFramebuffer()
        {
            if (Framebuffer.VideoMemory != null && !Framebuffer.TripleBuffered)
            {
                ASC16.DrawChar('_',
                            CursorX * 8,
                            CursorY * 16,
                            0xFFFFFFFF
                            );
            }
        }

        public static void Write(object s)
        {
            for (byte i = 0; i < s.ToString().Length; i++)
            {
                if (s.ToString()[i] == '\r')
                {
                    continue;
                }

                if (s.ToString()[i] == '\n')
                {
                    WriteLine();
                }
                else
                {
                    OnWrite?.Invoke(s.ToString()[i]);
                    WriteFramebuffer(s.ToString()[i]);
                    CursorX++;
                    if (CursorX * 8 == Width)
                    {
                        CursorX = 0;
                        CursorY++;
                    }
                    MoveUp();
                    UpdateCursor();
                }
            }
            s.Dispose();
        }

        public static void Write(char chr)
        {
            if (chr == '\r')
            {
                return;
            }
            if (chr == '\n')
            {
                WriteLine();
            }
            else
            {
                OnWrite?.Invoke(chr);
                WriteFramebuffer(chr);


                CursorX++;
                if (CursorX * 8 == Width)
                {
                    CursorX = 0;
                    CursorY++;
                }
                MoveUp();
                UpdateCursor();

            }
        }

        public static void Write(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\r')
                {
                    continue;
                }
                if (s[i] == '\n')
                {
                    WriteLine();
                }
                else
                {
                    OnWrite?.Invoke(s[i]);
                    WriteFramebuffer(s[i]);


                    CursorX++;
                    if (CursorX * 8 == Width)
                    {
                        CursorX = 0;
                        CursorY++;
                    }
                    MoveUp();
                    UpdateCursor();
                }
            }
        }

        public static void WriteLine(object o)
        {
            for (byte i = 0; i < o.ToString().Length; i++)
            {
                if (o.ToString()[i] == '\r')
                {
                    continue;
                }
                if (o.ToString()[i] == '\n')
                {
                    WriteLine();
                }
                else
                {
                    OnWrite?.Invoke(o.ToString()[i]);
                    WriteFramebuffer(o.ToString()[i]);


                    CursorX++;
                    if (CursorX * 8 == Width)
                    {
                        CursorX = 0;
                        CursorY++;
                    }
                    MoveUp();
                    UpdateCursor();
                }
            }
            o.Dispose();
            WriteLine();
        }

        public static void WriteLine(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\r')
                {
                    continue;
                }
                if (s[i] == '\n')
                {
                    WriteLine();
                }
                else
                {
                    OnWrite?.Invoke(s[i]);
                    WriteFramebuffer(s[i]);


                    CursorX++;
                    if (CursorX * 8 == Width)
                    {
                        CursorX = 0;
                        CursorY++;
                    }
                    MoveUp();
                    UpdateCursor();
                }
            }
            WriteLine();
        }
        public static void WriteLine(char c)
        {
            if (c == '\r')
            {
                return;
            }
            if (c == '\n')
            {
                WriteLine();
            }
            else
            {
                OnWrite?.Invoke(c);
                WriteFramebuffer(c);


                CursorX++;
                if (CursorX * 8 == Width)
                {
                    CursorX = 0;
                    CursorY++;
                }
                MoveUp();
                UpdateCursor();
                WriteLine();
            }
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

        public static void WriteAt(char chr, int x, int y)
        {
            OnWrite?.Invoke(chr);
            WriteFramebuffer(chr, x, y);
        }

        public static void Clear()
        {
            CursorX = 0;
            CursorY = 0;
            Framebuffer.Clear(BackgroundColor);
        }
    }
}