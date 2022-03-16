// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence
namespace OS_Sharp.FileSystem
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