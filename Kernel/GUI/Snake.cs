#if HasGUI
using MOOS.Driver;
using System;

namespace MOOS.GUI
{
    class Snake : Window
    {
        const int aWidth = 10;
        const int aHeight = 10;

        enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        class SnakeNode
        {
            public int X;
            public int Y;
        }

        Direction Dir = Direction.Right;
        SnakeNode[] SnakeNodes;

        int Max = aWidth* aHeight;

        int FoodX = -1;
        int FoodY = -1;

        Random random;

        public Snake(int X, int Y) : base(X, Y, aWidth*15, aHeight*15)
        {
#if Chinese
            Title = "贪吃蛇";
#else
            Title = "Snake";
#endif

            //MessageBox = new MessageBox() { Visible = false, Info = "Game Over!", Width = 150, Height = 16, X = 300, Y = 350 };

            //System.Add(MessageBox);

            SnakeNodes = new SnakeNode[Max];

            random = new Random();

            Add(new SnakeNode() { X = aWidth / 2, Y = aHeight / 2 });

            NewFood();
        }

        int Count = 0;
        void Add(SnakeNode snakeNode)
        {
            SnakeNodes[Count] = snakeNode;
            Count++;
        }

        public override void OnDraw()
        {
            base.OnDraw();

            IsEatBody();
            if (Count == Max)
            {
                NewGame();
            }

            Refresh();
            /*
            if (!System.MessageBox.Actived)
            {
                Control();
            }
            */
            Control();

            Display();
        }

        public override void OnInput()
        {
            base.OnInput();

            switch (Keyboard.KeyInfo.Key)
            {
                case ConsoleKey.W:
                    Dir = Direction.Up;
                    break;
                case ConsoleKey.A:
                    Dir = Direction.Left;
                    break;
                case ConsoleKey.S:
                    Dir = Direction.Down;
                    break;
                case ConsoleKey.D:
                    Dir = Direction.Right;
                    break;
            }
        }

        ulong W = 0;

        private void Control()
        {
            if (Timer.Ticks < W + 150)
            {
                return;
            }
            W = Timer.Ticks;

            switch (Dir)
            {
                case Direction.Up:
                    if (SnakeNodes[Count - 1].Y - 1 >= 0)
                    {
                        SetChildren();
                        SnakeNodes[Count - 1].Y--;
                    }

                    if (SnakeNodes[Count - 1].X == FoodX && SnakeNodes[Count - 1].Y == FoodY)
                    {
                        if (Count < Max)
                            Add(new SnakeNode() { X = SnakeNodes[Count - 1].X, Y = SnakeNodes[Count - 1].Y - 1 });
                        NewFood();
                    }

                    break;
                case Direction.Down:
                    if (SnakeNodes[Count - 1].Y + 1 < aHeight)
                    {
                        SetChildren();
                        SnakeNodes[Count - 1].Y++;
                    }

                    if (SnakeNodes[Count - 1].X == FoodX && SnakeNodes[Count - 1].Y == FoodY)
                    {
                        if (Count < Max)
                            Add(new SnakeNode() { X = SnakeNodes[Count - 1].X, Y = SnakeNodes[Count - 1].Y + 1 });
                        NewFood();
                    }

                    break;
                case Direction.Left:
                    if (SnakeNodes[Count - 1].X - 1 >= 0)
                    {
                        SetChildren();
                        SnakeNodes[Count - 1].X--;
                    }

                    if (SnakeNodes[Count - 1].X == FoodX && SnakeNodes[Count - 1].Y == FoodY)
                    {
                        if (Count < Max)
                            Add(new SnakeNode() { X = SnakeNodes[Count - 1].X - 1, Y = SnakeNodes[Count - 1].Y });
                        NewFood();
                    }

                    break;
                case Direction.Right:
                    if (SnakeNodes[Count - 1].X + 1 < aWidth)
                    {
                        SetChildren();
                        SnakeNodes[Count - 1].X++;
                    }

                    if (SnakeNodes[Count - 1].X == FoodX && SnakeNodes[Count - 1].Y == FoodY)
                    {
                        if (Count < Max)
                            Add(new SnakeNode() { X = SnakeNodes[Count - 1].X + 1, Y = SnakeNodes[Count - 1].Y });
                        NewFood();
                    }

                    break;
            }
        }

        private void SetChildren()
        {
            for (int i = 0; i < Count; i++)
            {
                if (i + 1 <= Count - 1)
                {
                    SnakeNodes[i].X = SnakeNodes[i + 1].X;
                    SnakeNodes[i].Y = SnakeNodes[i + 1].Y;
                }
            }
        }

        void NewFood()
        {
            int newX = random.Next(2, aWidth - 3);
            int newY = random.Next(2, aHeight - 3);
            for (int i = 0; i < Count; i++)
            {
                if (SnakeNodes[i].X == newX && SnakeNodes[i].Y == newY)
                {
                    i = 0;
                    newX = random.Next(2, aWidth - 3);
                    newY = random.Next(2, aHeight - 3);
                }
            }

            FoodX = newX;
            FoodY = newY;
        }

        void IsEatBody()
        {
            for (int i = 0; i < Count; i++)
            {
                for (int k = 0; k < Count; k++)
                {
                    if (i != k)
                    {
                        if (SnakeNodes[i].X == SnakeNodes[k].X && SnakeNodes[i].Y == SnakeNodes[k].Y)
                        {
                            System.Windows.Forms.MessageBox.Show($"Game over! Your score: {Count}");
                            NewGame();
                        }
                    }
                }
            }
        }

        private void NewGame()
        {
            Count = 1;
        }

        void Refresh()
        {
            Framebuffer.Graphics.FillRectangle(X, Y, Width, Height, 0xFF9FAE87);
            for (int h = 0; h < aHeight; h++)
            {
                for (int w = 0; w < aWidth; w++)
                {
                    Framebuffer.Graphics.FillRectangle(this.X + (w * SizePerBlock) + 3, this.Y + (h * SizePerBlock) + 3, SizePerBlock - 6, SizePerBlock - 6, 0xFF98A682);
                }
            }
        }

        int SizePerBlock = 15;

        void Display()
        {
            for (int i = 0; i < Count; i++)
            {
                if (i == Count - 1)
                {
                    Framebuffer.Graphics.DrawRectangle(this.X + (SnakeNodes[i].X * SizePerBlock), this.Y + (SnakeNodes[i].Y * SizePerBlock), SizePerBlock, SizePerBlock, 0xFF262627, 2);

                    switch (Dir)
                    {
                        case Direction.Up:
                            Framebuffer.Graphics.FillRectangle(this.X + (SnakeNodes[i].X * SizePerBlock) + SizePerBlock / 5, this.Y + (SnakeNodes[i].Y * SizePerBlock) + SizePerBlock / 5, SizePerBlock / 5, SizePerBlock / 5, 0xFF262627);
                            Framebuffer.Graphics.FillRectangle(this.X + (SnakeNodes[i].X * SizePerBlock) + SizePerBlock - ((SizePerBlock / 5) * 2), this.Y + (SnakeNodes[i].Y * SizePerBlock) + SizePerBlock / 5, SizePerBlock / 5, SizePerBlock / 5, 0xFF262627);
                            break;
                        case Direction.Down:
                            Framebuffer.Graphics.FillRectangle(this.X + (SnakeNodes[i].X * SizePerBlock) + SizePerBlock / 5, this.Y + (SnakeNodes[i].Y * SizePerBlock) + SizePerBlock - ((SizePerBlock / 5) * 2), SizePerBlock / 5, SizePerBlock / 5, 0xFF262627);
                            Framebuffer.Graphics.FillRectangle(this.X + (SnakeNodes[i].X * SizePerBlock) + SizePerBlock - ((SizePerBlock / 5) * 2), this.Y + (SnakeNodes[i].Y * SizePerBlock) + SizePerBlock - ((SizePerBlock / 5) * 2), SizePerBlock / 5, SizePerBlock / 5, 0xFF262627);
                            break;
                        case Direction.Left:
                            Framebuffer.Graphics.FillRectangle(this.X + (SnakeNodes[i].X * SizePerBlock) + SizePerBlock / 5, this.Y + (SnakeNodes[i].Y * SizePerBlock) + SizePerBlock / 5, SizePerBlock / 5, SizePerBlock / 5, 0xFF262627);
                            Framebuffer.Graphics.FillRectangle(this.X + (SnakeNodes[i].X * SizePerBlock) + SizePerBlock / 5, this.Y + (SnakeNodes[i].Y * SizePerBlock) + SizePerBlock - ((SizePerBlock / 5) * 2), SizePerBlock / 5, SizePerBlock / 5, 0xFF262627);
                            break;
                        case Direction.Right:
                            Framebuffer.Graphics.FillRectangle(this.X + (SnakeNodes[i].X * SizePerBlock) + SizePerBlock - ((SizePerBlock / 5) * 2), this.Y + (SnakeNodes[i].Y * SizePerBlock) + SizePerBlock / 5, SizePerBlock / 5, SizePerBlock / 5, 0xFF262627);
                            Framebuffer.Graphics.FillRectangle(this.X + (SnakeNodes[i].X * SizePerBlock) + SizePerBlock - ((SizePerBlock / 5) * 2), this.Y + (SnakeNodes[i].Y * SizePerBlock) + SizePerBlock - (SizePerBlock / 5 * 2), SizePerBlock / 5, SizePerBlock / 5, 0xFF262627);
                            break;
                    }
                }
                else
                {
                    Framebuffer.Graphics.DrawRectangle(this.X + (SnakeNodes[i].X * SizePerBlock), this.Y + (SnakeNodes[i].Y * SizePerBlock), SizePerBlock, SizePerBlock, 2);
                    Framebuffer.Graphics.FillRectangle(this.X + (SnakeNodes[i].X * SizePerBlock) + 3, this.Y + (SnakeNodes[i].Y * SizePerBlock) + 3, SizePerBlock - 6, SizePerBlock - 6, 0xFF262627);
                }
            }

            //Food
            Framebuffer.Graphics.DrawRectangle(this.X + (FoodX * SizePerBlock), this.Y + (FoodY * SizePerBlock), SizePerBlock, SizePerBlock, 2);
            Framebuffer.Graphics.FillRectangle(this.X + (FoodX * SizePerBlock) + 3, this.Y + (FoodY * SizePerBlock) + 3, SizePerBlock - 6, SizePerBlock - 6, 0xFF262627);
        }
    }
}
#endif