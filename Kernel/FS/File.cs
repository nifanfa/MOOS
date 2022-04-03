/*
 * Copyright(c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
 */
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

        public abstract byte[] ReadAllBytes(string Name);
        public abstract string[] GetFiles();
    }
}
