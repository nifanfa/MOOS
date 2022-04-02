using Kernel.Driver;
using Kernel.Misc;
using System.Drawing;

namespace Kernel.GUI
{
    internal class FConsole : Form
    {
        string Data;
        public Image ScreenBuf;
        string Cmd;

        public FConsole(int X, int Y) : base(X, Y, 640, 320)
        {
            Title = "Console";
            Cmd = string.Empty;
            Data = string.Empty;
            BackgroundColor = 0xFF101010;
            ScreenBuf = new Image(640, 320);

            Console.OnWrite += Console_OnWrite;
            PS2Keyboard.OnKeyChanged += PS2Keyboard_OnKeyChanged;
            Console.WriteLine("Type help to get information!");
        }

        private void PS2Keyboard_OnKeyChanged(System.ConsoleKeyInfo key)
        {
            if (key.KeyState == System.ConsoleKeyState.Pressed)
            {
                if (key.Key == System.ConsoleKey.Backspace)
                {
                    if (Data.Length != 0)
                        Data.Length -= 1;
                }
                else if (key.KeyChar != '\0')
                {
                    Console_OnWrite(key.KeyChar);

                    string cs = key.KeyChar.ToString();
                    string cache1 = Cmd;
                    Cmd = cache1 + cs;
                    cache1.Dispose();
                }

                if (key.Key == System.ConsoleKey.Enter)
                {
                    if (Cmd.Length != 0) Cmd.Length -= 1;

                    // when a command is invoked
                    switch (Cmd)
                    {
                        case "hello":
                            Panic.Error(": )");
                            break;

                        case "help":
                            Console.WriteLine("OS_Sharp Operating System https://github.com/nifanfa/OS-Sharp");
                            break;

                        case "shutdown":
                            ACPI.Shutdown();
                            break;

                        default:
                            Console.Write("No such command: \"");
                            Console.Write(Cmd);
                            Console.WriteLine("\"");
                            break;
                    }

                    Cmd.Dispose();
                    Cmd = string.Empty;
                }
                else if (key.Key == System.ConsoleKey.Backspace) if (Cmd.Length != 0) Cmd.Length -= 1;
            }
        }

        public override void Update()
        {
            base.Update();
            int w = 0, h = 0;

            string cur = "_";
            string s = Data + cur;
            BitFont.DrawString("Song", 0xFFFFFFFF, s, X, Y, Width);
            //font.DrawString(X, Y, s, Width);
            cur.Dispose();
            s.Dispose();
            //BitFont.DrawString("Song", 0xFFFFFFFF, Data, X, Y, 640);
        }

        private void Console_OnWrite(char chr)
        {
            string cs = chr.ToString();
            string cache = Data;
            Data = cache + cs;
            cs.Dispose();
            cache.Dispose();
        }
    }
}
