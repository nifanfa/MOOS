#if HasGUI
using MOOS.Driver;
using MOOS.Misc;
using System;
using System.Collections.Generic;

namespace MOOS.GUI
{
    public enum NotificationLevel 
    {
        None,
        Error
    }

    public class Nofity
    {
        public int X, Y;
        public readonly string Message;
        public NotificationLevel NotificationLevel;
        public int SWidth;
        public int SHeight;

        public int Remain;

        public Nofity(string msg,NotificationLevel level = NotificationLevel.None)
        {
            Remain = 0;
            Message = msg;
            X = 0; Y = 0;
            SWidth = WindowManager.font.MeasureString(msg);
            SHeight = WindowManager.font.FontSize;
            NotificationLevel = level;
        }

        public override void Dispose()
        {
            Message.Dispose();
            base.Dispose();
        }
    }

    public static class NotificationManager
    {
        static List<Nofity> Notifications;

        public static unsafe void Initialize()
        {
            Notifications = new();

            Interrupts.EnableInterrupt(0x20, &OnInterrupt);

            Add(new Nofity("Welcome to MOOS"));
            Add(new Nofity(Audio.HasAudioDevice ? "Info: Audio controller available" : "Warning: No audio controller on this PC", Audio.HasAudioDevice ? NotificationLevel.None : NotificationLevel.Error));
        }

        public static void Add(Nofity nofity)
        {
            Notifications.Add(nofity);
        }

        public const int Devide = 30;

        public const int Threshold = 50;

        public const int DisposeAfter = 2000;

        public static void Update()
        {
            int y = Devide * 2;
            for (int i = 0; i < Notifications.Count; i++)
            {
                var v = Notifications[i];

                Framebuffer.Graphics.FillRectangle(Framebuffer.Width - v.X, v.Y + y, v.SWidth + Devide, v.SHeight + Devide, 0xFF111111);
                Framebuffer.Graphics.DrawRectangle(Framebuffer.Width - v.X, v.Y + y, v.SWidth + Devide, v.SHeight + Devide, 0xFF222222);
                Framebuffer.Graphics.FillRectangle(Framebuffer.Width - v.X, v.Y + y, 5, v.SHeight + Devide, v.NotificationLevel == NotificationLevel.None? 0xFF80B000 : 0xFFE74C3C);
                WindowManager.font.DrawString(Framebuffer.Width - v.X + (Devide / 2), v.Y + y + (Devide / 2), v.Message);

                y += v.SHeight + Devide;
                y += Devide;
            }
        }

        public static void OnInterrupt()
        {
            int num = 0;

            for (int i = 0; i < Notifications.Count; i++)
            {
                var v = Notifications[i];

                if (v.X < (Threshold + v.SWidth))
                {
                    num++;
                }
            }

            if((Timer.Ticks % 2) == 0)
            for (int i = 0; i < Notifications.Count; i++)
            {
                var v = Notifications[i];
                if (v.X < (Threshold + v.SWidth))
                {
                    v.X ++;
                    break;
                }
            }

            if (num == 0)
            {
                for (int i = 0; i < Notifications.Count; i++)
                {
                    var v = Notifications[i];

                    if (v.X < (Threshold + v.SWidth))
                    {
                        break;
                    }
                    else
                    {
                        v.Remain++;
                        if (v.Remain > DisposeAfter)
                        {
                            Notifications.Remove(v);
                            v.Dispose();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
#endif