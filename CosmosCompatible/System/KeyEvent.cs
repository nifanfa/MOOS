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
            this.Modifiers = ConsoleModifiers.None;
            Type = KeyEventType.Make;
        }

        public KeyEvent(char keyChar, ConsoleKeyEx key, bool shift, bool alt, bool control, KeyEventType type)
        {
            this.KeyChar = keyChar;
            this.Key = key;
            this.Modifiers = ConsoleModifiers.None;
            if (shift)
            {
                this.Modifiers |= ConsoleModifiers.Shift;
            }
            if (alt)
            {
                this.Modifiers |= ConsoleModifiers.Alt;
            }
            if (control)
            {
                this.Modifiers |= ConsoleModifiers.Control;
            }
            this.Type = type;
        }
    }
}