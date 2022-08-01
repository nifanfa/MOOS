using System;

namespace Cosmos.System
{
    internal class KeyboardManager
    {
        static KeyEvent KeyEvent;

        internal static bool TryReadKey(out KeyEvent key)
        {
            KeyEvent.Key = (ConsoleKeyEx)MOOS.Keyboard.KeyInfo.Key;
            KeyEvent.KeyChar = MOOS.Keyboard.KeyInfo.KeyChar;
            key = KeyEvent;

            bool doReturn = MOOS.Keyboard.KeyInfo.KeyState.HasFlag(ConsoleKeyState.Pressed) && MOOS.Keyboard.KeyInfo.KeyChar != '\0' ;

            MOOS.Keyboard.CleanKeyInfo();
            return doReturn;
        }
    }
}
