#if HasGUI
using System.Desktops;
using System.Drawing;
using System.Windows;
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

		private string str;

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
					if (IsUnderMouse() && DesktopManager.Dir.Length > 1)
					{
						int i;
						for (i = DesktopManager.Dir.Length - 1; i >= 0; i--)
						{
							/*
							if (
								//DesktopManager.Dir[i] == '\\' ||
								//DesktopManager.Dir[i] == '/'
								)
							{
								//DesktopManager.Dir[i] = '\0';
								//DesktopManager.Dir[i]

                            }*/
						}
						//DesktopManager.Dir.Length = i + 1;
					}
					Visible = false;
				}
			}
		}

		public override void OnDraw()
		{
			int len = WindowManager.font.MeasureString(str);
			Height = WindowManager.font.FontSize * 2;
			Width = len;

			Framebuffer.Graphics.FillRectangle(Color.FromArgb(0xFF222222), X, Y, Width, Height);
			WindowManager.font.DrawString(X, Y + (WindowManager.font.FontSize / 2), str);
			DrawBorder();
		}
	}
}
#endif