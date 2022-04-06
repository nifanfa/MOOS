using System.Collections.Generic;
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


        [DllImport("*")]
        private static extern char* get_files(char* directory);

        public override List<string> GetFiles(string Directory) 
        {
            List<string> files = new List<string>();
            char* c;
            fixed (char* p = Directory) c = get_files(p);
            int i = 0;
            bool atEnd = false;
            while (true) 
            {
                int len = 0;
                while (c[i+ len] != '\n')
                {
                    if (c[i + len] == 0)
                    {
                        atEnd = true;
                        break;
                    }

                    len++;
                }
                if (atEnd) break;
                files.Add(new string(c, i, len));
                i += len;
                i++;
            }

            return files;
        }

        [RuntimeExport("get_fattime")]
        public static uint get_fattime() 
        {
            uint year = RTC.Year;
            //TO-DO 2100year
            year += 2000;
            uint month = RTC.Month;
            uint day = RTC.Day;
            uint hour = RTC.Hour;
            uint minute = RTC.Minute;
            uint second = RTC.Second;

            year -= 1980;
            second /= 2;

            year <<= 25;
            month <<= 21;
            day <<= 16;
            hour <<= 11;
            minute <<= 5;

            uint result = year | month | day | hour | minute | second;
            return result;
        }

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
