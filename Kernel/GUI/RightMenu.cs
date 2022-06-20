#if HasGUI
using System.Windows.Forms;

namespace MOOS.GUI
{
    internal class RightMenu : Window
    {
        public RightMenu() : base(Control.MousePosition.X, Control.MousePosition.Y, 100, 50)
        {
#if Chinese
            str = "返回上级目录";
#else
            str = "Back to the parent directory";
#endif
            Visible = false;
        }

        string str;

        public override void OnSetVisible(bool value)
        {
            base.OnSetVisible(value);
            if (value)
            {
                X = Control.MousePosition.X - 8;
                Y = Control.MousePosition.Y - 8;
            }
        }

        public override void OnInput()
        {
            if (Visible) 
            {
                if (Control.MouseButtons.HasFlag(MouseButtons.Left))
                {
                    if (IsUnderMouse() && Desktop.Dir.Length > 1)
                    {
                        int i;
                        for (i = Desktop.Dir.Length - 1; i >= 0; i--)
                        {
                            if (
                                Desktop.Dir[i] == '\\' ||
                                Desktop.Dir[i] == '/'
                                )
                            {
                                Desktop.Dir[i] = '\0';
                            }
                        }
                        Desktop.Dir.Length = i + 1;
                    }
                    this.Visible = false;
                }
            }
        }

        public override void OnDraw()
        {
            int len = font.MeasureString(str);
            Height = font.FontSize * 2;
            Width = len;

            Framebuffer.Graphics.FillRectangle(X, Y, Width, Height, 0xFF222222);
            font.DrawString(X, Y + (font.FontSize / 2), str);
            DrawBorder(false);
        }
    }
}
#endif