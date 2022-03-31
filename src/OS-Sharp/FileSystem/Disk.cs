/*
* Copyright (c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
*/

namespace OS_Sharp.FileSystem
{
    public abstract class Disk
    {
        public abstract bool Read(ulong sector, uint count, byte[] data);
        public abstract bool Write(ulong sector, uint count, byte[] data);
    }
}
