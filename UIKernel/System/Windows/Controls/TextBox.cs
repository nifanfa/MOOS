using MOOS;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media;

namespace System.Windows.Controls
{
    public class TextBox : Widget
    {
        public string Text { set;get; }
        public int FontSize { set; get; }
        public int MaxLength { set; get; }
        public bool IsReadOnly { set; get; }
        public FontWeight FontWeight  {set; get; }
        public TextWrapping TextWrapping  {set; get; }
        public HorizontalAlignment HorizontalContentAlignment { get; set; }
        public VerticalAlignment VerticalContentAlignment { get; set; }

       

        /* Private */
        int start = 0;
        DateTime Flicker = DateTime.Now;
        bool isShowFlicker;

        public TextBox()
        {
            X = 0;
            Y = 0;
            Width = 300;
            Height = 42;
            FontWeight = new FontWeight();
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            VerticalContentAlignment = VerticalAlignment.Stretch;
            TextWrapping = TextWrapping.NoWrap;
            Background = Brushes.White;
            Cursor = new Cursor(CursorState.TextSelect);
            IsReadOnly = false;
            Keyboard.OnKeyChanged += Keyboard_OnKeyChanged;
        }

        void Keyboard_OnKeyChanged(ConsoleKeyInfo key)
        {
            if (IsReadOnly)
            {
                return;
            }
           
            if (IsFocus)
            {
                if (key.KeyState == System.ConsoleKeyState.Pressed)
                {
                    if (MaxLength > 0)
                    {
                        if (Text.Length > MaxLength)
                        {
                            return;
                        }
                    }

                    switch (key.Key)
                    {
                        case ConsoleKey.Backspace:
                            if (Text.Length > 0)
                            {
                                Text = Text.Substring(0, Text.Length -1);
                            }
                            break;
                        case ConsoleKey.Enter:

                            break;
                        default:
                            Text += key.KeyChar.ToString();
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

            int pos = 1;
            int w = 0, h = 0;
          
            int fnt = 0;
            int _w = (pos * fnt);

            Framebuffer.Graphics.FillRectangle(Color.FromArgb(Background.Value), X, Y, Width, Height);

            if (!string.IsNullOrEmpty(Text))
            {
                switch (this.TextWrapping)
                {
                    case TextWrapping.WrapWithOverflow:
                        //WrapWithOverflow
                        break;
                    case TextWrapping.NoWrap:

                        w = WindowManager.font.MeasureString(Text);
                        h = (Y + (Height / 2)) - (WindowManager.font.FontSize / 2);

                        if (w > Width)
                        {
                            start = ((w - Width) / WindowManager.font.FontSize);
                        }
                        else
                        {
                            start = 0;
                        }

                        for (int i = start; i < Text.Length; i++)
                        {
                            _w = fnt;

                            if (_w < (Width - WindowManager.font.FontSize))
                            {
                                fnt += WindowManager.font.DrawChar(Framebuffer.Graphics, X + _w, h, Text[i], Foreground.Value);
                            }
                            pos++;
                        }
                    break;
                    case TextWrapping.Wrap:
                        for (int i = 0; i < Text.Length; i++)
                        {
                            w += WindowManager.font.DrawChar(Framebuffer.Graphics, X + w, Y + h, Text[i], Foreground.Value);
                            if (w + WindowManager.font.FontSize > Width && Width != -1 || Text[i] == '\n')
                            {
                                w = 0;
                                h += WindowManager.font.FontSize;

                                if (Height != -1 && h >= Height)
                                {
                                    Framebuffer.Graphics.CopyFromScreen(X, Y, X, Y + WindowManager.font.FontSize, new Size(Width, Height - WindowManager.font.FontSize));

                                    h -= WindowManager.font.FontSize;
                                }
                            }
                        }
                        break;
                }
            }

            if (IsFocus)
            {
                if (isShowFlicker)
                {
                    Framebuffer.Graphics.DrawLine(Color.FromArgb(0xFF000000), (X + _w + (WindowManager.font.FontSize/2)), Y + 5, (X + _w + (WindowManager.font.FontSize)/2), (Y + Height) - 5);
                }

                if (DateTime.Now.Ticks >= Flicker.Ticks)
                {
                    isShowFlicker = !isShowFlicker;
                    Flicker = DateTime.Now.AddTicks(TimeSpan.FromMilliseconds(100).Ticks);
                }
            }

            if (BorderBrush != null)
            {
                DrawBorder();
            }
        }

    }
}
