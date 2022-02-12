using System;

namespace Kernel
{
    public static unsafe class Console
    {
        public const byte Width = 80;
        public const byte Height = 25;

        private static byte Color = 0;
        public static int CursorX = 0;
        public static int CursorY = 0;

        internal static void Setup()
        {
            ResetColor();
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

        public static void Write(string s)
        {
            for (byte i = 0; i < s.Length; i++)
            {
                Console.Write(s[i]);
            }
            s.Dispose();
        }

        public static void Back()
        {
            if (CursorX == 0) return;
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

            byte* p = ((byte*)(0xb8000 + (CursorY * Width * 2) + (CursorX * 2)));
            *p = (byte)chr;
            p++;
            *p = Color;
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
                Framebuffer.Fill(X, Y, 8, 16, 0x0);
                ASC16.DrawChar(chr, X, Y, 0xFFFFFFFF);
            }
        }

        public static ConsoleKeyInfo ReadKey(bool echoOff = false)
        {
            PS2Keyboard.CleanKeyInfo();
            while (PS2Keyboard.KeyInfo.KeyChar == '\0') Native.Hlt();
            if (!echoOff)
            {
                if (PS2Keyboard.KeyInfo.Key != ConsoleKey.Enter) Console.Write(PS2Keyboard.KeyInfo.KeyChar);
                else Console.WriteLine();
            }

            return PS2Keyboard.KeyInfo;
        }

        public static string ReadLine() 
        {
            string s = string.Empty;
            ConsoleKeyInfo key;
            while ((key = Console.ReadKey()).Key != ConsoleKey.Enter)
            {
                string cache1 = key.KeyChar.ToString();
                string cache2 = s + cache1;
                s.Dispose();
                cache1.Dispose();
                s = cache2;
                Native.Hlt();
            }
            return s;
        }

        private static void MoveUp()
        {
            if (CursorY >= Height - 1)
            {
                Native.Movsb((void*)0xb8000, (void*)0xB80A0, 0xF00);
                for (int i = 0; i < Width; i++) WriteAt(' ', i, CursorY);

                MoveUpFramebuffer();
                CursorY--;
            }
        }

        private static void MoveUpFramebuffer()
        {
            if(Framebuffer.VideoMemory != null && !Framebuffer.TripleBuffered)
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

        public static void WriteLine(string s)
        {
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
            byte* p = (byte*)0xb8000 + ((y * Width + x) * 2);
            *p = (byte)chr;
            p++;
            *p = Color;
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

        public static byte ForegroundColor
        {
            get { return (byte)(Color & 0x0F); }
            set { Color &= 0xF0; Color |= (byte)(value & 0x0F); }
        }

        public static byte BackgroundColor
        {
            get { return (byte)(Color >> 4); }
            set { Color &= 0x0F; Color |= (byte)((value & 0x0F) << 4); }
        }
    }
}
