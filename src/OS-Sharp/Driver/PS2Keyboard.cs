// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the  MIT licence
//http://cc.etsii.ull.es/ftp/antiguo/EC/AOA/APPND/Apndxc.pdf

using System;
using static System.ConsoleKey;

namespace Kernel
{
    public static class PS2Keyboard
    {
        public static ConsoleKeyInfo KeyInfo;

        private static char[] keyChars;
        private static char[] keyCharsShift;
        private static ConsoleKey[] keys;

        public delegate void OnKeyHandler(ConsoleKeyInfo key);
        public static event OnKeyHandler OnKeyChanged;

        public static bool Initialize()
        {
            keyChars = new char[]
            {
                '\0','\0','1','2','3','4','5','6','7','8','9','0','-','=','\b',' ',
                'q','w','e','r','t','y','u','i','o','p','[',']','\n','\0',
                'a','s','d','f','g','h','j','k','l',';','\'','`','\0','\\',
                'z','x','c','v','b','n','m',',','.','/','\0','\0','\0',' ','\0',
                '\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','-',
                '\0','\0','\0','+','\0','\0','\0','\0','\b','/','\n','\0','\0','\0','\b','\0','\0'
                ,'\0','\0','\0','\0','\0','\0','\0','\0','\0'
            };

            keyCharsShift = new char[]
            {
                '\0','\0','!','@','#','$','%','^','&','*','(',')','_','+','\b',' ',
                'q','w','e','r','t','y','u','i','o','p','{','}','\n','\0',
                'a','s','d','f','g','h','j','k','l',':','\"','~','\0','|',
                'z','x','c','v','b','n','m','<','>','?','\0','\0','\0',' ','\0',
                '\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','-',
                '\0','\0','\0','+','\0','\0','\0','\0','\b','/','\n','\0','\0','\0','\b','\0','\0'
                ,'\0','\0','\0','\0','\0','\0','\0','\0','\0'
            };

            keys = new[] {
                None, Escape, D1, D2, D3, D4, D5, D6, D7, D8, D9, D0, OemMinus, OemPlus, Backspace, Tab,
                Q, W, E, R, T, Y, U, I, O, P, Oem4, Oem6, Return, LControlKey,
                A, S, D, F, G, H, J, K, L, Oem1, Oem7, Oem3, LShiftKey, Oem8,
                Z, X, C, V, B, N, M, OemComma, OemPeriod, Oem2, RShiftKey, Multiply, LMenu, Space, Capital, F1, F2, F3, F4, F5,
                F6, F7, F8, F9, F10, NumLock, Scroll, Home, Up, Prior, Subtract, Left, Clear, Right, Add, End,
                Down, Next, Insert, Delete, Snapshot, None, Oem5, F11, F12
            };

            CleanKeyInfo();
            return true;
        }

        public static void CleanKeyInfo(bool NoModifiers = false)
        {
            KeyInfo.KeyChar = '\0';
            KeyInfo.ScanCode = 0;
            KeyInfo.KeyState = ConsoleKeyState.None;
            if (!NoModifiers)
            {
                KeyInfo.Modifiers = ConsoleModifiers.None;
            }
        }

        public static void ProcessKey(byte b)
        {
            KeyInfo.ScanCode = b;
            KeyInfo.KeyState = b > 0x80 ? ConsoleKeyState.Released : ConsoleKeyState.Pressed;

            SetIfKeyModifier(b, 0x1D, ConsoleModifiers.Ctrl);
            SetIfKeyModifier(b, 0x2A, ConsoleModifiers.Shift);
            SetIfKeyModifier(b, 0x36, ConsoleModifiers.Shift);
            SetIfKeyModifier(b, 0x38, ConsoleModifiers.Alt);

            if (b == 0x3A)
            {
                if (PS2Keyboard.KeyInfo.Modifiers.HasFlag(ConsoleModifiers.CapsLock))
                {
                    KeyInfo.Modifiers &= ~ConsoleModifiers.CapsLock;
                }
                else
                {
                    KeyInfo.Modifiers |= ConsoleModifiers.CapsLock;
                }
            }

            if (b < keyChars.Length)
            {
                KeyInfo.KeyChar = PS2Keyboard.KeyInfo.Modifiers.HasFlag(ConsoleModifiers.CapsLock) ? char.ToUpper(keyChars[b]) : keyChars[b];
            }
            if (b < keyCharsShift.Length && PS2Keyboard.KeyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift))
            {
                KeyInfo.KeyChar = PS2Keyboard.KeyInfo.Modifiers.HasFlag(ConsoleModifiers.CapsLock) ? char.ToUpper(keyCharsShift[b]) : keyCharsShift[b];
            }

            if (b - 0x80 < keys.Length)
            {
                KeyInfo.Key = keys[b < keys.Length ? b : b - 0x80];
            }

            OnKeyChanged?.Invoke(KeyInfo);
        }

        private static void SetIfKeyModifier(byte scanCode, byte pressedScanCode, ConsoleModifiers modifier)
        {
            if (scanCode == pressedScanCode)
            {
                KeyInfo.Modifiers |= modifier;
            }

            if (scanCode == pressedScanCode + 0x80)
            {
                KeyInfo.Modifiers &= ~modifier;
            }
        }
    }
}
