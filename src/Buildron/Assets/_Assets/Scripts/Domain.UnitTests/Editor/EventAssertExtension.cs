using System.Collections;
using System;
using NUnit.Framework;

public static class EventAssertExtension 
{
	public static AssertRaised<TEventArgs> CreateAssert<TEventArgs>(this object instance, string eventName, int expectedRaiseCount)
	{
		var result = new AssertRaised<TEventArgs> (eventName, expectedRaiseCount);

		var instanceType = instance.GetType ();
		var e = instanceType.GetEvent (eventName);
		e.AddEventHandler (instance, Delegate.CreateDelegate(e.EventHandlerType, result, "AddRaisedCount"));
		return result;
	}

	public class AssertRaised<TEventArgs>
	{
		private readonly string m_eventName;
		private readonly int m_expectedRaiseCount;
		private int m_actualRaiseCount;

		public AssertRaised(string eventName, int expectedRaiseCount)
		{
			m_eventName = eventName;
			m_expectedRaiseCount = expectedRaiseCount;
		}

		public void AddRaisedCount(object sender, TEventArgs args)
		{
			m_actualRaiseCount++;
		}

		public void Assert()
		{
			NUnit.Framework.Assert.AreEqual (
				m_expectedRaiseCount,
				m_actualRaiseCount, 
				"{0} expected raised {1} times, but was {2}", m_eventName, m_expectedRaiseCount, m_actualRaiseCount);
		}
	}
}