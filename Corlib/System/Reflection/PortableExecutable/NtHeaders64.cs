using System.Runtime.InteropServices;

namespace System.Reflection.PortableExecutable
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct NtHeaders64
    {
        public uint Signature;
        public FileHeader FileHeader;
        public OptionalHeaders64 OptionalHeader;
    }
}