using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace System.Desktops.Controls
{
    public class DesktopControl
    {
        public int X { set; get; }
        public int Y { set; get; }
        public int Width { set; get; }
        public int Height { set; get; }
        public string Name { set; get; }
        public string Content { set; get; }
        public Brush Background { set; get; }
        public Brush Foreground { set; get; }
        public HorizontalAlignment HorizontalAlignment { set; get; }
        public VerticalAlignment VerticalAlignment { set; get; }
        public ICommand Command { set; get; }
        public object CommandParameter { set; get; }

        public DesktopControl()
        {
            Background = Brushes.White;
            Foreground = Brushes.Black;
        }

        public virtual void Update()
        {
           
        }

        public virtual void Draw()
        { 
        
        }
    }
}
