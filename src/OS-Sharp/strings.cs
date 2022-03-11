namespace OS_Sharp
{
    internal unsafe class Strings
    {
        public static int Strlen(byte* c)
        {
            int i = 0;
            while (c[i] != 0)
            {
                i++;
            }

            return i;
        }
    }
}
