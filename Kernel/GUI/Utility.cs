namespace Kernel.GUI
{
    internal class Utility : Form
    {
        public Utility(int X, int Y) : base(X, Y, 300, 200)
        {
        }

        public override void Update()
        {
            base.Update();
            Framebuffer.Fill(X, Y, Width, Height, 0xFFFF0000);
        }
    }
}
