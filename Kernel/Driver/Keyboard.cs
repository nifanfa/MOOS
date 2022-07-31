using System;

namespace MOOS
{
    public delegate void OnKeyHandler(ConsoleKeyInfo key);

    public static class Keyboard
    {
        public static ConsoleKeyInfo KeyInfo;

        public static event OnKeyHandler OnKeyChanged;

        public static void Initialize()
        {
            OnKeyChanged = null;
        }

        internal static void InvokeOnKeyChanged(ConsoleKeyInfo info)
        {
            OnKeyChanged?.Invoke(info);
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
