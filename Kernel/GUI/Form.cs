using System.Collections.Generic;
using System.Windows.Forms;

namespace Kernel.GUI
{
    internal class Form
    {
        public static List<Form> Forms;

        public static void Initialize() 
        {
            Forms = new List<Form>();
        }

        public static void UpdateAll() 
        {
            for(int i = 0; i < Forms.Count; i++) 
            {
                Forms[i].Update();
            }
        }

        int X, Y, Width, Height;

        public Form(int X,int Y,int Width,int Height)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            Forms.Add(this);
            Title = "Form1";
        }

        public int BarHeight = 25;
        public string Title;

        bool Move;
        int OffsetX;
        int OffsetY;

        public virtual void Update() 
        {
            if (Control.MouseButtons == MouseButtons.Left)
            {
                if (!Move && Control.MousePosition.X > X && Control.MousePosition.X < X + Width && Control.MousePosition.Y > Y - BarHeight && Control.MousePosition.Y < Y)
                {
                    Move = true;
                    OffsetX = Control.MousePosition.X - X;
                    OffsetY = Control.MousePosition.Y - Y;
                }
            }
            else
            {
                Move = false;
            }

            if (Move)
            {
                X = Control.MousePosition.X - OffsetX;
                Y = Control.MousePosition.Y - OffsetY;
            }

            Framebuffer.Fill(X, Y - BarHeight, Width, BarHeight, 0xFF101010);
            ASC16.DrawString("Form1", X + (BarHeight/4), Y - (BarHeight / 2) - (16 / 2), 0xFFFFFFFF);
            Framebuffer.Fill(X, Y, Width, Height, 0xFFFFFFFF);
        }
    }
}
