using System.Runtime;
using System.Runtime.InteropServices;

namespace Kernel.FS
{
    public unsafe class FATFS : File
    {
        [DllImport("*")]
        private static extern void fatfs_init();

        public FATFS()
        {
            fatfs_init();
            /*
            WriteAllBytes("0:/Hello.txt", new byte[]
            {
                (byte)'F',
                (byte)'u',
                (byte)'c',
                (byte)'k',
                (byte)' ',
                (byte)'u'
            });

            byte[] buffer = ReadAllBytes("0:/Hello.txt");
            for (int i = 0; i < buffer.Length; i++)
            {
                Console.Write((char)buffer[i]);
            }
            while (true) ;
            */
        }

        [RuntimeExport("get_fattime")]
        public static uint get_fattime() => 0;

        [RuntimeExport("RAM_disk_write")]
        public static int RAM_disk_write(byte* buffer, ulong sector, uint count)
        {
            Disk.Instance.Write(sector, count, buffer);
            return 0;
        }

        [RuntimeExport("RAM_disk_read")]
        public static int RAM_disk_read(byte* buffer, ulong sector, uint count)
        {
            Disk.Instance.Read(sector, count, buffer);
            return 0;
        }

        [DllImport("*")]
        private static extern uint read_all_bytes(char* filename, out void* data);

        [DllImport("*")]
        private static extern void write_all_bytes(char* filename, void* data, long filesize);

        public override byte[] ReadAllBytes(string Name)
        {
            fixed (char* p = Name)
            {
                uint size = read_all_bytes((char*)p, out void* data);
                byte[] buffer = new byte[size];
                fixed (byte* pp = buffer)
                {
                    Native.Movsb(pp, data, size);
                }
                Allocator.Free((System.IntPtr)data);
                return buffer;
            }
        }

        [DllImport("*")]
        public static extern int f_unlink(char* filename);

        public override void Delete(string Name) 
        {
            fixed(char* ptr = Name) 
            {
                f_unlink(ptr);
            }
        }

        public override void WriteAllBytes(string Name, byte[] Content) 
        {
            fixed(char* fname = Name) 
            {
                fixed (byte* buffer = Content)
                {
                    write_all_bytes(fname, buffer, Content.Length);
                }
            }
        }
    }
}
