#if HasGUI

using MOOS.Driver;
using MOOS.Graph;
using MOOS.Misc;
using System.Drawing;

namespace MOOS.GUI
{
    internal class FConsole : Window
    {
        string Data;
        public Image ScreenBuf;
        string Cmd;

        public FConsole(int X, int Y) : base(X, Y, 640, 320)
        {
#if Chinese
            Title = "控制台";
#else
            Title = "Console";
#endif
            Cmd = string.Empty;
            Data = string.Empty;
            ScreenBuf = new Image(640, 320);

            Rebind();
            Console.WriteLine("Type help to get information!");

            Console.OnWrite += Console_OnWrite;
        }

        public void Rebind()
        {
            Keyboard.OnKeyChanged += PS2Keyboard_OnKeyChanged;
        }

        private void PS2Keyboard_OnKeyChanged(object sender, System.ConsoleKeyInfo key)
        {
            if (key.KeyState == System.ConsoleKeyState.Pressed && Program.FConsole.Visible)
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
                            int a = 1;
                            int b = 0;
                            int c = a / b;
                            break;

                        case "help":
                            Console.WriteLine("help: to get this information");
                            Console.WriteLine("shutdown: power off");
                            Console.WriteLine("reboot: reboot this computer");
                            Console.WriteLine("cpu: list cpu information");
                            Console.WriteLine("hello: issue kernel panic");
                            break;

                        case "shutdown":
                            Power.Shutdown();
                            break;

                        case "cpu":
                            Console.WriteLine("multi-processor IDs:");
                            for (int i = 0; i < ACPI.LocalAPIC_CPUIDs.Count; i++)
                                Console.WriteLine($" cpu id:{ACPI.LocalAPIC_CPUIDs[i]}");
                            Console.WriteLine($"frequency: {Timer.CPU_Clock/1048576}mhz");
                            break;

                        case "null":
                            unsafe
                            {
                                uint* ptr = null;
                                *ptr = 0xDEADBEEF;
                            }
                            break;

                        case "reboot":
                            Power.Reboot();
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

        public override void OnDraw()
        {
            base.OnDraw();
            int w = 0, h = 0;

            string s0 = "_";
            string s1 = Data + s0;
            //BitFont.DrawString("Song", 0xFFFFFFFF, s, X, Y, Width);
            DrawString(X, Y, s1, Height, Width);
            s0.Dispose();
            s1.Dispose();
            //BitFont.DrawString("Song", 0xFFFFFFFF, Data, X, Y, 640);
        }

        public void DrawString(int X, int Y, string Str,int HeightLimit = -1, int LineLimit = -1)
        {
            int w = 0, h = 0;
            for (int i = 0; i < Str.Length; i++)
            {
                w += WindowManager.font.DrawChar(Framebuffer.Graphics,X + w, Y + h, Str[i]);
                if (w + WindowManager.font.FontSize > LineLimit && LineLimit != -1 || Str[i] == '\n')
                {
                    w = 0;
                    h += WindowManager.font.FontSize;

                    if(HeightLimit != -1 && h >= HeightLimit)
                    {
                        Framebuffer.Graphics.Copy(X, Y, X, Y + WindowManager.font.FontSize, LineLimit, HeightLimit - (WindowManager.font.FontSize));
                        Framebuffer.Graphics.FillRectangle(X, Y + HeightLimit - (WindowManager.font.FontSize), LineLimit, WindowManager.font.FontSize, 0xFF222222);
                        h -= WindowManager.font.FontSize;
                    }
                }
            }
        }

        private void Console_OnWrite(char chr)
        {
            if (Program.FConsole.Visible == false)
            {
                WindowManager.MoveToEnd(Program.FConsole);
                Program.FConsole.Visible = true;
            }

            string cs = chr.ToString();
            string cache = Data;
            Data = cache + cs;
            cs.Dispose();
            cache.Dispose();
        }
    }
}
#endif