#if HasGUI
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using MOOS.Graph;
using MOOS.GUI.Widgets;

namespace MOOS.GUI
{
	internal unsafe class Paint : Window
	{
		private Image img;
		private Graphics g;

		public Paint(int X, int Y) : base(X, Y, 490, 500)
		{
#if Chinese
			Title = "画图";
#else
			Title = "Paint";
#endif
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

#if Chinese
			AddButton(430, 10, 0xFF333333, 50, "清除");
#else
			AddButton(430, 10, 0xFF333333, 50, "Clear");
#endif

			img = new Image(Width, Height);
			fixed (int* p = img.RawData)
			{
				g = new Graphics(Width, Height, p);
			}

			g.Clear(Color.FromArgb(0xFF222222));

			CurrentColor = 0xFFF0F0F0;
			LastX = -1;
			LastY = -1;
		}

		public override void OnDraw()
		{
			base.OnDraw();

			Framebuffer.Graphics.DrawImage(img, X, Y, false);

			for (int i = 0; i < Btns.Count; i++)
			{
				Framebuffer.Graphics.FillRectangle(Color.FromArgb(Btns[i].UIntParam), X + Btns[i].X, Y + Btns[i].Y, Btns[i].Width, Btns[i].Height);
#if Chinese
				if (Btns[i].Name == "清除")
#else
				if (Btns[i].Name == "Clear")
#endif
				{
					WindowManager.font.DrawString(X + Btns[i].X + (Btns[i].Width / 2) - (WindowManager.font.MeasureString(Btns[i].Name) / 2), Y + Btns[i].Y + 2, Btns[i].Name);
				}
			}
		}

		private int LastX,
			LastY;
		private uint CurrentColor;

		public override void OnInput()
		{
			base.OnInput();

			if (IsFocus) return;

			if (Control.MouseButtons.HasFlag(MouseButtons.Left))
			{
				if (Control.MousePosition.X >= X && Control.MousePosition.X <= X + Width && Control.MousePosition.Y >= Y && Control.MousePosition.Y <= Y + Height)
				{
					WindowManager.MouseHandled = true;
					if (Control.MousePosition.X - X != LastX || Control.MousePosition.Y - Y != LastY)
					{
						g.DrawLine(Color.FromArgb(CurrentColor), LastX, LastY, Control.MousePosition.X - X, Control.MousePosition.Y - Y);
					}

					for (int i = 0; i < Btns.Count; i++)
					{
						if (Control.MousePosition.X > X + Btns[i].X && Control.MousePosition.X < X + Btns[i].X + Btns[i].Width && Control.MousePosition.Y > Y + Btns[i].Y && Control.MousePosition.Y < Y + Btns[i].Y + Btns[i].Height)
						{
#if Chinese
							if (Btns[i].Name == "清除")
#else
							if (Btns[i].Name == "Clear")
#endif
							{
								g.Clear(Color.FromArgb(0xFF222222));
							} else
							{
								CurrentColor = Btns[i].UIntParam;
							}
						}
					}
				}
			} else
			{
				WindowManager.MouseHandled = false;
			}

			LastX = Control.MousePosition.X - X;
			LastY = Control.MousePosition.Y - Y;
		}

		private List<Button> Btns;

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
#endif