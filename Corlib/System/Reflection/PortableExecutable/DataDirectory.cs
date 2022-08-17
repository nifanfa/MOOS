using System.Runtime.InteropServices;

namespace System.Reflection.PortableExecutable
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DataDirectory
    {
        public uint VirtualAddress;
        public uint Size;
    }
}