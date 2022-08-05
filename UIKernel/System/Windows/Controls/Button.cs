#if HasGUI
using MOOS;
using System;
using System.Drawing;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;

namespace System.Windows.Controls
{
    public class Button : Widget
    {
        public string Content { set; get; }
        public Binding Command { set; get; }
        public static object CommandProperty { get;  set; }
        public object CommandParameter { get; set; }

        public Button()
        {
            X = 0;
            Y = 0;
            Width = 300;
            Height = 42;
            Background = new Brush(0xFF111111);
            CommandParameter = string.Empty;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (MouseEnter)
            {
                this.onMouseFocus();

                if (IsFocus)
                {
                    if (Control.Clicked)
                    {
                        if (Command != null && Command.Source != null)
                        {
                            Command.Source.Execute.Invoke(CommandParameter);
                        }
                    }
                }
            }
            else
            {
                this.onMouseLostFocus();
            }

        }

        public override void OnDraw()
        {
            base.OnDraw();

            Framebuffer.Graphics.FillRectangle(Color.FromArgb(Background.Value), X, Y, Width, Height);

            if (!string.IsNullOrEmpty(Content))
            {
                WindowManager.font.DrawString(X + (Width / 2) - ((WindowManager.font.MeasureString(Content)) / 2),(Y + (Height / 2) ) - (WindowManager.font.FontSize/2) , Content, Foreground.Value);
            }

            if (BorderBrush != null)
            {
                DrawBorder();
            }
        }

        public void SetBinding(object commandProperty, Binding binding)
        {
            Command = binding;
        }
    }
}
#endif