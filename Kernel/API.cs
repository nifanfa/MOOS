using System.Solution1;

namespace Kernel
{
    public static unsafe class API
    {
        public static SystemTable st;

        public static void Initialize() 
        {
            st = new SystemTable();
            st.Console_WriteLine = &Console.WriteLine;
        }
    }
}
