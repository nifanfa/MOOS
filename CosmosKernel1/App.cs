using Cosmos.System;
using nifanfa.CosmosDrawString;
using System.Drawing;

namespace CosmosKernel1
{
    public class App
    {
        public readonly uint _width;
        public readonly uint _height;
        public readonly uint width;
        public readonly uint height;

        public uint dockX;
        public uint dockY;
        public uint dockWidth = 40;
        public uint dockHeight = 30;

        public uint _x;
        public uint _y;
        public uint x;
        public uint y;
        public string name;

        bool pressed;
        public bool visible = false;

        public int _i = 0;

        public App(uint width, uint height, uint x = 0, uint y = 0)
        {
            this._width = width;
            this._height = height;
            this._x = x;
            this._y = y;

            this.x = x + 2;
            this.y = y + 22;
            this.width = width - 4;
            this.height = height - 22 - 1;
        }

        public void Update()
        {
            if (_i != 0)
            {
                _i--;
            }

            if (MouseManager.X > dockX && MouseManager.X < dockX + dockWidth && MouseManager.Y > dockY && MouseManager.Y < dockY + dockHeight)
            {
                Kernel.vMWareSVGAII._DrawACSIIString(name, (uint)Color.White.ToArgb(), (uint)(dockX - ((name.Length * 8) / 2) + dockWidth / 2), dockY - 20);
            }

            if (MouseManager.MouseState == MouseState.Left && _i == 0)
            {
                if (MouseManager.X > dockX && MouseManager.X < dockX + dockWidth && MouseManager.Y > dockY && MouseManager.Y < dockY + dockHeight)
                {
                    visible = !visible;
                    _i = 60;
                }
            }

            if (Kernel.Pressed)
            {
                if (MouseManager.X > _x && MouseManager.X < _x + 22 && MouseManager.Y > _y && MouseManager.Y < _y + 22)
                {
                    this.pressed = true;
                }
            }
            else
            {
                this.pressed = false;
            }

            if (!visible)
                goto end;

            if (this.pressed)
            {
                this._x = (uint)MouseManager.X;
                this._y = (uint)MouseManager.Y;

                this.x = (uint)(MouseManager.X + 2);
                this.y = (uint)(MouseManager.Y + 22);
            }

            /*
            Kernel.vMWareSVGAII.DoubleBuffer_DrawFillRectangle(_x, _y, _width, _height, (uint)Color.FromArgb(200, 200, 200).ToArgb());
            Kernel.vMWareSVGAII.DoubleBuffer_DrawFillRectangle(_x + 1, _y + 1, _width - 2, 20, (uint)Color.FromArgb(0, 0, 135).ToArgb());
            */
            Kernel.vMWareSVGAII.DrawFillRectangle(_x, _y, _width, _height, (uint)Color.White.ToArgb());
            Kernel.vMWareSVGAII.DrawRectangle((uint)Kernel.avgCol.ToArgb(), (int)_x, (int)_y, (int)_width, (int)_height);

            Kernel.vMWareSVGAII._DrawACSIIString(name, (uint)Color.Black.ToArgb(), _x + 2, _y + 2);
            //Kernel.vMWareSVGAII.DoubleBuffer_DrawFillRectangle(_x + 22, _y, 1, 22, (uint)Color.FromArgb(200, 200, 200).ToArgb());
            //Kernel.vMWareSVGAII.DoubleBuffer_DrawFillRectangle(x, y, 20, 20, (uint)Color.Gray.ToArgb());
            _Update();

            end:;
        }

        public virtual void _Update()
        {
        }
    }
}
