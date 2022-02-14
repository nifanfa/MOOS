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
    }
}
