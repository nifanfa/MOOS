/*
* Copyright (c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
*/


namespace System
{
    public struct ConsoleKeyInfo
    {
        public int ScanCode;
        public ConsoleKey Key;
        public char KeyChar;
        public ConsoleModifiers Modifiers;
        public ConsoleKeyState KeyState;
    }
}
