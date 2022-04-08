/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
namespace Kernel
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
