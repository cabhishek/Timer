using System;
using System.Threading;

namespace Timer
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			int tickCount = 0;
			//sample callback which does the work you want.
			Action callback = () => { tickCount++; };

			// perform work after a specfic interval
			TimeSpan interval = TimeSpan.FromMilliseconds(1);

			//instatiate the timer
			var timer = new Timer(callback, interval);

			//start it
			timer.Start();

			//wait and give some room for processing 
			Thread.Sleep(20);

			timer.Stop();
			timer.Dispose();

			//tick count should be ~20-23
			Console.WriteLine("Tick Count => {0}", tickCount);

			// keep result window open to see the result
			Thread.Sleep(2000);
		}
	}
}