/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Internal.Runtime.CompilerServices;
using MOOS.Driver;

namespace MOOS.Misc
{
	public unsafe class Thread
	{
		public bool Terminated;
		public IDT.IDTStackGeneric* Stack;
		public int RunOnWhichCPU;
		public bool IsIdleThread = false;

		public Thread(delegate*<void> method, ulong stack_size = 16384)
		{
			NewThread(method, stack_size);
		}

		private void NewThread(delegate*<void> method, ulong stack_size)
		{
			Stack = (IDT.IDTStackGeneric*)Allocator.Allocate((ulong)sizeof(IDT.IDTStackGeneric));

			Stack->irs.cs = 0x08;
			Stack->irs.ss = 0x10;
			Stack->irs.rsp = ((ulong)Allocator.Allocate(stack_size)) + stack_size;

			Stack->irs.rsp -= 8;
			*(ulong*)Stack->irs.rsp = (ulong)(delegate*<void>)&ThreadPool.Terminate;

			Stack->irs.rflags = 0x202;

			Stack->irs.rip = (ulong)method;

			Terminated = false;
		}

		public Thread(Action action, ulong stack_size = 16384)
		{
			NewThread((delegate*<void>)action.m_functionPointer, stack_size);
		}

		public Thread Start()
		{
			lock (this)
			{
				//Bootstrap CPU
				RunOnWhichCPU = 0;
				ThreadPool.Threads.Add(this);
				return this;
			}
		}

		public Thread Start(int run_on_which_cpu)
		{
			lock (this)
			{
				bool hasThatCPU = false;
				for (int i = 0; i < ACPI.LocalAPIC_CPUIDs.Count; i++)
				{
					if (ACPI.LocalAPIC_CPUIDs[i] == run_on_which_cpu)
					{
						hasThatCPU = true;
					}
				}
				if (!hasThatCPU)
				{
					run_on_which_cpu = 0;
				}

				RunOnWhichCPU = run_on_which_cpu;
				ThreadPool.Threads.Add(this);
				return this;
			}
		}

		public static void Sleep(ulong Millionsecos)
		{
			Timer.Sleep(Millionsecos);
		}
	}

	internal static unsafe class ThreadPool
	{
		public static List<Thread> Threads;
		public static bool Initialized = false;
		public static bool Locked = false;
		public static long Locker = 0;

		private static int Index
		{
			get => Indexs[SMP.ThisCPU];
			set => Indexs[SMP.ThisCPU] = value;
		}

		public static void Initialize()
		{
			Native.Cli();
			//Bootstrap CPU
			if (SMP.ThisCPU == 0)
			{
				byte size = 0;
				for (int i = 0; i < ACPI.LocalAPIC_CPUIDs.Count; i++)
				{
					if (ACPI.LocalAPIC_CPUIDs[i] > size)
					{
						size = ACPI.LocalAPIC_CPUIDs[i];
					}
				}

				Indexs = new int[size + 1];

				Locked = false;
				Initialized = false;
				Threads = new();
				//At least a thread for each CPU to make Thread Pool work
				Thread t = new(&IdleThread)
				{
					IsIdleThread = true
				};
				t.Start(0);
				Initialized = true;
			}
			//Application CPU
			else
			{
				//At least a thread for each CPU to make Thread Pool work
				Thread t = new(&IdleThread)
				{
					IsIdleThread = true
				};
				t.Start((int)SMP.ThisCPU);
			}
			Native.Sti();
			Schedule_Next(); //start scheduling
		}

		public static void Terminate()
		{
			Console.Write(Index.ToString());
			Threads[Index].Terminated = true;
			Schedule_Next();
			Panic.Error("Termination Failed!");
		}

		[DllImport("*")]
		public static extern void Schedule_Next();

		public static void TestThread()
		{
			Console.WriteLine("Non-Loop Thread Test!");
			return;
		}

		public static void A()
		{
			for (; ; )
			{
				Console.WriteLine("Thread A");
			}
		}

		public static void B()
		{
			for (; ; )
			{
				Console.WriteLine("Thread B");
			}
		}

		public static void IdleThread()
		{
			for (; ; )
			{
				Schedule_Next();
			}
		}

		private static int[] Indexs;

		public static bool CanLock => Unsafe.As<bool, ulong>(ref Initialized);

		public static void Lock()
		{
			Locker = SMP.ThisCPU;
			Locked = true;

			LocalAPIC.SendAllInterrupt(0x20);
		}

		public static void UnLock()
		{
			Locked = false;
		}

		public static int ThreadCount => Threads.Count;

		private static uint TickAll;
		private static uint TickIdle;

		public static uint CPUUsage;

		public static void Schedule(IDT.IDTStackGeneric* stack)
		{
			if (!Initialized)
			{
				return;
			}

			//Lock all processors except locker CPU
			if (Locked && Locker != SMP.ThisCPU)
			{
				while (Locked)
				{
					Native.Nop();
				}

				return;
			}

			//Lock locker CPU
			if (Locked && Locker == SMP.ThisCPU)
			{
				return;
			}

			for (; ; )
			{
				if (
				!Threads[Index].Terminated &&
				Threads[Index].RunOnWhichCPU == SMP.ThisCPU
				)
				{
					Native.Movsb(Threads[Index].Stack, stack, (ulong)sizeof(IDT.IDTStackGeneric));
					break;
				}
				Index = (Index + 1) % Threads.Count;
			}

			do
			{
				Index = (Index + 1) % Threads.Count;
			} while
			(
			Threads[Index].Terminated ||
			Threads[Index].RunOnWhichCPU != SMP.ThisCPU
			);

			#region CPU Usage
			if (SMP.ThisCPU == 0)
			{
				if ((Timer.Ticks % 100) == 0)
				{
					if (TickAll != 0 && TickIdle != 0)
					{
						CPUUsage = 100 - (TickIdle * 100 / TickAll);
					}

					TickIdle = 0;
					TickAll = 0;
				}
			}
			if (Threads[Index].IsIdleThread)
			{
				TickIdle++;
			}
			TickAll++;
			#endregion

			Native.Movsb(stack, Threads[Index].Stack, (ulong)sizeof(IDT.IDTStackGeneric));
		}
	}
}