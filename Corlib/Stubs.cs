using System.Runtime;

class Stubs
{
    [RuntimeExport("__fail_fast")]
    static void FailFast() { while (true) ; }

    [RuntimeExport("memset")]
    static unsafe void MemSet(byte* ptr, int c, int count)
    {
        for (byte* p = ptr; p < ptr + count; p++)
            *p = (byte)c;
    }
}