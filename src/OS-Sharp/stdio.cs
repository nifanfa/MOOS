using System.Runtime;
using System.Runtime.InteropServices;
using OS_Sharp.FileSystem;
using OS_Sharp.Misc;

namespace OS_Sharp
{
    internal unsafe class Stdio
    {
        //Not really stdio thing
        [RuntimeExport("print")]
        public static void Print(byte* msg)
        {
            byte b;
            while ((b = *msg++) != 0)
            {
                if (b == '\n')
                {
                    Console.WriteLine();
                }
                else
                {
                    Console.Write((char)b);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct FILE
        {
            public byte* DATA;
            public long OFFSET;
            public long LENGTH;
        }

        public enum SEEK
        {
            SET,
            CUR,
            END
        }

        [RuntimeExport("fopen")]
        public static FILE* Fopen(byte* name, byte* mode)
        {
            string sname = string.FromASCII((System.IntPtr)name, Strings.Strlen(name));
            FILE file = new();
            byte[] buffer = File.Instance.ReadAllBytes(sname);
            if (buffer == null)
            {
                Panic.Error("fopen: file not found!");
            }
            file.DATA = (byte*)Allocator.Allocate((ulong)buffer.Length);
            fixed (byte* p = buffer)
            {
                Native.Movsb(file.DATA, p, (ulong)buffer.Length);
            }

            file.LENGTH = buffer.Length;
            buffer.Dispose();
            sname.Dispose();
            return &file;
        }

        [RuntimeExport("fseek")]
        public static void Fseek(FILE* handle, long offset, SEEK seek)
        {
            if (seek == SEEK.SET)
            {
                handle->OFFSET = offset;
            }
            else
            {
                Panic.Error("Unknown seek");
            }
        }

        [RuntimeExport("fread")]
        public static void Fread(byte* buffer, long elementSize, long elementCount, FILE* handle)
        {
            Native.Movsb(buffer, handle->DATA + handle->OFFSET, (ulong)elementSize);
        }
    }
}
