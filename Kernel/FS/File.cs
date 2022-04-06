/*
 * Copyright(c) 2022 nifanfa, This code is part of the Solution1 licensed under the MIT licence.
 */
using System.Collections.Generic;

namespace Kernel.FS
{
    public abstract class File
    {
        /// <summary>
        /// This will be overwritten if you initialize file system
        /// </summary>
        public static File Instance;

        public File()
        {
            Instance = this;
        }

        public abstract List<string> GetFiles(string Directory);
        public abstract void Delete(string Name);
        public abstract byte[] ReadAllBytes(string Name);
        public abstract void WriteAllBytes(string Name, byte[] Content);
    }
}
