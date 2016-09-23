using NUnit.Framework;
using Buildron.Infrastructure.BuildsProviders.Filter;
using Rhino.Mocks;
using Buildron.Domain.Builds;

namespace Buildron.Infrastructure.FunctionalTests.BuildsProviders.Filter
{
	[Category("Buildron.Infrastructure")]
	[Category("Unity")]
	public class FilterBuildEventInterceptorTest
	{
		[Test]
		public void OnStatusChanged_BuildNotFilter_Canceled ()
		{
			var b1 = new Build { Id = "1" };
			var b2 = new Build { Id = "2" };

			var filter = MockRepository.GenerateMock<IBuildFilter> ();
			filter.Expect (p => p.Filter (b1)).Return (true);
			filter.Expect (p => p.Filter (b2)).Return (false);
			var target = new FilterBuildEventInterceptor (filter);

			var b1Event = new BuildEvent (b1);
			target.OnStatusChanged (b1Event);
			Assert.IsFalse (b1Event.Canceled);

			var b2Event = new BuildEvent (b2);
			target.OnStatusChanged (b2Event);
			Assert.IsTrue (b2Event.Canceled);
		}

		[Test]
		public void OnTriggeredByChanged_BuildNotFilter_Canceled ()
		{
			var b1 = new Build { Id = "1" };
			var b2 = new Build { Id = "2" };

			var filter = MockRepository.GenerateMock<IBuildFilter> ();
			filter.Expect (p => p.Filter (b1)).Return (true);
			filter.Expect (p => p.Filter (b2)).Return (false);
			var target = new FilterBuildEventInterceptor (filter);

			var b1Event = new BuildEvent (b1);
			target.OnTriggeredByChanged (b1Event);
			Assert.IsFalse (b1Event.Canceled);

			var b2Event = new BuildEvent (b2);
			target.OnTriggeredByChanged (b2Event);
			Assert.IsTrue (b2Event.Canceled);
		}
	}
}