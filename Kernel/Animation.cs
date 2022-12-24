using MOOS.Driver;
using MOOS.Misc;
using System;
using System.Collections.Generic;

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
    }

    public static class Animator
    {
        static List<Animation> Animations;

        public static unsafe void Initialize() 
        {
            Animations = new List<Animation>();
            Interrupts.EnableInterrupt(0x20, &OnInterrupt);
        }

        public static void AddAnimation(Animation ani) 
        {
            Animations.Add(ani);
        }

        public static void DisposeAnimation(Animation ani) 
        {
            Animations.Remove(ani);
            ani.Dispose();
        }

        public static void OnInterrupt() 
        {
            for(int i = 0; i < Animations.Count; i++)
            {
                Animation v = Animations[i];
                if(!v.Stopped)
                {
                    if (v.PeriodInMS == 0) continue;
                    if((Timer.Ticks % (ulong)v.PeriodInMS) == 0)
                    {
                        v.Value = Math.Clamp(v.Value + v.ValueChangesInPeriod, v.MinimumValue, v.MaximumValue);
                    }
                }
            }
        }
    }
}
