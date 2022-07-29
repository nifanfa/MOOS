using Cosmos.System.Graphics;
using System;
using System.Drawing;

namespace CosmosKernel1
{
    class Clock : App
    {
        public Clock(uint width, uint height, uint x = 0, uint y = 0) : base(width, height, x, y)
        {
            name = "Clock";
        }

        public override void _Update()
        {
        }

        void drawHand(Canvas vMWareSVGAII, uint color, int xStart, int yStart, int angle, int radius)
        {
        }
    }
}
