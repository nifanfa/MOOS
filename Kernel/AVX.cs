using System.Runtime.InteropServices;

namespace Kernel
{
    public static class AVX
    {
        [DllImport("*")]
        public static extern void enable_avx();
    }
}
