namespace System.Runtime.InteropServices
{
    [ComVisible(true)]
    public enum UnmanagedType
    {
        Bool = 2,
        I1 = 3,
        U1 = 4,
        I2 = 5,
        U2 = 6,
        I4 = 7,
        U4 = 8,
        I8 = 9,
        U8 = 10,
        R4 = 11,
        R8 = 12,
        Currency = 15,
        BStr = 19,
        LPStr = 20,
        LPWStr = 21,
        LPTStr = 22,
        ByValTStr = 23,
        IUnknown = 25,
        IDispatch = 26,
        Struct = 27,
        Interface = 28,
        SafeArray = 29,
        ByValArray = 30,
        SysInt = 31,
        SysUInt = 32,
        VBByRefStr = 34,
        AnsiBStr = 35,
        TBStr = 36,
        VariantBool = 37,
        FunctionPtr = 38,
        AsAny = 40,
        LPArray = 42,
        LPStruct = 43,
        CustomMarshaler = 44,
        Error = 45
    }
}