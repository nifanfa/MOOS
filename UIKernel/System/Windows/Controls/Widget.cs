
using MOOS;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace System.Windows.Controls
{
    public class Position
    {
        public int X { set; get; }
        public int Y { set; get; }
        public int Width { set; get; }
        public int Height { set; get; }
    }

    public class Widget
    {
        Cursor _cursor;
        Brush _background;
        public string Name { set; get; }
        public int X { set; get; }
        public int Y { set; get; }
        public int Width { set; get; }
        public int Height { set; get; }
        public bool MouseEnter { set; get; }
        public Thickness Margin { set; get; }
        public Thickness Padding { set; get; }
        
        public Brush Background
        {
            set
            {
                _background = value;
            }
            get { return _background; }
        }

        public Cursor Cursor
        {
            set
            {
                _cursor = value;
            }
            get { return _cursor; }
        }

        public Brush Foreground { set; get; }
        public Brush BorderBrush { set; get; }
        public Thickness BorderThickness { set; get; }
        public Brush ColorFocus { set; get; }
        public Brush ColorNormal { set; get; }
        public int GridRow { get; set; }
        public int GridColumn { get; set; }
        public FontFamily FontFamily { get; set; }

        Widget _parent;
        public Widget Parent
        {
            set
            {
                _parent = value;
                onSetParent(_parent);
            }
            get
            {
                return _parent;
            }
        }

        GridCollection _pos;
        public GridCollection Pos
        {
            set
            {
                _pos = value;
            }
            get
            {
                return _pos;
            }
        }

        public int GridColumnSpan { get; set; }
        public int GridRowSpan { get; set; }
        public bool IsFocus { get { return isFocus(); } }

        bool _isFocus;

        public Widget() : base()
        {
            Parent = this;
            Background = Brushes.White;
            Foreground = Brushes.Black;
            BorderBrush = Brushes.Black;
            ColorNormal = new Brush(0xFF111111);
            ColorFocus = new Brush(0xFF141414);
            BorderThickness = new Thickness(1);
            Margin = new Thickness();
            Padding = new Thickness();
            FontFamily = new FontFamily();
            Cursor = new Cursor(CursorState.None);
        }

        public virtual void Draw()
        {
            if (this.Parent == null)
            {
                return;
            }

            // Position & margin
            if (Pos == null)
            {
                X = this.Parent.X + this.Margin.Left;
                Y = this.Parent.Y + this.Margin.Top;
                Width = this.Parent.Width - (this.Margin.Right * 2);
                Height = this.Parent.Height - (this.Margin.Bottom * 2);
            }
            else
            {
                X = this.Pos.Position.X + this.Margin.Left;
                Y = this.Pos.Position.Y + this.Margin.Top;
                Width = this.Pos.Position.Width - (this.Margin.Right * 2);
                Height = this.Pos.Position.Height - (this.Margin.Bottom * 2);
            }
        }

        public virtual void Update()
        {
            if (Control.MouseButtons == MouseButtons.Left)
            {
                if (!WindowManager.HasWindowMoving && Control.MousePosition.X > X && Control.MousePosition.X < (X + Width) && Control.MousePosition.Y > Y && Control.MousePosition.Y < (Y + Height))
                {
                    WindowManager.FocusControl = this;
                    _isFocus = true;
                }
            }

            if (!WindowManager.HasWindowMoving && Control.MousePosition.X > X && Control.MousePosition.X < (X + Width) && Control.MousePosition.Y > Y && Control.MousePosition.Y < (Y + Height))
            {
                CursorManager.FocusControl = this;
                MouseEnter = true;
            }
            else
            {
                MouseEnter = false;
                if (Control.MouseButtons == MouseButtons.Left)
                {
                    _isFocus = false;
                }
            }
        }

        public void onSetParent(Widget parent)
        {
            X = parent.X;
            Y = parent.Y;
            Width = parent.Width;
            Height = parent.Height;
        }

        public void DrawBorder()
        {
            Framebuffer.Graphics.DrawRectangle(X - (int)(BorderThickness.Left - 1), Y - (int)(BorderThickness.Top - 1), Width + (int)(BorderThickness.Right), Height + (int)(BorderThickness.Bottom), BorderBrush.Value);
        }

        public void onMouseFocus()
        {
            // Background.Value = ColorFocus.Value;
        }

        public void onMouseLostFocus()
        {

        }

        bool isFocus()
        {
            if (WindowManager.FocusWindow != null && WindowManager.FocusControl != null)
            {
                if (WindowManager.FocusControl == this && _isFocus)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
