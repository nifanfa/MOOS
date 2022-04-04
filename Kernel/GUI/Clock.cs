/*
 * Copyright(c) 2022 nifanfa, This code is part of the Solution1 licensed under the MIT licence.
 */
using System;

namespace Kernel.GUI
{
    internal class Clock : Window
    {
        public Clock(int X,int Y) : base(X, Y, 200, 200)
        {
            BackgroundColor = 0x0;
            Title = "Clock";
        }

        public override void Update()
        {
            base.Update();

            int second = RTC.Second * 6;
            DrawHand(X + (Width / 2), Y + (Height / 2), second, Width > Height ? (Height / 3) : (Width / 3), 0xFFFF0000);

            int minute = RTC.Minute * 6;
            DrawHand(X + (Width / 2), Y + (Height / 2), minute, Width > Height ? (Height / 4) : (Width / 4), 0xFFFFFFFF);

            int hour = (RTC.Hour >= 12 ? RTC.Hour - 12 : RTC.Hour) * 30;
            DrawHand(X + (Width / 2), Y + (Height / 2), hour, Width > Height ? (Height / 6) : (Width / 6), 0xFFFFFFFF);

            string devider = ":";
            string shour = RTC.Hour.ToString();
            string sminute = RTC.Minute.ToString();
            string ssecond = RTC.Second.ToString();
            string result = shour + devider + sminute + devider + ssecond;
            font.DrawString(X + (Width / 2) - (font.MeasureString(result) / 2), Y + font.FontSize, result);
            devider.Dispose();
            shour.Dispose();
            sminute.Dispose();
            ssecond.Dispose();
            result.Dispose();
        }

        void DrawHand(int xStart, int yStart, int angle, int radius, uint color)
        {
            angle /= 6;
            int[] sine = new int[16] { 0, 27, 54, 79, 104, 128, 150, 171, 190, 201, 221, 233, 243, 250, 254, 255 };
            int xEnd, yEnd, quadrant, x_flip, y_flip;

            quadrant = angle / 15;

            switch (quadrant)
            {
                case 0: x_flip = 1; y_flip = -1; break;
                case 1: angle = Math.Abs(angle - 30); x_flip = y_flip = 1; break;
                case 2: angle = angle - 30; x_flip = -1; y_flip = 1; break;
                case 3: angle = Math.Abs(angle - 60); x_flip = y_flip = -1; break;
                default: x_flip = y_flip = 1; break;
            }

            xEnd = xStart;
            yEnd = yStart;

            if (angle > sine.Length) return;

            xEnd += (x_flip * ((sine[angle] * radius) >> 8));
            yEnd += (y_flip * ((sine[15 - angle] * radius) >> 8));

            Framebuffer.DrawLine(xStart, yStart, xEnd, yEnd, color);
            sine.Dispose();
        }
    }
}
