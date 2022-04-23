namespace Kernel.GUI
{
    internal class MessageBox : Window
    {
        string _message;

        public MessageBox(int X, int Y) : base(X, Y, 200, Window.font.FontSize)
        {
            this._message = null;
            this.Title = "MessageBox";
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
