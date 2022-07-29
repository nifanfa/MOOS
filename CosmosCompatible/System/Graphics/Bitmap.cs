using MOOS.FS;

namespace Cosmos.System.Graphics
{
    public class Bitmap : Image
    {
        public Bitmap(string filename)
        {
            MOOS.Misc.Bitmap bmp = new MOOS.Misc.Bitmap(File.ReadAllBytes(filename));
            base.Width = bmp.Width;
            base.Height = bmp.Height;
            base.rawData = bmp.RawData;
        }
    }
}
