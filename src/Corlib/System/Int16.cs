/*
* Copyright (c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
*/

namespace System
{
    public struct Int16
    {
        public override string ToString()
        {
            return ((long)this).ToString();
        }
    }
}
