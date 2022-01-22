
namespace System
{
    public readonly struct KeyInfo
    {
        public readonly int ScanCode;
        public readonly Key Key;
        public readonly char KeyChar;
        public readonly KeyModifier Modifiers;


        public KeyInfo(int scan, Key key, char keyChar, bool alt, bool shift, bool ctrl)
        {
            ScanCode = scan;
            Key = key;
            KeyChar = keyChar;
            Modifiers = KeyModifier.None;

            if (alt) Modifiers |= KeyModifier.Alt;
            if (shift) Modifiers |= KeyModifier.Shift;
            if (ctrl) Modifiers |= KeyModifier.Ctrl;
        }
    }
}