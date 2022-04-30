using System.Drawing;

namespace Kernel.GUI
{
    internal class ImageViewer : Window
    {
        Image image;

        public ImageViewer(int X, int Y) : base(X, Y, 250, 200)
        {
            image = null;
            Title = "ImageViewer";
        }

        public override void OnDraw()
        {
            base.OnDraw();

            if(image!=null)
                Framebuffer.Graphics.DrawImage(X + (Width/2) - (image.Width/2), Y + (Height / 2) - (image.Height / 2), image);
        }

        public void SetImage(Image image) 
        {
            if (this.image != null)
            {
                this.image.Dispose();
            }

            if (image.Width > image.Height)
            {
                float ratio = image.Height / (float)image.Width;

                this.image = image.ResizeImage((int)(Width * 0.8f), (int)(Width * ratio * 0.8f));
            }
            else
            {
                float ratio = image.Height / (float)image.Width;

                this.image = image.ResizeImage((int)(Height * 0.8f), (int)(Height * ratio * 0.8f));
            }
        }
    }
}
