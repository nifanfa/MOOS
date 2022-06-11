/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
using System.Collections.Generic;

namespace MOOS.FS
{
    public class FileInfo 
    {
        public string Name;
        public FileAttribute Attribute;

        public override void Dispose()
        {
            Name.Dispose();
            base.Dispose();
        }
    }

    public enum FileAttribute : byte
    {
        ReadOnly = 0x01,
        Hidden = 0x02,
        System = 0x04,
        Directory = 0x10,
        Archive = 0x20,
    }

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

        public abstract List<FileInfo> GetFiles(string Directory);
        public abstract void Delete(string Name);
        public abstract byte[] ReadAllBytes(string Name);
        public abstract void WriteAllBytes(string Name, byte[] Content);
        public abstract void Format();
    }
}
