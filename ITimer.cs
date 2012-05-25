using System;

namespace Timer
{
	public interface ITimer
	{
		void Start();
		void Stop();
		void UpdateTimeInterval(TimeSpan interval);
	}
}