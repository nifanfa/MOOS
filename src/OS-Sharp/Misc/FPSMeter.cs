// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the  MIT licence
namespace Kernel
{
    internal static class FPSMeter
    {
        public static int FPS = 0;

        public static int LastS = -1;
        public static int Tick = 0;

        public static void Update()
        {
            if (LastS == -1)
            {
                LastS = RTC.Second;
            }
            if (RTC.Second - LastS != 0)
            {
                if (RTC.Second > LastS)
                {
                    FPS = Tick / (RTC.Second - LastS);
                }
                LastS = RTC.Second;
                Tick = 0;
            }
            Tick++;
        }
    }
}

