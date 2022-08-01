using System;
using System.Runtime;
using MOOS.Driver;
using MOOS.FS;
using MOOS.Misc;

namespace MOOS
{
	public static unsafe class API
	{
		public static unsafe void* HandleSystemCall(string name)
		{
			switch (name)
			{
				case "Allocate":
					return (delegate*<ulong, nint>)&Allocator.Allocate;
				case "Reallocate":
					return (delegate*<nint, ulong, nint>)&Allocator.Reallocate;
				case "MemCopy":
					return (delegate*<nint, nint, ulong, void>)&Allocator.MemoryCopy;
				case "Write":
					return (delegate*<string, void>)&Console.Write;
				case "SingleBuffered":
					return (delegate*<void>)new Action(() =>
					{
						Framebuffer.DoubleBuffered = false;
					}).m_functionPointer;
				case "DoubleBuffered":
					return (delegate*<void>)new Action(() =>
					{
						Framebuffer.DoubleBuffered = true;
					}).m_functionPointer;
				case "Sleep":
					return (delegate*<ulong, void>)&Thread.Sleep;
				case "GetTime":
					return (delegate*<ulong>)&API_GetTime;
				case "Error":
					return (delegate*<string, bool, void>)&Panic.Error;
			}
			Panic.Error($"System call \"{name}\" is not found");
			return null;
		}

		[RuntimeExport("GetTime")]
		public static ulong API_GetTime()
		{
			ulong century = RTC.Century;
			ulong year = RTC.Year;
			ulong month = RTC.Month;
			ulong day = RTC.Day;
			ulong hour = RTC.Hour;
			ulong minute = RTC.Minute;
			ulong second = RTC.Second;

			ulong time = 0;

			time |= century << 56;
			time |= year << 48;
			time |= month << 40;
			time |= day << 32;
			time |= hour << 24;
			time |= minute << 16;
			time |= second << 8;

			return time;
		}

		private static void Write(char s)
		{
			Console.Write(s);
		}

		private static void WriteLine()
		{
			Console.WriteLine();
		}

		[RuntimeExport("UnLock")]
		private static void UnLock()
		{
			if (ThreadPool.CanLock)
			{
				if (ThreadPool.Locked)
				{
					if (ThreadPool.Locker == SMP.ThisCPU)
					{
						ThreadPool.UnLock();
					}
				}
			}
		}
		[RuntimeExport("Lock")]
		private static void Lock()
		{
			if (ThreadPool.CanLock)
			{
				if (!ThreadPool.Locked)
				{
					ThreadPool.Lock();
				}
			}
		}

		[RuntimeExport("Error")]
		public static void Error(string message)
		{
			Panic.Error(message);
		}
		[RuntimeExport("DebugWrite")]
		public static void API_DebugWrite(char c)
		{
			Serial.Write(c);
		}

		[RuntimeExport("DebugWriteLine")]
		public static void API_DebugWriteLine(string s)
		{
			Serial.WriteLine(s);
		}

		public static void SwitchToConsoleMode()
		{
			Framebuffer.DoubleBuffered = false;
		}

		public static void ReadAllBytes(string name, ulong* length, byte** data)
		{
			byte[] buffer = File.Instance.ReadAllBytes(name);

			*data = (byte*)Allocator.Allocate((ulong)buffer.Length);
			*length = (ulong)buffer.Length;
			fixed (byte* p = buffer)
			{
				Native.Movsb(*data, p, *length);
			}

			buffer.Dispose();
		}

		public static ulong GetTick()
		{
			return Timer.Ticks;
		}
	}
}