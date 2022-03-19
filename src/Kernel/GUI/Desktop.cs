using OS_Sharp;
using OS_Sharp.FileSystem;
using OS_Sharp.GUI;
using OS_Sharp.Misc;
using System.Drawing;

namespace Kernel.GUI
{
    internal class Desktop
    {
        private static Image FileIcon;
        public static string CurrentDirectory;

        public static void Initialize() 
        {
            FileIcon = new PNG(File.Instance.ReadAllBytes("/UNKNOWN.PNG"));
            CurrentDirectory = " root@OS-Sharp: / ";
        }

        public static void Update()
        {
            const int BarHeight = 35;

            string[] names = File.Instance.GetFiles();
            int Devide = 60;
            int X = Devide;
            int Y = Devide + BarHeight;
            for (int i = 0; i < names.Length; i++)
            {
                if (Y + FileIcon.Height + Devide > Framebuffer.Height - Devide) 
                {
                    Y = Devide + BarHeight;
                    X += FileIcon.Width + Devide;
                }

                Framebuffer.DrawImage(X, Y, FileIcon);
                Form.font.DrawString(X, Y + FileIcon.Height, names[i], FileIcon.Width);
                Y += FileIcon.Height + Devide;
            }
            names.Dispose();

            Framebuffer.Fill(0, 0, Framebuffer.Width, BarHeight, 0xFF101010);
            Form.font.DrawString(0, (BarHeight / 2) - (Form.font.Height / 2), CurrentDirectory, Framebuffer.Width);

            string CPUUsage = ThreadPool.CPUUsage.ToString();
            string Str = "CPU 0: %";
            string Result = Str + CPUUsage;
            Form.font.DrawString(Framebuffer.Width - Form.font.MeasureString(Result) - Form.font.Width, (BarHeight / 2) - (Form.font.Height / 2), Result);
            CPUUsage.Dispose();
            Str.Dispose();
            Result.Dispose();
        }
    }
}
