namespace System.Diagnostics
{
	public class Stopwatch
	{
		private long elapsed;
		private long startTimeStamp;
		private bool isRunning;
		public bool IsRunning => isRunning;
		private const long TicksPerMillisecond = 10000;
		public TimeSpan Elapsed => new(GetElapsedDateTimeTicks());

		public long ElapsedMilliseconds => GetElapsedDateTimeTicks() / TicksPerMillisecond;

		public long ElapsedTicks => GetRawElapsedTicks();
		public Stopwatch()
		{
			Reset();
		}
		public static Stopwatch StartNew()
		{
			Stopwatch s = new();
			s.Start();
			return s;
		}
		public void Stop()
		{
			// Calling stop on a stopped Stopwatch is a no-op.
			if (isRunning)
			{
				long endTimeStamp = GetTimestamp();
				long elapsedThisPeriod = endTimeStamp - startTimeStamp;
				elapsed += elapsedThisPeriod;
				isRunning = false;

				if (elapsed < 0)
				{
					// When measuring small time periods the StopWatch.Elapsed* 
					// properties can return negative values.  This is due to 
					// bugs in the basic input/output system (BIOS) or the hardware
					// abstraction layer (HAL) on machines with variable-speed CPUs
					// (e.g. Intel SpeedStep).

					elapsed = 0;
				}
			}
		}
		public void Reset()
		{
			elapsed = 0;
			isRunning = false;
			startTimeStamp = 0;
		}
		public void Restart()
		{
			elapsed = 0;
			startTimeStamp = GetTimestamp();
			isRunning = true;
		}
		private long GetRawElapsedTicks()
		{
			long timeElapsed = elapsed;

			if (isRunning)
			{
				// If the StopWatch is running, add elapsed time since
				// the Stopwatch is started last time. 
				long currentTimeStamp = GetTimestamp();
				long elapsedUntilNow = currentTimeStamp - startTimeStamp;
				timeElapsed += elapsedUntilNow;
			}
			return timeElapsed;
		}
		public void Start()
		{
			// Calling start on a running Stopwatch is a no-op.
			if (!isRunning)
			{
				startTimeStamp = GetTimestamp();
				isRunning = true;
			}
		}
		private long GetElapsedDateTimeTicks()
		{
			long rawTicks = GetRawElapsedTicks();
			return rawTicks;
		}
		public static long GetTimestamp()
		{
			return DateTime.Now.Ticks;
		}
	}
}
