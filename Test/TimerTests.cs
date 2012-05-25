using System;
using NUnit.Framework;

namespace Timer.Test
{
	[TestFixture]
	public class TimerTests
	{
		private TestableTimer timer;
		int tickCount;

		[SetUp]
		public void SetUp()
		{
			tickCount = 0;
			timer = new TestableTimer(() => tickCount++);
		}

		[Test]
		public void TimerExecutesCallbackFunctionAsExpected()
		{
			timer.Start();
			timer.ExecuteCallBack();
			timer.ExecuteCallBack();
			timer.Stop();

			Assert.That(tickCount, Is.EqualTo(2));
		}

		[Test]
		public void UpdateTimeIntervalSetsNewIntervalAsExpected()
		{
			timer.Start();
			timer.UpdateTimeInterval(TimeSpan.FromMilliseconds(20));
			timer.Stop();
			
			Assert.That(timer.Interval.Milliseconds, Is.EqualTo(20));
		}
	}

	public class TestableTimer : Timer
	{
		public bool IsDisposed;

		public TestableTimer(Action callback)
			: base(callback, TimeSpan.Zero)
		{
		}

		public override void Start()
		{
		}

		public void ExecuteCallBack()
		{
			clientCallback();
		}

		public override void Dispose()
		{
			IsDisposed = true;
		}

		public TimeSpan Interval
		{
			get { return interval; }
		}
	}
}