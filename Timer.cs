﻿using System;
using System.Runtime.InteropServices;

namespace Timer
{
	/// <summary>
	/// This timer class uses unmanaged DLL for better accuracy at short frequencies. 
	/// This class is not thread safe.
	/// http://stackoverflow.com/questions/416522/c-sharp-why-are-timer-frequencies-extremely-off
	/// </summary>
	public class Timer : IDisposable, ITimer
	{
		private const string WINMM = "winmm.dll";
		private readonly MMTimerProc callbackFunction;
		protected readonly Action clientCallback;
		protected TimeSpan interval;
		private uint timerId;

		public Timer(Action clientCallback, TimeSpan interval)
		{
			callbackFunction = CallbackFunction;
			this.clientCallback = clientCallback;
			this.interval = interval;
		}

		public virtual void Dispose()
		{
			Stop();
		}

		public virtual void Start()
		{
			StartUnmanagedTimer();

			if (timerId == 0)
			{
				throw new Exception("TimeSet Event Error");
			}
		}

		public void Stop()
		{
			if (timerId == 0) return;
			StopUnmanagedTimer();
		}

		public void UpdateTimeInterval(TimeSpan interval)
		{
			Stop();
			this.interval = interval;
			Start();
		}

		private void StartUnmanagedTimer()
		{
			timerId = timeSetEvent((uint) interval.TotalMilliseconds, 0, callbackFunction, 0, 1);
		}

		private void StopUnmanagedTimer()
		{
			timeKillEvent(timerId);
			timerId = 0;
		}

		private void CallbackFunction(UInt32 timerid, UInt32 msg, IntPtr user, UInt32 dw1, UInt32 dw2)
		{
			clientCallback();
		}

		[DllImport(WINMM)]
		private static extern uint timeSetEvent(
			UInt32 uDelay,
			UInt32 uResolution,
			[MarshalAs(UnmanagedType.FunctionPtr)] MMTimerProc lpTimeProc,
			UInt32 dwUser,
			Int32 fuEvent
			);

		[DllImport(WINMM)]
		private static extern uint timeKillEvent(uint uTimerID);

		#region Nested type: MMTimerProc

		private delegate void MMTimerProc(UInt32 timerid, UInt32 msg, IntPtr user, UInt32 dw1, UInt32 dw2);

		#endregion
	}
}