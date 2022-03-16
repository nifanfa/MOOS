// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence
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
