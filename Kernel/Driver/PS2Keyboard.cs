/*
* Copyright (c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
*/

//http://cc.etsii.ull.es/ftp/antiguo/EC/AOA/APPND/Apndxc.pdf

using MOOS.Misc;
using System;
using static System.ConsoleKey;

namespace MOOS
{
    public static unsafe class PS2Keyboard
    {
        private static char[] keyChars;
        private static char[] keyCharsShift;
        private static ConsoleKey[] keys;

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

            Keyboard.CleanKeyInfo();
            Interrupts.EnableInterrupt(0x21, &OnInterrupt);
            return true;
        }

        public static void OnInterrupt()
        {
            byte b = Native.In8(0x60);
            PS2Keyboard.ProcessKey(b);
        }

        public static void ProcessKey(byte b)
        {
            if (b >= keys.Length && (b - 0x80) >= keys.Length) return;

            Keyboard.KeyInfo.ScanCode = b;
            Keyboard.KeyInfo.KeyState = b > 0x80 ? ConsoleKeyState.Released : ConsoleKeyState.Pressed;

            SetIfKeyModifier(b, 0x1D, ConsoleModifiers.Control);
            SetIfKeyModifier(b, 0x2A, ConsoleModifiers.Shift);
            SetIfKeyModifier(b, 0x36, ConsoleModifiers.Shift);
            SetIfKeyModifier(b, 0x38, ConsoleModifiers.Alt);

            if (b == 0x3A)
            {
                if (Keyboard.KeyInfo.Modifiers.HasFlag(ConsoleModifiers.CapsLock))
                {
                    Keyboard.KeyInfo.Modifiers &= ~ConsoleModifiers.CapsLock;
                }
                else
                {
                    Keyboard.KeyInfo.Modifiers |= ConsoleModifiers.CapsLock;
                }
            }

            if (b < keyChars.Length)
            {
                Keyboard.KeyInfo.KeyChar = Keyboard.KeyInfo.Modifiers.HasFlag(ConsoleModifiers.CapsLock) ? keyChars[b].ToUpper() : keyChars[b];
            }
            if (b < keyCharsShift.Length && Keyboard.KeyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift))
            {
                Keyboard.KeyInfo.KeyChar = Keyboard.KeyInfo.Modifiers.HasFlag(ConsoleModifiers.CapsLock) ? keyCharsShift[b].ToUpper() : keyCharsShift[b];
            }

            if (b < keys.Length)
            {
                Keyboard.KeyInfo.Key = keys[b];
            }
            else
            {
                Keyboard.KeyInfo.Key = keys[b - 0x80];
            }

            Keyboard.InvokeOnKeyChanged(Keyboard.KeyInfo);

            //This is for some kind of PC that have PS2 emulation but doesn't have PS2 mouse emulation
            Kbd2Mouse.OnKeyChanged(Keyboard.KeyInfo);
        }

        private static void SetIfKeyModifier(byte scanCode, byte pressedScanCode, ConsoleModifiers modifier)
        {
            if (scanCode == pressedScanCode)
            {
                Keyboard.KeyInfo.Modifiers |= modifier;
            }

            if (scanCode == pressedScanCode + 0x80)
            {
                Keyboard.KeyInfo.Modifiers &= ~modifier;
            }
        }
    }
}