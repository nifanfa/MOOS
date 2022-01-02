
public static unsafe class Console
{
    public const byte Width = 80;
    public const byte Height = 25;

    private static byte Color = 0;
    public static ulong CursorX = 0;
    public static ulong CursorY = 0;

    public static void Setup()
    {
        ResetColor();
        Clear();
    }

    public static void ResetColor()
    {
        BackgroundColor = ConsoleColor.Black;
        ForegroundColor = ConsoleColor.White;
    }

    public static void Write(char chr)
    {
        byte* p = ((byte*)(0xb8000 + (CursorY * Width * 2) + (CursorX * 2)));
        *p = (byte)chr;
        p++;
        *p = Color;
        CursorX++;
        if (CursorX == Width)
        {
            CursorX = 0;
            CursorY++;
        }
    }

    public static void WriteLine()
    {
        CursorX = 0;
        CursorY++;
    }

    public static void WriteAt(char chr, byte x, byte y)
    {
        byte* p = (byte*)0xb8000 + ((y * Width + x) * 2);
        *p = (byte)chr;
        p++;
        *p = Color;
    }

    public static void Clear()
    {
        CursorX = 0;
        CursorY = 0;
        int Res = Width * Height;
        for (int i = 0; i < Res; i++)
        {
            Write(' ');
        }
        CursorX = 0;
        CursorY = 0;
    }

    public static byte ForegroundColor
    {
        get { return (byte)(Color & 0x0F); }
        set { Color &= 0xF0; Color |= (byte)(value & 0x0F); }
    }

    public static byte BackgroundColor
    {
        get { return (byte)(Color >> 4); }
        set { Color &= 0x0F; Color |= (byte)((value & 0x0F) << 4); }
    }
}