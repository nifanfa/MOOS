using System.Runtime;

namespace Kernel
{
    public static unsafe class stdlib
    {
        [RuntimeExport("__imp_malloc")]
        public static void* malloc(ulong size) 
        {
            return (void*)Allocator.Allocate(size);
        }

        [RuntimeExport("__imp_malloc")]
        public static void free(void* ptr) 
        {
            Allocator.Free((System.IntPtr)ptr);
        }

        [RuntimeExport("__imp_realloc")]
        public static void* realloc(void* ptr, ulong size) 
        {
            return Allocator.Reallocate(ptr, size);
        }
    }
}
