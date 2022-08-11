using System.Collections.Generic;

namespace MOOS.FS
{
    public class FileInfo 
    {
        public string Name;
        public FileAttribute Attribute;

        public ulong Param0;
        public ulong Param1;

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

    public static class File
    {
        /// <summary>
        /// This will be overwritten if you initialize file system
        /// </summary>
        public static FileSystem Instance;

        public static List<FileInfo> GetFiles(string Directory) => Instance.GetFiles(Directory);
        public static byte[] ReadAllBytes(string name) => Instance.ReadAllBytes(name);
    }

    public abstract class FileSystem
    {
        public FileSystem()
        {
            File.Instance = this;
        }

        public const int SectorSize = 512;

        public static ulong SizeToSec(ulong size)
        {
            return ((size - (size % SectorSize)) / SectorSize) + ((size % SectorSize) != 0 ? 1ul : 0);
        }

        public static bool IsInDirectory(string fname, string dir)
        {
            if (fname.Length < dir.Length) return false;
            for (int i = 0; i < fname.Length; i++)
            {
                if (i < dir.Length)
                {
                    if (dir[i] != fname[i]) return false;
                }
                else
                {
                    if (fname[i] == '/') return false;
                }
            }
            return true;
        }

        public abstract List<FileInfo> GetFiles(string Directory);
        public abstract void Delete(string Name);
        public abstract byte[] ReadAllBytes(string Name);
        public abstract void WriteAllBytes(string Name, byte[] Content);
        public abstract void Format();
    }
}