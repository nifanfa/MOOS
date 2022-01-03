using System;
using System.Diagnostics;

namespace Launcher
{
    internal class Utility
    {
        public static void Start(string File, string Arguments)
        {
            string currentd = Environment.CurrentDirectory;
            Environment.CurrentDirectory = new FileInfo(File).DirectoryName;
            var v = Process.Start(File, Arguments);
            v.WaitForExit();
            Environment.CurrentDirectory = currentd;
        }
    }
}
