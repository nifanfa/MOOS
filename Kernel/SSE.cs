using System.Runtime.InteropServices;

namespace Kernel
{
    internal static class SSE
    {
        [DllImport("*")]
        public static extern void enable_sse();
    }
}
