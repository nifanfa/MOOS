#if HasGUI
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using MOOS.FS;
using MOOS.Misc;

namespace MOOS.GUI
{
	internal class Desktop
	{
		private static Image FileIcon;
		private static Image ImageIcon;
		private static Image GameIcon;
		private static Image AppIcon;
		private static Image AudioIcon;
		private static Image BuiltInAppIcon;
		private static Image FolderIcon;

		public static string Prefix;
		public static string Dir;
		public static ImageViewer imageViewer;
		public static MessageBox msgbox;
		public static NESEmu nesemu;

		public static bool IsAtRoot => Desktop.Dir.Length < 1;

		public static void Initialize()
		{
			IndexClicked = -1;
			FileIcon = new PNG(File.Instance.ReadAllBytes("Images/file.png"));
			ImageIcon = new PNG(File.Instance.ReadAllBytes("Images/Image.png"));
			GameIcon = new PNG(File.Instance.ReadAllBytes("Images/Game.png"));
			AppIcon = new PNG(File.Instance.ReadAllBytes("Images/App.png"));
			AudioIcon = new PNG(File.Instance.ReadAllBytes("Images/Audio.png"));
			BuiltInAppIcon = new PNG(File.Instance.ReadAllBytes("Images/BApp.png"));
			FolderIcon = new PNG(File.Instance.ReadAllBytes("Images/folder.png"));
#if Chinese
			Prefix = " 管理员@Moos: ";
#else
			Prefix = " root@Moos: ";
#endif
			Dir = "";
			imageViewer = new ImageViewer(400, 400);
			msgbox = new MessageBox(100, 300);
			nesemu = new(150, 350);
			imageViewer.Visible = false;
			msgbox.Visible = false;
			nesemu.Visible = false;
			WindowManager.Windows.Add(msgbox);
			WindowManager.Windows.Add(imageViewer);
			WindowManager.Windows.Add(nesemu);

			BuiltInAppNames = new string[]
			{
#if Chinese
				"计算器",
				" 时钟",
				" 画图",
				"贪吃蛇",
				"控制台",
				"监视器"
#else
				"Calculator",
				"Clock",
				"Paint",
				"Snake",
				"Console",
				"Monitor"
#endif
			};

			LastPoint.X = -1;
			LastPoint.Y = -1;
		}

		public static string[] BuiltInAppNames;

		public static void Update()
		{
			const int BarHeight = 35;

			List<FileInfo> names = File.Instance.GetFiles(Dir);
			int Devide = 60;
			int X = Devide;
			int Y = Devide + BarHeight;

			if (IsAtRoot)
			{
				for (int i = 0; i < BuiltInAppNames.Length; i++)
				{
					if (Y + FileIcon.Height + Devide > Framebuffer.Graphics.Height - Devide)
					{
						Y = Devide + BarHeight;
						X += FileIcon.Width + Devide;
					}

					ClickEvent(BuiltInAppNames[i], false, X, Y, i);

					Framebuffer.Graphics.DrawImage(BuiltInAppIcon, X, Y, true);
					WindowManager.font.DrawString(X, Y + FileIcon.Height, BuiltInAppNames[i], FileIcon.Width + 8, WindowManager.font.FontSize * 3);
					Y += FileIcon.Height + Devide;
				}
			}

			for (int i = 0; i < names.Count; i++)
			{
				if (Y + FileIcon.Height + Devide > Framebuffer.Graphics.Height - Devide)
				{
					Y = Devide + BarHeight;
					X += FileIcon.Width + Devide;
				}

				ClickEvent(names[i].Name, names[i].Attribute == FileAttribute.Directory, X, Y, i + (IsAtRoot ? BuiltInAppNames.Length : 0));
				string name = names[i].Name;//.ToLower();
				if (name.EndsWith(".png"))
				{
					Framebuffer.Graphics.DrawImage(ImageIcon, X, Y, true);
				} else if (name.EndsWith(".nes"))
				{
					Framebuffer.Graphics.DrawImage(GameIcon, X, Y, true);
				} else if (name.EndsWith(".exe"))

				{
					Framebuffer.Graphics.DrawImage(AppIcon, X, Y, true);
				} else if (name.EndsWith(".wav"))
				{
					Framebuffer.Graphics.DrawImage(AudioIcon, X, Y, true);
				} else if (names[i].Attribute == FileAttribute.Directory)
				{
					Framebuffer.Graphics.DrawImage(FolderIcon, X, Y, true);
				} else
				{
					Framebuffer.Graphics.DrawImage(FileIcon, X, Y, true);
				}
				//BitFont.DrawString("Song", 0xFFFFFFFF, names[i], X, Y + FileIcon.Height, FileIcon.Width + 16);
				WindowManager.font.DrawString(X, Y + FileIcon.Height, names[i].Name, FileIcon.Width + 8, WindowManager.font.FontSize * 3);
				Y += FileIcon.Height + Devide;
				name.Dispose();
				names[i].Dispose();
			}
			names.Dispose();

			Framebuffer.Graphics.FillRectangle(Color.FromArgb(0xFF111111), 0, 0, Framebuffer.Graphics.Width, BarHeight);
			//BitFont.DrawString("Song", 0xFFFFFFFF, CurrentDirectory, 0, (BarHeight / 2) - (16 / 2));

			string pre = Prefix + Dir;
			WindowManager.font.DrawString(0, (BarHeight / 2) - (WindowManager.font.FontSize / 2), pre, Framebuffer.Graphics.Width);
			pre.Dispose();

#if Chinese
			string Result = $"FPS:{FPSMeter.FPS} | 处理器使用率:{ThreadPool.CPUUsage}% | 线程数量: {ThreadPool.ThreadCount} | 内存: {(Allocator.MemoryInUse / 1024)}/{((Allocator.NumPages * Allocator.PageSize) / 1024)}kbytes";
#else
			string Result = $"FPS:{FPSMeter.FPS} | CPU Usage:{ThreadPool.CPUUsage}% | ThreadCount: {ThreadPool.ThreadCount} | Memory: {Allocator.MemoryInUse / 1024}/{Allocator.NumPages * Allocator.PageSize / 1024}kbytes";
#endif
			//BitFont.DrawString("Song", 0xFFFFFFFF, Result, Framebuffer.Graphics.Width - BitFont.MeasureString("Song", Result) - 16, (BarHeight / 2) - (16 / 2));
			WindowManager.font.DrawString(Framebuffer.Graphics.Width - WindowManager.font.MeasureString(Result) - WindowManager.font.FontSize, (BarHeight / 2) - (WindowManager.font.FontSize / 2), Result);

			if (Control.MouseButtons.HasFlag(MouseButtons.Left) && !WindowManager.HasWindowMoving && !WindowManager.MouseHandled)
			{
				if (LastPoint.X == -1 && LastPoint.Y == -1)
				{
					LastPoint.X = Control.MousePosition.X;
					LastPoint.Y = Control.MousePosition.Y;
				} else
				{
					if (Control.MousePosition.X > LastPoint.X && Control.MousePosition.Y > LastPoint.Y)
					{
						Framebuffer.Graphics.FillRectangle(
							Color.FromArgb(0x7F2E86C1),
							LastPoint.X,
							LastPoint.Y,
							Control.MousePosition.X - LastPoint.X,
							Control.MousePosition.Y - LastPoint.Y);
					}

					if (Control.MousePosition.X < LastPoint.X && Control.MousePosition.Y < LastPoint.Y)
					{
						Framebuffer.Graphics.FillRectangle(
							Color.FromArgb(0x7F2E86C1),
							Control.MousePosition.X,
							Control.MousePosition.Y,
							LastPoint.X - Control.MousePosition.X,
							LastPoint.Y - Control.MousePosition.Y);
					}

					if (Control.MousePosition.X < LastPoint.X && Control.MousePosition.Y > LastPoint.Y)
					{
						Framebuffer.Graphics.FillRectangle(
							Color.FromArgb(0x7F2E86C1),
							Control.MousePosition.X,
							LastPoint.Y,
							LastPoint.X - Control.MousePosition.X,
							Control.MousePosition.Y - LastPoint.Y);
					}

					if (Control.MousePosition.X > LastPoint.X && Control.MousePosition.Y < LastPoint.Y)
					{
						Framebuffer.Graphics.FillRectangle(
							Color.FromArgb(0x7F2E86C1),
							LastPoint.X,
							Control.MousePosition.Y,
							Control.MousePosition.X - LastPoint.X,
							LastPoint.Y - Control.MousePosition.Y);
					}
				}
			} else
			{
				LastPoint.X = -1;
				LastPoint.Y = -1;
			}

			Result.Dispose();
		}

		public static Point LastPoint;

		private static void ClickEvent(string name, bool isDirectory, int X, int Y, int i)
		{
			if (Control.MouseButtons == MouseButtons.Left)
			{
				bool clickable = true;
				for (int d = 0; d < WindowManager.Windows.Count; d++)
				{
					if (WindowManager.Windows[d].Visible)
					{
						if (WindowManager.Windows[d].IsUnderMouse())
						{
							clickable = false;
						}
					}
				}

				if (!WindowManager.HasWindowMoving && clickable && !ClickLock && Control.MousePosition.X > X && Control.MousePosition.X < X + FileIcon.Width && Control.MousePosition.Y > Y && Control.MousePosition.Y < Y + FileIcon.Height)
				{
					IndexClicked = i;
					OnClick(name, isDirectory, X, Y);
				}
			} else
			{
				ClickLock = false;
			}

			if (IndexClicked == i)
			{
				int w = (int)(FileIcon.Width * 1.5f);
				Framebuffer.Graphics.FillRectangle(Color.FromArgb(0x7F2E86C1), X + ((FileIcon.Width / 2) - (w / 2)), Y, w, FileIcon.Height * 2);
			}
		}

		private static bool ClickLock = false;
		private static int IndexClicked;

		public static void OnClick(string name, bool isDirectory, int itemX, int itemY)
		{
			ClickLock = true;

			string devider = "/";
			string path = Dir + devider + name;
			if (name.ToLower().EndsWith(".png"))
			{
				byte[] buffer = File.Instance.ReadAllBytes(path);
				PNG png = new(buffer);
				buffer.Dispose();
				imageViewer.SetImage(png);
				png.Dispose();
				WindowManager.MoveToEnd(imageViewer);
				imageViewer.Visible = true;
			} else if (name.ToLower().EndsWith(".exe"))
			{
				WindowManager.MoveToEnd(Program.FConsole);
				if (Program.FConsole.Visible == false)
				{
					Program.FConsole.Visible = true;
				}

				//TO-DO disposing
				Console.WriteLine("Loading EXE...");
				byte[] buffer = File.Instance.ReadAllBytes(path);
				Process.Start(buffer);
			} else if (name.ToLower().EndsWith(".wav"))
			{
				if (Audio.HasAudioDevice)
				{
					byte[] buffer = File.Instance.ReadAllBytes(path);
					WAV.Decode(buffer, out byte[] pcm);
					Audio.Play(pcm);
					pcm.Dispose();
					buffer.Dispose();
				} else
				{
					msgbox.X = itemX + 75;
					msgbox.Y = itemY + 75;
#if Chinese
				msgbox.SetText("声卡不可用!");
#else
					msgbox.SetText("Audio controller is unavailable!");
#endif
					WindowManager.MoveToEnd(msgbox);
					msgbox.Visible = true;
				}
			} else if (name.ToLower().EndsWith(".nes"))
			{
				nesemu.OpenROM(File.Instance.ReadAllBytes(path));
				WindowManager.MoveToEnd(nesemu);
				nesemu.Visible = true;
			}
#if Chinese
			else if (name == "计算器")
#else
			  else if (name == "Calculator")
#endif
			{
				new Calculator(300, 500);
			}
#if Chinese
			else if (name == "监视器")
#else
			  else if (name == "Monitor")
#endif
			{
				new Monitor(200, 450);
			}
#if Chinese
			else if (name == " 时钟")
#else
			  else if (name == "Clock")
#endif
			{
				new Clock(650, 500);
			}
#if Chinese
			else if (name == " 画图")
#else
			  else if (name == "Paint")
#endif
			{
				new Paint(500, 200);
			}
#if Chinese
			else if (name == "贪吃蛇")
#else
			  else if (name == "Snake")
#endif
			{
				new Snake(600, 100);
			}
#if Chinese
			else if (name == "控制台")
#else
			  else if (name == "Console")
#endif
			{
				Program.FConsole.Visible = true;
			} else if (isDirectory)
			{
				string newd = Dir + devider + name;
				Dir.Dispose();
				Dir = newd;
			} else
			{
				WindowManager.MoveToEnd(new FileViewer(100, 100, path));
			}
			path.Dispose();
			devider.Dispose();
			name.Dispose();
		}
	}
}
#endif