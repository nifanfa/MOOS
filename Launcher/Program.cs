// See https://aka.ms/new-console-template for more information
using Launcher;
using System.Diagnostics;
using System.IO.Compression;

Console.WriteLine("Hello, World!");

string Env = Environment.CurrentDirectory;
Environment.CurrentDirectory = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.IndexOf("\\bin"));
Utility.Start("dotnet", "publish -r win-x64 -c debug Kernel");
Environment.CurrentDirectory = Env;

Utility.Start(@"nasm.exe", "-fbin EntryPoint.asm -o entry");
byte[] Loader = File.ReadAllBytes("entry");
byte[] Kernel = File.ReadAllBytes(@"win-x64\native\Kernel.exe");
List<byte> list = new List<byte>();
list.AddRange(Loader);
list.AddRange(Kernel);
File.WriteAllBytes("kernel", list.ToArray());
ZipFile.ExtractToDirectory(@"grub2.zip", @"tmp\", true);
File.Move(@"kernel", @"tmp\boot\kernel", true);
Utility.Start(@"mkisofs.exe", $"-relaxed-filenames -J -R -o \"{Environment.CurrentDirectory}\\output.iso\" -b \"{@"boot/grub/i386-pc/eltorito.img"}\" -no-emul-boot -boot-load-size 4 -boot-info-table {Environment.CurrentDirectory}\\tmp");
if (!File.Exists(@"C:\Program Files\qemu\qemu-system-x86_64.exe"))
    Console.WriteLine("Qemu is not installed");
else
    Process.Start(@"C:\Program Files\qemu\qemu-system-x86_64.exe", "-m 8192 -cdrom output.iso");