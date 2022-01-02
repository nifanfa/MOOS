namespace System
{
    public sealed unsafe class String 
    {
        //This layout is an ILC contract
        public readonly int Length;
        private char FirstChar;

        public char this [int index] 
        {
            get 
            {
                fixed(char* p = &FirstChar) 
                {
                    return p[index];
                }
            }
        }
    }
}
