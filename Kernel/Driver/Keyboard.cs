using System;
using System.Collections.Generic;

namespace MOOS
{
    public delegate void OnKeyHandler(ConsoleKeyInfo key);

    public static class Keyboard
    {
        public static ConsoleKeyInfo KeyInfo;

        public static event OnKeyHandler OnKeyChanged
        {
            add
            {
                _KeyKeyChangeds.Add(value);
            }

            remove
            {
                _KeyKeyChangeds.Remove(value);
            }
        }

        static List<OnKeyHandler> _KeyKeyChangeds;
        static List<OnKeyHandler> KeyKeyChangeds { get { return _KeyKeyChangeds; } }

        public static void Initialize() 
        {
            _KeyKeyChangeds = new List<OnKeyHandler>();
        }

        internal static void InvokeOnKeyChanged(ConsoleKeyInfo info) 
        {
            for (int i = 0; i < KeyKeyChangeds.Count; i++)
            {
                KeyKeyChangeds[i]?.Invoke(info);
            }
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
    }
}
