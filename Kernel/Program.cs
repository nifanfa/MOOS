using System.Runtime;

unsafe class Program
{
    [RuntimeExport("Main")]
    static void Main()
    {
        //You don't need to build. Just save and run Launcher
        ulong* p = (ulong*)0xb8000;
        p[0] = 0x1F6C1F6C1F651F48;
        p[1] = 0x1F6F1F571F201F6F;
        p[2] = 0x1F211F641F6C1F72;
        for (; ; );
    }
}
