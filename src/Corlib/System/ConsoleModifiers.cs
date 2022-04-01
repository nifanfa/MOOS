/*
* Copyright (c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
*/


namespace System
{
    [Flags]
    public enum ConsoleModifiers
    {
        None = 0,
        Alt = 1,
        Shift = 2,
        CapsLock = 3,
        Ctrl = 4,
    }
}
