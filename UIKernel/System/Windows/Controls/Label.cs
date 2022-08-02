using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;

namespace System.Windows.Controls
{
    public class Label : Widget
    {
        public string Content { set; get; }
        public int FontSize { set; get; }
        public FontWeight FontWeight { get; set; }
        public HorizontalAlignment HorizontalContentAlignment { get; set; }
        public VerticalAlignment VerticalContentAlignment { get; set; }

        public Label()
        {
            FontWeight = new FontWeight();
            X = 0;
            Y = 0;
            Width = 300;
            Height = 42;
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            VerticalContentAlignment = VerticalAlignment.Stretch;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnDraw()
        {
            base.OnDraw();

            if (!string.IsNullOrEmpty(Content))
            {
                WindowManager.font.DrawString(X + (Width / 2) - ((WindowManager.font.MeasureString(Content)) / 2), (Y + (Height / 2)) - (WindowManager.font.FontSize / 2), Content, Foreground.Value);
            }

            if (BorderBrush != null)
            {
                DrawBorder();
            }
        }
    }
}
