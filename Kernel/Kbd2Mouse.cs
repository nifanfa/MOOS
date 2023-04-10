using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MOOS
{
    internal static class Kbd2Mouse
    {
        static bool up = false, down = false, left = false, right = false, f1 = false, f2 = false;

        internal static void OnKeyChanged(ConsoleKeyInfo keyInfo)
        {
            const int ratio = 10;

            if (keyInfo.Key == ConsoleKey.Up)
                up = keyInfo.KeyState == ConsoleKeyState.Pressed;
            if (keyInfo.Key == ConsoleKey.Down)
                down = keyInfo.KeyState == ConsoleKeyState.Pressed;
            if (keyInfo.Key == ConsoleKey.Left)
                left = keyInfo.KeyState == ConsoleKeyState.Pressed;
            if (keyInfo.Key == ConsoleKey.Right)
                right = keyInfo.KeyState == ConsoleKeyState.Pressed;

            //Console.WriteLine($"{up},{down},{left},{right}");

            if (
                keyInfo.Key == ConsoleKey.Up ||
                keyInfo.Key == ConsoleKey.Down ||
                keyInfo.Key == ConsoleKey.Left ||
                keyInfo.Key == ConsoleKey.Right
                )
            {
                int axisX = 0, axisY = 0;
                if (left)
                    axisX -= ratio;
                if (right)
                    axisX += ratio;
                if (up)
                    axisY -= ratio;
                if (down)
                    axisY += ratio;

                Control.MousePosition.X = Math.Clamp(Control.MousePosition.X + axisX, 0, Framebuffer.Width);
                Control.MousePosition.Y = Math.Clamp(Control.MousePosition.Y + axisY, 0, Framebuffer.Height);
            }

            if (keyInfo.Key == ConsoleKey.F1)
                f1 = keyInfo.KeyState == ConsoleKeyState.Pressed;
            if (keyInfo.Key == ConsoleKey.F2)
                f2 = keyInfo.KeyState == ConsoleKeyState.Pressed;

            if (f1)
                Control.MouseButtons |= MouseButtons.Left;
            else
                Control.MouseButtons &= ~MouseButtons.Left;
            if (f2)
                Control.MouseButtons |= MouseButtons.Right;
            else
                Control.MouseButtons &= ~MouseButtons.Right;
        }
    }
}
