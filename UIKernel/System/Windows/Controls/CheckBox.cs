using MOOS;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace System.Windows.Controls
{
    public class CheckBox : Widget
    {
        public string Content { set; get; }
        public bool IsChecked { set; get; }
        public HorizontalAlignment HorizontalAlignment { get; set; }
        public VerticalAlignment VerticalAlignment { get; set; }

        int _height = 24;
        int _width = 24;
        Brush _border;
        Brush _checked;
        bool _clicked;
        public CheckBox()
        {
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            Cursor = new Cursor(CursorState.Hand);
            Background = Brushes.LightGray;
            Foreground = Brushes.Black;
            _border = Brushes.Gray;
            _checked = Brushes.Green;
           
        }

        public override void Update()
        {
            base.Update();


            if (IsFocus)
            {
                if (Control.MouseButtons == MouseButtons.Left)
                {
                    if (!_clicked)
                    {
                        _clicked = true;
                        IsChecked = !IsChecked;
                    }
                }
            }
            
            if (Control.MouseButtons == MouseButtons.None)
            {
                _clicked = false;
            }
        }

        public override void Draw()
        {
            base.Draw();

            Framebuffer.Graphics.FillRectangle((X + (Width/2)) - (_width/2), (Y + (Height / 2)) - (_height / 2), _width, _height, Background.Value);
            Framebuffer.Graphics.DrawRectangle((X + (Width / 2)) - (_width / 2), (Y + (Height / 2)) - (_height / 2), _width + 1, _height + 1, _border.Value);

            if (IsChecked)
            {
                Framebuffer.Graphics.FillRectangle(((X + (Width / 2)) - (_width / 2)) + 2, ((Y + (Height / 2)) - (_height / 2)) + 2, _width - 3, _height - 3, _checked.Value);
            }
        }
    }
}
