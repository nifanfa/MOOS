using System.Diagnostics;

namespace System.Windows
{
    public class MessageBox : Window
    {
        public static MessageBox Instance { private set; get; }

        string _message;

        public MessageBox()
        {
            if (Instance != null)
            {
                Instance = this;
            }
            this._message = null;
#if Chinese
            this.Title = "信息框";
#else
            this.Title = "MessageBox";
#endif
            X = 0;
            Y = 0;
            Width = 400;
            Height = 75;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public override void OnDraw()
        {
            if (this._message != null)
            {
                this.Width = WindowManager.font.MeasureString(_message);
            }

            if (this.Width < 400)
            {
                this.Width = 400;
            }

            base.OnDraw();

            if (this._message != null)
            {
                WindowManager.font.DrawString(X + (Width / 2) - ((WindowManager.font.MeasureString(_message)) / 2), (Y + (Height / 2)) -(WindowManager.font.FontSize / 2), _message, Foreground.Value);
            }
        }

        public void SetText(string text, string title  = "MessageBox") 
        {
            this.Title = title;
            if (this._message != null) this._message.Dispose();
            this._message = text;
        }

        public static void Show(string text, string title)
        {
            Instance.SetText(text, title);
            Instance.ShowDialog();
        }

        public static void Initialize()
        {
           Instance = new MessageBox();
        }
    }
}
