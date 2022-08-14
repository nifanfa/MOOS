namespace MOOS
{
    internal class FPSMeter
    {
        public int FPS = 0;

        public int LastS = -1;
        public int Tick = 0;

        public void Update()
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