using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct SimpleTextOutputMode
{
    public int MaxMode;

    // current settings
    public int Mode;
    public int Attribute;
    public int CursorColumn;
    public int CursorRow;
    public bool CursorVisible;
}

[StructLayout(LayoutKind.Sequential)]
public readonly unsafe struct SimpleTextOutputInterface
{
    public readonly delegate* <SimpleTextOutputInterface*, bool, EfiStatus> Reset;

    public readonly delegate* <SimpleTextOutputInterface*, char*, EfiStatus> OutputString;
    public readonly delegate* <SimpleTextOutputInterface*, bool, EfiStatus> TestString;

    public readonly delegate* <SimpleTextOutputInterface*, ulong, ulong*, ulong*, EfiStatus> QueryMode;
    public readonly delegate* <SimpleTextOutputInterface*, ulong, EfiStatus> SetMode;
    public readonly delegate* <SimpleTextOutputInterface*, EfiTextAttribute, EfiStatus> SetAttribute;

    public readonly delegate* <SimpleTextOutputInterface*, EfiStatus> ClearScreen;
    public readonly delegate* <SimpleTextOutputInterface*, ulong, ulong, EfiStatus> SetCursorPosition;
    public readonly delegate* <SimpleTextOutputInterface*, bool, EfiStatus> EnableCursor;
    public readonly SimpleTextOutputMode* Mode;
}

[Flags]
public enum EfiTextAttribute : ulong
{
    EfiBlack = 0x00,
    EfiBlue = 0x01,
    EfiGreen = 0x02,
    EfiCyan = EfiBlue | EfiGreen,
    EfiRed = 0x04,
    EfiMagenta = EfiBlue | EfiRed,
    EfiBrown = EfiGreen | EfiRed,
    EfiLightgray = EfiBlue | EfiGreen | EfiRed,
    EfiBright = 0x08,
    EfiDarkgray = EfiBright,
    EfiLightblue = EfiBlue | EfiBright,
    EfiLightgreen = EfiGreen | EfiBright,
    EfiLightcyan = EfiCyan | EfiBright,
    EfiLightred = EfiRed | EfiBright,
    EfiLightmagenta = EfiMagenta | EfiBright,
    EfiYellow = EfiBrown | EfiBright,
    EfiWhite = EfiBlue | EfiGreen | EfiRed | EfiBright,
    EfiBackgroundBlack = 0x00,
    EfiBackgroundBlue = 0x10,
    EfiBackgroundGreen = 0x20,
    EfiBackgroundCyan = EfiBackgroundBlue | EfiBackgroundGreen,
    EfiBackgroundRed = 0x40,
    EfiBackgroundMagenta = EfiBackgroundBlue | EfiBackgroundRed,
    EfiBackgroundBrown = EfiBackgroundGreen | EfiBackgroundRed,
    EfiBackgroundLightgray = EfiBackgroundBlue | EfiBackgroundGreen | EfiBackgroundRed
}

public enum EfiScanCode : ushort
{
    ScanNull = 0x0000,
    ScanUp = 0x0001,
    ScanDown = 0x0002,
    ScanRight = 0x0003,
    ScanLeft = 0x0004,
    ScanHome = 0x0005,
    ScanEnd = 0x0006,
    ScanInsert = 0x0007,
    ScanDelete = 0x0008,
    ScanPageUp = 0x0009,
    ScanPageDown = 0x000A,
    ScanF1 = 0x000B,
    ScanF2 = 0x000C,
    ScanF3 = 0x000D,
    ScanF4 = 0x000E,
    ScanF5 = 0x000F,
    ScanF6 = 0x0010,
    ScanF7 = 0x0011,
    ScanF8 = 0x0012,
    ScanF9 = 0x0013,
    ScanF10 = 0x0014,
    ScanF11 = 0x0015,
    ScanF12 = 0x0016,
    ScanEsc = 0x0017
}


[StructLayout(LayoutKind.Sequential)]
public struct EFI_INPUT_KEY
{
    public EfiScanCode ScanCode;
    public char UnicodeChar;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct SIMPLE_INPUT_INTERFACE
{
    public readonly delegate* <SIMPLE_INPUT_INTERFACE*, bool, EfiStatus> Reset;
    public readonly delegate* <SIMPLE_INPUT_INTERFACE*, EFI_INPUT_KEY*, EfiStatus> ReadKeyStroke;
    public readonly EfiEvent WaitForKey;
}