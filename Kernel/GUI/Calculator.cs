#if HasGUI
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using MOOS.Graph;
using MOOS.GUI.Widgets;

namespace MOOS.GUI
{
	internal class Calculator : Window
	{
		private Image image;
		private Graphics g;

		public unsafe Calculator(int X, int Y) : base(X, Y, 270, 140)
		{
#if Chinese
			Title = "计算器";
#else
			Title = "Calculator";
#endif
			Btns = new List<Button>();

			image = new Image(Width, Height);
			fixed (int* p = image.RawData)
			{
				g = new Graphics(image.Width, image.Height, p);
			}

			PressedButton = new();

			g.FillRectangle(Color.FromArgb(0xFF222222), 0, 0, Width, Height);
			g.FillRectangle(Color.FromArgb(0xFF333333), 0, 0, Width, 20);

			//7
			AddButton(0, 30, "7");
			//8
			AddButton(70, 30, "8");
			//9
			AddButton(140, 30, "9");


			//4
			AddButton(0, 60, "4");
			//5
			AddButton(70, 60, "5");
			//6
			AddButton(140, 60, "6");


			//1
			AddButton(0, 90, "1");
			//2
			AddButton(70, 90, "2");
			//3
			AddButton(140, 90, "3");


			//0
			AddButton(70, 120, "0");


			//C
			AddButton(210, 30, "C");
			//+
			AddButton(210, 60, "+");
			//-
			AddButton(210, 90, "-");
			//=
			AddButton(210, 120, "=");

		}

		public override void OnDraw()
		{
			base.OnDraw();

			Framebuffer.Graphics.DrawImage(image, X, Y);

			string v = ValueToDisplay.ToString();
			WindowManager.font.DrawString(X, Y + 2, v);

			if (Pressed)
			{
				Framebuffer.Graphics.FillRectangle(Color.FromArgb(0xFF222222), X + PressedButton.X, Y + PressedButton.Y, 60, 20);
				int i = WindowManager.font.MeasureString(PressedButton.Name);
				WindowManager.font.DrawString(X + PressedButton.X + (60 / 2) - (i / 2), Y + PressedButton.Y + 2, PressedButton.Name);
			}

			v.Dispose();
		}

		private bool Pressed = false;
		private Button PressedButton;

		public override void OnInput()
		{
			if (!IsFocus) return;
			base.OnInput();

			if (Control.MouseButtons.HasFlag(MouseButtons.Left))
			{
				if (!Pressed)
				{
					for (int i = 0; i < Btns.Count; i++)
					{
						if (Control.MousePosition.X > X + Btns[i].X && Control.MousePosition.X < X + Btns[i].X + Btns[i].Width && Control.MousePosition.Y > Y + Btns[i].Y && Control.MousePosition.Y < Y + Btns[i].Y + Btns[i].Height)
						{
							ProcessButton(Btns[i]);
							PressedButton = Btns[i];
							Pressed = true;
						}
					}
				}
			} else
			{
				Pressed = false;
			}
		}

		private enum CalculatorOperation
		{
			None,
			Plus,
			Minus
		}

		private long Num1 = 0;
		private long Num2 = 0;
		private long ValueToDisplay = 0;
		private CalculatorOperation opreation = CalculatorOperation.None;

		private unsafe void ProcessButton(Button btn)
		{
			if (btn.Name[0] >= 0x30 && btn.Name[0] <= 0x39)
			{
				if (Num2 == 0)
				{
					Num2 = btn.Name[0] - 0x30L;
				} else
				{
					Num2 *= 10;
					Num2 += btn.Name[0] - 0x30L;
				}

				ValueToDisplay = Num2;
			} else if (btn.Name == "+")
			{
				if (Num1 == 0)
				{
					Num1 = Num2;
				}

				opreation = CalculatorOperation.Plus;

				Num2 = 0;
			} else if (btn.Name == "-")
			{
				if (Num1 == 0)
				{
					Num1 = Num2;
				}

				opreation = CalculatorOperation.Minus;

				Num2 = 0;
			} else if (btn.Name == "C")
			{
				Num1 = 0;
				Num2 = 0;
				ValueToDisplay = 0;
			} else if (btn.Name == "=")
			{
				if (opreation == CalculatorOperation.Plus)
				{
					Num1 += Num2;
				} else if (opreation == CalculatorOperation.Minus)
				{
					if (Num1 >= Num2)
					{
						Num1 -= Num2;
					} else
					{
						Num1 = 0;
						Num2 = 0;
					}
				} else if (opreation == CalculatorOperation.None)
				{
					Num2 = 0;
				}

				ValueToDisplay = Num1;
			}
		}

		private List<Button> Btns;


		private void AddButton(int X, int Y, string s)
		{
			g.FillRectangle(Color.FromArgb(0xFF333333), X, Y, 60, 20);
			int i = WindowManager.font.MeasureString(s);
			WindowManager.font.DrawString(X + (60 / 2) - (i / 2), Y + 2, s, g);

			Btns.Add(new Button()
			{
				X = X,
				Y = Y,
				Width = 60,
				Height = 20,
				Name = s
			});
		}
	}
}
#endif