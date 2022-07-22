#if Kernel && HasGUI
using MOOS.GUI;

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