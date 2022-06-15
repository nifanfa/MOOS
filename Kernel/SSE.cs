using System.Runtime.InteropServices;

namespace MOOS
{
    internal static class SSE
    {
        [DllImport("*")]
        public static extern void enable_sse();
    }
}