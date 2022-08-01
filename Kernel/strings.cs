namespace MOOS
{
    internal unsafe class strings
    {
        public static int strlen(byte* c) 
        {
            int i = 0;
            while (c[i] != 0) i++;
            return i;
        }
    }
}