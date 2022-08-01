#if HasGUI
using System;
using System.Drawing;
using MOOS.Driver;

namespace MOOS.GUI
{
	internal class Snake : Window
	{
		private const int aWidth = 10;
		private const int aHeight = 10;

		private enum Direction
		{
			Up,
			Down,
			Left,
			Right
		}

		private class SnakeNode
		{
			public int X;
			public int Y;
		}

		private Direction Dir = Direction.Right;
		private SnakeNode[] SnakeNodes;
		private int Max = aWidth * aHeight;
		private int FoodX = -1;
		private int FoodY = -1;
		private Random random;

		public Snake(int X, int Y) : base(X, Y, aWidth * 15, aHeight * 15)
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

		private int Count = 0;

		private void Add(SnakeNode snakeNode)
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

		private ulong W = 0;

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
						{
							Add(new SnakeNode() { X = SnakeNodes[Count - 1].X, Y = SnakeNodes[Count - 1].Y - 1 });
						}

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
						{
							Add(new SnakeNode() { X = SnakeNodes[Count - 1].X, Y = SnakeNodes[Count - 1].Y + 1 });
						}

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
						{
							Add(new SnakeNode() { X = SnakeNodes[Count - 1].X - 1, Y = SnakeNodes[Count - 1].Y });
						}

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
						{
							Add(new SnakeNode() { X = SnakeNodes[Count - 1].X + 1, Y = SnakeNodes[Count - 1].Y });
						}

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

		private void NewFood()
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

		private void IsEatBody()
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

		private void Refresh()
		{
			Framebuffer.Graphics.FillRectangle(Color.FromArgb(0xFF9FAE87), X, Y, Width, Height);
			for (int h = 0; h < aHeight; h++)
			{
				for (int w = 0; w < aWidth; w++)
				{
					Framebuffer.Graphics.FillRectangle(Color.FromArgb(0xFF98A682), X + (w * SizePerBlock) + 3, Y + (h * SizePerBlock) + 3, SizePerBlock - 6, SizePerBlock - 6);
				}
			}
		}

		private int SizePerBlock = 15;

		private void Display()
		{
			for (int i = 0; i < Count; i++)
			{
				if (i == Count - 1)
				{
					Framebuffer.Graphics.DrawRectangle(Color.FromArgb(0xFF262627), X + (SnakeNodes[i].X * SizePerBlock), Y + (SnakeNodes[i].Y * SizePerBlock), SizePerBlock, SizePerBlock);

					switch (Dir)
					{
						case Direction.Up:
							Framebuffer.Graphics.FillRectangle(Color.FromArgb(0xFF262627), X + (SnakeNodes[i].X * SizePerBlock) + (SizePerBlock / 5), Y + (SnakeNodes[i].Y * SizePerBlock) + (SizePerBlock / 5), SizePerBlock / 5, SizePerBlock / 5);
							Framebuffer.Graphics.FillRectangle(Color.FromArgb(0xFF262627), X + (SnakeNodes[i].X * SizePerBlock) + SizePerBlock - (SizePerBlock / 5 * 2), Y + (SnakeNodes[i].Y * SizePerBlock) + (SizePerBlock / 5), SizePerBlock / 5, SizePerBlock / 5);
							break;
						case Direction.Down:
							Framebuffer.Graphics.FillRectangle(Color.FromArgb(0xFF262627), X + (SnakeNodes[i].X * SizePerBlock) + (SizePerBlock / 5), Y + (SnakeNodes[i].Y * SizePerBlock) + SizePerBlock - (SizePerBlock / 5 * 2), SizePerBlock / 5, SizePerBlock / 5);
							Framebuffer.Graphics.FillRectangle(Color.FromArgb(0xFF262627), X + (SnakeNodes[i].X * SizePerBlock) + SizePerBlock - (SizePerBlock / 5 * 2), Y + (SnakeNodes[i].Y * SizePerBlock) + SizePerBlock - (SizePerBlock / 5 * 2), SizePerBlock / 5, SizePerBlock / 5);
							break;
						case Direction.Left:
							Framebuffer.Graphics.FillRectangle(Color.FromArgb(0xFF262627), X + (SnakeNodes[i].X * SizePerBlock) + (SizePerBlock / 5), Y + (SnakeNodes[i].Y * SizePerBlock) + (SizePerBlock / 5), SizePerBlock / 5, SizePerBlock / 5);
							Framebuffer.Graphics.FillRectangle(Color.FromArgb(0xFF262627), X + (SnakeNodes[i].X * SizePerBlock) + (SizePerBlock / 5), Y + (SnakeNodes[i].Y * SizePerBlock) + SizePerBlock - (SizePerBlock / 5 * 2), SizePerBlock / 5, SizePerBlock / 5);
							break;
						case Direction.Right:
							Framebuffer.Graphics.FillRectangle(Color.FromArgb(0xFF262627), X + (SnakeNodes[i].X * SizePerBlock) + SizePerBlock - (SizePerBlock / 5 * 2), Y + (SnakeNodes[i].Y * SizePerBlock) + (SizePerBlock / 5), SizePerBlock / 5, SizePerBlock / 5);
							Framebuffer.Graphics.FillRectangle(Color.FromArgb(0xFF262627), X + (SnakeNodes[i].X * SizePerBlock) + SizePerBlock - (SizePerBlock / 5 * 2), Y + (SnakeNodes[i].Y * SizePerBlock) + SizePerBlock - (SizePerBlock / 5 * 2), SizePerBlock / 5, SizePerBlock / 5);
							break;
					}
				} else
				{
					Framebuffer.Graphics.DrawRectangle(Color.FromArgb(2), X + (SnakeNodes[i].X * SizePerBlock), Y + (SnakeNodes[i].Y * SizePerBlock), SizePerBlock, SizePerBlock);
					Framebuffer.Graphics.FillRectangle(Color.FromArgb(0xFF262627), X + (SnakeNodes[i].X * SizePerBlock) + 3, Y + (SnakeNodes[i].Y * SizePerBlock) + 3, SizePerBlock - 6, SizePerBlock - 6);
				}
			}

			//Food
			Framebuffer.Graphics.DrawRectangle(Color.FromArgb(2), X + (FoodX * SizePerBlock), Y + (FoodY * SizePerBlock), SizePerBlock, SizePerBlock);
			Framebuffer.Graphics.FillRectangle(Color.FromArgb(0xFF262627), X + (FoodX * SizePerBlock) + 3, Y + (FoodY * SizePerBlock) + 3, SizePerBlock - 6, SizePerBlock - 6);
		}
	}
}
#endif