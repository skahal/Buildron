using NUnit.Framework;
using Buildron.Infrastructure.BuildsProviders.Filter;
using Rhino.Mocks;
using Buildron.Domain.Builds;
using Buildron.Domain.RemoteControls;
using Buildron.Domain.Servers;
using Buildron.Domain.Users;
using System;

namespace Buildron.Infrastructure.FunctionalTests.BuildsProviders.Filter
{
	[Category("Buildron.Infrastructure")]
	[Category("Unity")]
	public class FilterBuildsProviderTest
	{
		[Test]
		public void Constructor_UnderlyingBuildsProvider_Filtered ()
		{
			var user1 = new RemoteControl { UserName = "u1" };
			var build1 = new Build { Id = "b1", Status = BuildStatus.Success };
			var build2 = new Build { Id = "b2", Status = BuildStatus.Failed };

			var underlying = MockRepository.GenerateMock<IBuildsProvider> ();
			underlying.Expect(u => u.AuthenticateUser(user1));
			underlying.Expect (u => u.AuthenticationRequirement).Return (AuthenticationRequirement.Never);
			underlying.Expect (u => u.AuthenticationTip).Return ("Tip1");
			underlying.Expect (u => u.Name).Return ("Name1");
			underlying.Expect (u => u.RefreshAllBuilds ());
			underlying.Expect (b => b.RunBuild (user1, build1));
			underlying.Expect (b => b.StopBuild (user1, build2));

			var rcListener = MockRepository.GenerateMock<IRemoteControlService> ();
			var serverService = MockRepository.GenerateMock<IServerService> ();
			serverService.Expect (s => s.GetState ()).Return (new ServerState {
				BuildFilter = new BuildFilter
				{
					FailedEnabled = false
				}
			});

			var target = new FilterBuildsProvider (underlying, rcListener, serverService);

			target.AuthenticateUser (user1);
			Assert.AreEqual (AuthenticationRequirement.Never, target.AuthenticationRequirement);
			Assert.AreEqual ("Tip1", target.AuthenticationTip);
			var buildsRefreshedRaised = target.CreateAssert<EventArgs> ("BuildsRefreshed", 1);
			var buildUpdatedRaised = target.CreateAssert<BuildUpdatedEventArgs> ("BuildUpdated", 1);
			Assert.AreEqual ("Name1", target.Name);
			target.RefreshAllBuilds ();
			target.RunBuild (user1, build1);
			var serverDownRaised = target.CreateAssert<EventArgs>("ServerDown", 1);
			var serverUpRaised = target.CreateAssert<EventArgs>("ServerUp", 1);
			target.StopBuild (user1, build2);
			var userAuthenticationCompletedRaised = target.CreateAssert<UserAuthenticationCompletedEventArgs> ("UserAuthenticationCompleted", 1);
		
			underlying.Raise (u => u.BuildsRefreshed += null, null, new BuildsRefreshedEventArgs (null, null, null));
			underlying.Raise (u => u.BuildUpdated += null, null, new BuildUpdatedEventArgs (build1));
			underlying.Raise (u => u.BuildUpdated += null, null, new BuildUpdatedEventArgs (build2));
			underlying.Raise (u => u.ServerDown += null, null, EventArgs.Empty);
			underlying.Raise (u => u.ServerUp += null, null, EventArgs.Empty);
			underlying.Raise (u => u.UserAuthenticationCompleted += null, null, new UserAuthenticationCompletedEventArgs(null, true));
		
			buildsRefreshedRaised.Assert ();
			buildUpdatedRaised.Assert ();
			serverDownRaised.Assert ();
			serverUpRaised.Assert ();
			userAuthenticationCompletedRaised.Assert ();
			underlying.VerifyAllExpectations ();
		}

		[Test]
		public void Filter_Builds_Filtered ()
		{
			var filter = new BuildFilter ();
			var underlying = MockRepository.GenerateMock<IBuildsProvider> ();
			var rcListener = MockRepository.GenerateMock<IRemoteControlService> ();
			var serverService = MockRepository.GenerateMock<IServerService> ();
			serverService.Expect (s => s.GetState ()).Return (new ServerState {
				BuildFilter = filter
			});

			var target = new FilterBuildsProvider (underlying, rcListener, serverService);
			var build = new Build 
			{
				Status = BuildStatus.Running,
				Configuration = new BuildConfiguration { Name = "BC1" }
			};

			filter.KeyWord = "BC1";
			Assert.IsTrue (target.Filter (build));

			filter.KeyWord = "-2";
			Assert.IsTrue (target.Filter (build));

			filter.KeyWord = "-1";
			Assert.IsFalse (target.Filter (build));

			filter.KeyWord = "BC1";
			filter.FailedEnabled = false;
			filter.QueuedEnabled = false;
			filter.SuccessEnabled = false;
			Assert.IsTrue (target.Filter (build));

			filter.RunningEnabled = false;
			Assert.IsFalse (target.Filter (build));

			build.Status = BuildStatus.Failed;
			filter.FailedEnabled = true;
			Assert.IsTrue (target.Filter (build));

			build.Status = BuildStatus.Success;
			filter.FailedEnabled = false;
			filter.SuccessEnabled = true;
			Assert.IsTrue (target.Filter (build));

			build.Status = BuildStatus.Queued;
			filter.SuccessEnabled = false;
			filter.QueuedEnabled = true;
			Assert.IsTrue (target.Filter (build));
		}
	}
}