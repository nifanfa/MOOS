using MOOS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media;

namespace System.Windows.Controls
{
    public class Slider : Widget
    {
        public double SmallChange { get; set; }
        public double Value { get; set; }
        public double Minimum { get; set; }
        public double Maximum { get; set; }
        public double LargeChange { get; set; }

        int _height = 5;
        int _slideH = 20;
        int _slideW = 10;
        Brush _border;
        public Slider()
        {
            X = 0;
            Y = 0;

            Width = 300;
            Height = _height;
            Background = Brushes.Transparent;
            Foreground = Brushes.LightGray;
            _border = Brushes.Gray;
            BorderBrush = null;
            Minimum = 0;
            Value = 0;
            Maximum = 10;
            Keyboard.OnKeyChanged += Keyboard_OnKeyChanged1;
        }

        void Keyboard_OnKeyChanged1(ConsoleKeyInfo key)
        {
            if (IsFocus)
            {
                if (key.KeyState == System.ConsoleKeyState.Pressed)
                {
                    switch (key.Key)
                    {
                        case ConsoleKey.Left:
                            if (Value > Minimum)
                            {
                                Value--;
                            }
                            break;
                        case ConsoleKey.LeftWindows:
                            if (Value > Minimum)
                            {
                                Value--;
                            }
                            break;
                        case ConsoleKey.Right:
                            if (Value < Maximum)
                            {
                                Value++;
                            }
                            break;
                        case ConsoleKey.RightWindows:
                            if (Value < Maximum)
                            {
                                Value++;
                            }
                            break;
                    }
                }
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnDraw()
        {
            base.OnDraw();

            int yb = (Y + (_height + (_height / 2)));

            Framebuffer.Graphics.FillRectangle(Color.FromArgb(Foreground.Value), X, yb, Width, _height);

            Framebuffer.Graphics.DrawRectangle(Color.FromArgb(_border.Value), X - 1, yb - 1, Width + 1, _height+1);

            int _xs = X + ((int)Value * (int)(Width / Maximum)) - (_slideW/2);

            Framebuffer.Graphics.FillRectangle(Color.FromArgb(Foreground.Value), _xs, Y, _slideW, _slideH);
            Framebuffer.Graphics.DrawRectangle(Color.FromArgb(_border.Value), _xs - 1, Y - 1, _slideW + 1, _slideH + 1);
            Framebuffer.Graphics.DrawRectangle(Color.FromArgb(_border.Value), _xs - 2, Y - 2, _slideW + 3, _slideH + 3);

            if (BorderBrush != null)
            {
                DrawBorder();
            }
        }
    }
}
