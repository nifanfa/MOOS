// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the  MIT licence
using System;

namespace Kernel
{
    public static unsafe class Console
    {
        public const byte Width = 80;
        public const byte Height = 25;

        public static int CursorX = 0;
        public static int CursorY = 0;

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

        public static void Write(object s)
        {
            for (byte i = 0; i < s.ToString().Length; i++)
            {
                Console.Write(s.ToString()[i]);
            }
            s.Dispose();
        }

        public static void Back()
        {
            if (CursorX == 0)
            {
                return;
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

        public static void Write(char chr)
        {
            OnWrite?.Invoke(chr);
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

        private static void WriteFramebuffer(char chr)
        {
            if (Framebuffer.VideoMemory != null && !Framebuffer.TripleBuffered)
            {
                int X = (Framebuffer.Width / 2) - ((Width * 8) / 2) + (CursorX * 8);
                int Y = (Framebuffer.Height / 2) - ((Height * 16) / 2) + (CursorY * 16);
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

        public static ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            PS2Keyboard.CleanKeyInfo(true);
            while (PS2Keyboard.KeyInfo.KeyChar == '\0')
            {
                Native.Hlt();
            }

            if (!intercept)
            {
                switch (PS2Keyboard.KeyInfo.Key)
                {
                    case ConsoleKey.Enter:
                        Console.WriteLine();
                        break;
                    case ConsoleKey.Delete:
                    case ConsoleKey.Backspace:
                        Console.Back();
                        break;
                    default:
                        Console.Write((PS2Keyboard.KeyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift) ? PS2Keyboard.KeyInfo.KeyChar.ToUpper() : PS2Keyboard.KeyInfo.KeyChar).ToString());
                        break;
                }
            }
            return PS2Keyboard.KeyInfo;
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
                        if (s.Length == 0)
                        {
                            continue;
                        }

                        s.Length -= 1;
                        break;
                    default:
                        s += (PS2Keyboard.KeyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift) ? key.KeyChar.ToUpper() : key.KeyChar).ToString();
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
                Native.Movsb((void*)0xb8000, (void*)0xB80A0, 0xF00);
                for (int i = 0; i < Width; i++)
                {
                    WriteAt(' ', i, CursorY);
                }

                MoveUpFramebuffer();
                CursorY--;
            }
        }

        private static void MoveUpFramebuffer()
        {
            if (Framebuffer.VideoMemory != null && !Framebuffer.TripleBuffered)
            {
                Framebuffer.Copy(
                    (Framebuffer.Width / 2) - (Width * 8 / 2),
                    (Framebuffer.Height / 2) - (Height * 16 / 2),

                    (Framebuffer.Width / 2) - (Width * 8 / 2),
                    (Framebuffer.Height / 2) - (Height * 16 / 2) + 16,

                    Width * 8,
                    Height * 16
                    );
            }
        }

        private static void UpdateCursor()
        {
            int pos = (CursorY * Width) + CursorX;
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
                            (Framebuffer.Width / 2) - ((Width * 8) / 2) + ((CursorX) * 8),
                            (Framebuffer.Height / 2) - ((Height * 16) / 2) + (CursorY * 16),
                            0xFFFFFFFF
                            );
            }
        }

        public static void WriteLine(object o)
        {
            string s = o.ToString();
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

        public static void WriteAt(char chr, int x, int y)
        {
            WriteFramebuffer(chr, x, y);
        }

        public static void Clear()
        {
            CursorX = 0;
            CursorY = 0;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    WriteAt(' ', x, y);
                }
            }
        }

        public static uint ForegroundColor = ConsoleColor.White;
        public static uint BackgroundColor = ConsoleColor.Black;
    }
}
