/* Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence
using System;

namespace Kernel
{
    public static unsafe class Console
    {
        public static int Width = Framebuffer.Width;
        public static int Height = Framebuffer.Height;
        public static uint ForegroundColor = ConsoleColor.White;
        public static uint BackgroundColor = ConsoleColor.Black;
        private static int CursorX = 0;
        private static int CursorY = 0;


        internal static void Initialize()
        {
            Clear();
            EnableCursor();
            SetCursorStyle(14);
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
                if (CursorY == 0)
                {
                    return;
                }
                else
                {
                    CursorY--;
                    CursorX = Width;
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

        public static void Write(char chr)
        {
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
                MoveUpFramebuffer();
                CursorY--;
            }
        }

        private static void MoveUpFramebuffer()
        {
            if (Framebuffer.VideoMemory != null && !Framebuffer.TripleBuffered)
            {
                Framebuffer.Copy(0, -16, 0, 0, Width, Height);
            }
        }

        private static void UpdateCursor()
        {
            int pos = ((CursorY * 16) * Width) + CursorX * 8;
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

        public static void WriteLine(object o)
        {
            string s = o.ToString();
            Write(s);
            WriteFramebuffer(' ');
            CursorX = 0;
            CursorY++;
            MoveUp();
            UpdateCursor();
            s.Dispose();
        }

        public static void WriteLine()
        {
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
    }
}*/

// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence

using System;

namespace Kernel
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

        public static void Write(char chr)
        {
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
                MoveUpFramebuffer();
                CursorY--;
            }
        }

        private static void MoveUpFramebuffer()
        {
            if (Framebuffer.VideoMemory != null && !Framebuffer.TripleBuffered)
            {
                Framebuffer.Copy(0, -16, 0, 0, Width, Height);
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

        public static void WriteLine(object o)
        {
            string s = o.ToString();
            Write(s);
            WriteFramebuffer(' ');
            CursorX = 0;
            CursorY++;
            MoveUp();
            UpdateCursor();
            s.Dispose();
        }

        public static void WriteLine()
        {
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
            Framebuffer.Clear(BackgroundColor);
        }
    }
}