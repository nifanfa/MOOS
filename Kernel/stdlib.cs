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


        [RuntimeExport("calloc")]
        public static void* calloc(ulong num,ulong size)
        {
            void* ptr = (void*)Allocator.Allocate(num * size);
            Native.Stosb(ptr, 0, num * size);
            return ptr;
        }

        [RuntimeExport("kmalloc")]
        public static void* kmalloc(ulong size)
        {
            return (void*)Allocator.Allocate(size);
        }

        [RuntimeExport("kfree")]
        public static void kfree(void* ptr)
        {
            Allocator.Free((System.IntPtr)ptr);
        }

        [RuntimeExport("krealloc")]
        public static void* krealloc(void* ptr, ulong size)
        {
            return (void*)Allocator.Reallocate((System.IntPtr)ptr, size);
        }


        [RuntimeExport("kcalloc")]
        public static void* kcalloc(ulong num, ulong size)
        {
            void* ptr = (void*)Allocator.Allocate(num * size);
            Native.Stosb(ptr, 0, num * size);
            return ptr;
        }
    }
}