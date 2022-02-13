namespace Kernel.FS
{
    public abstract class File
    {
        public static File Instance;

        public File()
        {
            Instance = this;
        }

        public abstract byte[] ReadAllBytes(string Name);
    }
}
