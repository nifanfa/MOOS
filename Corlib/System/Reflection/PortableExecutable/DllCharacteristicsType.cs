namespace System.Reflection.PortableExecutable
{
    enum DllCharacteristicsType : ushort
    {
        RES_0 = 0x0001,
        RES_1 = 0x0002,
        RES_2 = 0x0004,
        RES_3 = 0x0008,
        DynamicBase = 0x0040,
        ForceIntegrity = 0x0080,
        NxCompat = 0x0100,
        NoIsolation = 0x0200,
        NoSEH = 0x0400,
        NoBind = 0x0800,
        RES_4 = 0x1000,
        WDMDriver = 0x2000,
        TerminalServerName = 0x8000
    }
}