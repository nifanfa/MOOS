using System.Runtime;

namespace OS_Sharp
{
    public static unsafe class Stdlib
    {
        [RuntimeExport("malloc")]
        public static void* Malloc(ulong size)
        {
            return (void*)Allocator.Allocate(size);
        }

        [RuntimeExport("free")]
        public static void Free(void* ptr)
        {
            Allocator.Free((System.IntPtr)ptr);
        }

        [RuntimeExport("realloc")]
        public static void* Realloc(void* ptr, ulong size)
        {
            return (void*)Allocator.Reallocate((System.IntPtr)ptr, size);
        }
    }
}
