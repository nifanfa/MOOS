using System.Runtime;

namespace MOOS
{
    public static unsafe class stdlib
    {
        [RuntimeExport("malloc")]
        public static void* malloc(ulong size) 
        {
            return (void*)Allocator.Allocate(size);
        }

        [RuntimeExport("free")]
        public static void free(void* ptr) 
        {
            Allocator.Free((System.IntPtr)ptr);
        }

        [RuntimeExport("realloc")]
        public static void* realloc(void* ptr, ulong size) 
        {
            return (void*)Allocator.Reallocate((System.IntPtr)ptr, size);
        }
    }
}