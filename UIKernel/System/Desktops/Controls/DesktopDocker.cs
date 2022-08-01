using MOOS;
using MOOS.FS;
using MOOS.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace System.Desktops.Controls
{
    public class DesktopDocker : DesktopControl
    {
        public List<DockerItem> items { set; get; }
        
        Image LDocker { set; get; }
        Image RDocker { set; get; }

        int minWidth = 400;
        Brush _borderColor;
        public DesktopDocker()
        {
            Background = ColorConverter.FromARGB(255,222,222,222);
            Foreground = Brushes.Black;
            _borderColor = Brushes.Black;

            Height = 49;
            items = new List<DockerItem>();

            LDocker = new PNG(File.Instance.ReadAllBytes("Images/L_docker.png"));
            RDocker = new PNG(File.Instance.ReadAllBytes("Images/R_docker.png"));

            DockerItem item = new DockerItem();
            item.Icon = DesktopIcons.AppIcon;
            item.Name = "Moos";
            items.Add(item);

            item = new DockerItem();
            item.Icon = DesktopIcons.AppTerminal;
            item.Name = "Terminal";
            item.Command = new ICommand(onTermina);
            items.Add(item);

            item = new DockerItem();
            item.Icon = DesktopIcons.BuiltInAppIcon;
            item.Name = "NativeApp";
            items.Add(item);
          
            onWidthItems();
            onItemsReorder();
        }

        void onTermina(object obj)
        {
            Debug.WriteLine($"[Docker] Terminal");
        }

        void onItemsReorder()
        {
            int _x = (X + Framebuffer.Width) - ((Framebuffer.Width / 2) + (Width / 2));
            int _y = (Y + (Framebuffer.Height - Height) - 2);

            for (int i = 0; i < items.Count; i++)
            {
                if (i == 0)
                {
                    int _tx = ((_x + (Width / 2)) - (((items[i].Width * (items.Count + 1)) / 2)));
                    items[i].X = (_tx + (((items[i].Width * items.Count + 1) / items.Count) / 2));
                }
                else
                {
                    items[i].X = items[i - 1].X + 64;
                }
                items[i].Y = _y;
            }
        }

        void onWidthItems()
        {
            if (items.Count == 0)
            {
                Width = minWidth;
                return;
            }

            Width = items.Count * 64;

            if (Width  < minWidth)
            {
                Width = minWidth;
            }
        }

        public override void Update()
        {
            base.Update();

            onWidthItems();

            for (int i = 0; i < items.Count; i++)
            {
                items[i].Update();
            }
        }

        public override void Draw()
        {
            base.Draw();

            int _x = (X + Framebuffer.Width) - ((Framebuffer.Width / 2) + (Width / 2));
            int _y = (Y + (Framebuffer.Height - Height) - 2);

            Framebuffer.Graphics.DrawImage((_x - LDocker.Width), (_y - 1), LDocker);
            Framebuffer.Graphics.DrawImage(((_x + Width) + (RDocker.Width/2)) - (RDocker.Width / 2), (_y - 1), RDocker);
            Framebuffer.Graphics.AFillRectangle(_x, _y, Width, Height, Background.Value);
            Framebuffer.Graphics.DrawLine(_x, _y, ((_x +Width) - (RDocker.Width/2)) + (RDocker.Width / 2), _y , _borderColor.Value);
            Framebuffer.Graphics.DrawLine(_x , (_y + Height), ((_x + Width) - (RDocker.Width / 2)) + (RDocker.Width / 2), (_y + Height), _borderColor.Value);

            for (int i = 0; i < items.Count; i++)
            {
                items[i].Draw();
            }
        }
    }
}
