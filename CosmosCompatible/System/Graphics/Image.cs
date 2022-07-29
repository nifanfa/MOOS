namespace Cosmos.System.Graphics
{
    public class Image
    {
        public int Width;
        public int Height;
        public int[] rawData;

        public Image ResizeImage(int NewWidth, int NewHeight)
        {
            if (NewWidth == 0 || NewHeight == 0)
            {
                return new Image();
            }

            int w1 = Width, h1 = Height;
            int[] temp = new int[NewWidth * NewHeight];

            int x_ratio = ((w1 << 16) / NewWidth) + 1, y_ratio = ((h1 << 16) / NewHeight) + 1;
            int x2, y2;

            for (int i = 0; i < NewHeight; i++)
            {
                for (int j = 0; j < NewWidth; j++)
                {
                    x2 = ((j * x_ratio) >> 16);
                    y2 = ((i * y_ratio) >> 16);
                    temp[(uint)((i * NewWidth) + j)] = rawData[(uint)((y2 * w1) + x2)];
                }
            }

            Image image = new Image()
            {
                Width = NewWidth,
                Height = NewHeight,
                rawData = temp
            };

            return image;
        }
    }
}
