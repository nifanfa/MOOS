using Kernel.FS;
using Kernel.Misc;
using System.Runtime;
using System.Runtime.InteropServices;

namespace Kernel
{
    internal unsafe class stdio
    {
        //Not really stdio thing
        [RuntimeExport("print")]
        public static void print(byte* msg) 
        {
            byte b;
            while((b = *msg++) != 0) 
            {
                if (b == '\n') Console.WriteLine();
                else Console.Write((char)b);
            }
        }

        [StructLayout(LayoutKind.Sequential,Pack = 1)]
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
        public static FILE* fopen(byte* name, byte* mode) 
        {
            string sname = string.FromASCII((System.IntPtr)name, strings.strlen(name));
            FILE file = new FILE();
            byte[] buffer = File.Instance.ReadAllBytes(sname);
            if(buffer == null) 
            {
                Panic.Error("fopen: file not found!");
            }
            file.DATA = (byte*)Allocator.Allocate((ulong)buffer.Length);
            fixed (byte* p = buffer)
                Native.Movsb(file.DATA, p, (ulong)buffer.Length);
            file.LENGTH = buffer.Length;
            buffer.Dispose();
            sname.Dispose();
            return &file;
        }

        [RuntimeExport("fseek")]
        public static void fseek(FILE* handle,long offset,SEEK seek)
        {
            if (seek == SEEK.SET) 
            {
                handle->OFFSET = offset;
            }
            else 
            {
                Console.WriteLine("Unknown seek");
                for (; ; );
            }
        }

        [RuntimeExport("fread")]
        public static void fread(byte* buffer,long elementSize,long elementCount,FILE* handle)
        {
            Native.Movsb(buffer, handle->DATA + handle->OFFSET, (ulong)elementSize);
        }
    }
}
