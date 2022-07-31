using System;
using System.Collections.Generic;
using MOOS.Driver;
using MOOS.Misc;

namespace MOOS
{
    public class Animation
    {
        public int Value;
        public int MinimumValue;
        public int MaximumValue;

        public int PeriodInMS;
        public int ValueChangesInPeriod;

        public bool Stopped;

        public Animation()
        {
            PeriodInMS = 1;
            ValueChangesInPeriod = 1;
        }
        public override void Dispose()
        {
            Animator.Animations.Remove(this);
            base.Dispose();
        }
    }

    public static class Animator
    {
        internal static List<Animation> Animations = new();

        public static unsafe void Initialize()
        {
            Interrupts.EnableInterrupt(0x20, &OnInterrupt);
        }

        public static void AddAnimation(Animation ani)
        {
            Animations.Add(ani);
        }

        public static void OnInterrupt()
        {
            for (int i = 0; i < Animations.Count; i++)
            {
                Animation v = Animations[i];
                if (!v.Stopped)
                {
                    if ((Timer.Ticks % (ulong)v.PeriodInMS) == 0)
                    {
                        v.Value = Math.Clamp(v.Value + v.ValueChangesInPeriod, v.MinimumValue, v.MaximumValue);
                    }
                }
            }
        }
    }
}