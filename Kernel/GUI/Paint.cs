using Kernel.Graph;
using Kernel.GUI.Widgets;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static Kernel.GUI.Calculator;

namespace Kernel.GUI
{
    internal unsafe class Paint : Window
    {
        Image img;
        Graphics g;

        public Paint(int X, int Y) : base(X, Y, 490, 500)
        {
            Title = "Paint";
            Btns = new List<Button>();
            AddButton(10, 10, 0xFFC0392B);
            AddButton(40, 10, 0xFFE74C3C);
            AddButton(70, 10, 0xFFAF7AC5);
            AddButton(100, 10, 0xFF8E44AD);
            AddButton(130, 10, 0xFF2980B9);
            AddButton(160, 10, 0xFF5DADE2);
            AddButton(190, 10, 0xFF1ABC9C);
            AddButton(220, 10, 0xFF45B39D);
            AddButton(250, 10, 0xFF52BE80);
            AddButton(280, 10, 0xFF27AE60);
            AddButton(310, 10, 0xFFF1C40F);
            AddButton(340, 10, 0xFFE67E22);
            AddButton(370, 10, 0xFFECF0F1);
            AddButton(400, 10, 0xFFD4AC0D);

            AddButton(430, 10, 0xFF333333, 50, "Clear");

            img = new Image(this.Width, this.Height);
            fixed (uint* p = img.RawData)
                g = new Graphics(this.Width, this.Height, p);
            g.Clear(0xFF222222);

            CurrentColor = 0xFFF0F0F0;
            LastX = -1;
            LastY = -1;
        }

        public override void OnDraw()
        {
            base.OnDraw();

            Framebuffer.Graphics.DrawImage(X, Y, img, false);

            for (int i = 0; i < Btns.Count; i++)
            {
                Framebuffer.Graphics.FillRectangle(X + Btns[i].X, Y + Btns[i].Y, Btns[i].Width, Btns[i].Height, Btns[i].UIntParam);
                if (Btns[i].Name == "Clear")
                {
                    font.DrawString(X + Btns[i].X + 4, Y + Btns[i].Y + 2, Btns[i].Name);
                }
            }
        }

        int LastX,
            LastY;

        uint CurrentColor;

        public override void OnInput()
        {
            base.OnInput();

            if (Control.MouseButtons.HasFlag(MouseButtons.Left))
            {
                if (Control.MousePosition.X >= this.X && Control.MousePosition.X <= this.X + this.Width && Control.MousePosition.Y >= this.Y && Control.MousePosition.Y <= this.Y + this.Height)
                {
                    MouseHandled = true;
                    if (Control.MousePosition.X - this.X != LastX || Control.MousePosition.Y - this.Y != LastY)
                    {
                        g.DrawLine(LastX, LastY, Control.MousePosition.X - this.X, Control.MousePosition.Y - this.Y, CurrentColor);
                    }

                    for (int i = 0; i < Btns.Count; i++)
                    {
                        if (Control.MousePosition.X > this.X + Btns[i].X && Control.MousePosition.X < this.X + Btns[i].X + Btns[i].Width && Control.MousePosition.Y > this.Y + Btns[i].Y && Control.MousePosition.Y < this.Y + Btns[i].Y + Btns[i].Height)
                        {
                            if (Btns[i].Name == "Clear")
                            {
                                g.Clear(0xFF222222);
                            }
                            else
                            {
                                CurrentColor = Btns[i].UIntParam;
                            }
                        }
                    }
                }
            }
            else
            {
                MouseHandled = false;
            }

            LastX = Control.MousePosition.X - this.X;
            LastY = Control.MousePosition.Y - this.Y;
        }

        List<Button> Btns;

        private void AddButton(int X, int Y, uint Color, int aWidth = 20, string aName = "")
        {
            Btns.Add(new Button()
            {
                X = X,
                Y = Y,
                Width = aWidth,
                Height = 20,
                UIntParam = Color,
                Name = aName
            });
        }
    }
}
