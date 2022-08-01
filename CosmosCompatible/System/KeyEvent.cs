using System;

namespace Cosmos.System
{
    public struct KeyEvent
    {
        public enum KeyEventType
        {
            Make,
            Break
        }

        public char KeyChar;
        public ConsoleKeyEx Key;
        public ConsoleModifiers Modifiers;
        public KeyEventType Type;

        public KeyEvent()
        {
            KeyChar = '\0';
            Key = ConsoleKeyEx.NoName;
            Modifiers = ConsoleModifiers.None;
            Type = KeyEventType.Make;
        }

        public KeyEvent(char keyChar, ConsoleKeyEx key, bool shift, bool alt, bool control, KeyEventType type)
        {
            KeyChar = keyChar;
            Key = key;
            Modifiers = ConsoleModifiers.None;
            if (shift)
            {
                Modifiers |= ConsoleModifiers.Shift;
            }
            if (alt)
            {
                Modifiers |= ConsoleModifiers.Alt;
            }
            if (control)
            {
                Modifiers |= ConsoleModifiers.Ctrl;
            }
            Type = type;
        }
    }
}