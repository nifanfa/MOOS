#if HasGUI
using MOOS.GUI;

namespace MOOS.GUI
{
	internal class MessageBox : Window
	{
		private string _message;

		public MessageBox(int X, int Y) : base(X, Y, 200, WindowManager.font.FontSize * 2)
		{
			_message = null;
#if Chinese
			this.Title = "信息框";
#else
			Title = "MessageBox";
#endif
		}

		public override void OnDraw()
		{
			if (_message != null)
			{
				Width = WindowManager.font.MeasureString(_message);
			}
			base.OnDraw();
			if (_message != null)
			{
				WindowManager.font.DrawString(X, Y, _message);
			}
		}

		public void SetText(string text)
		{
			if (_message != null)
			{
				_message.Dispose();
			}

			_message = text;
		}
	}
}
namespace System.Windows.Forms
{
	public static class MessageBox
	{
		public static void Show(string text)
		{
			Desktop.msgbox.X = WindowManager.Windows[0].X + 75;
			Desktop.msgbox.Y = WindowManager.Windows[0].Y + 75;
			Desktop.msgbox.SetText(text);
			WindowManager.MoveToEnd(Desktop.msgbox);
			Desktop.msgbox.Visible = true;
		}
	}
}
#endif