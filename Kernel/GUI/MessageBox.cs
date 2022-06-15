#if HasGUI
namespace MOOS.GUI
{
    internal class MessageBox : Window
    {
        string _message;

        public MessageBox(int X, int Y) : base(X, Y, 200, Window.font.FontSize*2)
        {
            this._message = null;
#if Chinese
            this.Title = "信息框";
#else
            this.Title = "MessageBox";
#endif
        }

        public override void OnDraw()
        {
            if (this._message != null)
            {
                this.Width = font.MeasureString(_message);
            }
            base.OnDraw();
            if(this._message!=null)
            {
                font.DrawString(X, Y, _message);
            }
        }

        public void SetText(string text) 
        {
            if (this._message != null) this._message.Dispose();
            this._message = text;
        }
    }
}
#endif