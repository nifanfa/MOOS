namespace System.Reflection.PortableExecutable
{
    enum SubSystemType : ushort
    {
        Unknown = 0,
        Native = 1,
        WindowsGUI = 2,
        WindowsCUI = 3,
        PosixCUI = 7,
        WindowsCEGui = 9,
        EfiApplication = 10,
        EfiBootServiceDriver = 11,
        EfiRuntimeDriver = 12,
        EfiRom = 13,
        Xbox = 14
    }
}