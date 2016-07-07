using Buildron.Domain;
using NUnit.Framework;
using Rhino.Mocks;
using Skahal.Logging;
using Buildron.Domain.Builds;
using Buildron.Domain.Users;
using System;
using UnityEngine;

namespace Buildron.Domain.UnitTests.Users
{
    [Category("Buildron.Domain")]
    public class UserServiceTest
    {
        [Test]
        public void Initialize_BuildsProviderRaiseEvents_UserEventsRaised()
        {
            var provider = MockRepository.GenerateMock<IBuildsProvider>();
			var target = new UserService(new IUserAvatarProvider[0], new IUserAvatarProvider[0], MockRepository.GenerateMock<ISHLogStrategy>());
			target.Initialize (provider);

            var user1 = new User
            {
                UserName = "u1"
            };

            var user2 = new User
            {
                UserName = "u2"
            };

            var build1 = new Build
            {
                Id = "1",
                TriggeredBy = user1
            };

            user1.Builds.Add(build1);

            var userFoundCalledCount = 0;
            target.UserFound += (s1, e1) =>
            {
                userFoundCalledCount++;

                switch(userFoundCalledCount)
                {
                    case 1:
                        Assert.AreEqual(user1, e1.User);
                        break;

                    case 2:
                        Assert.AreEqual(user2, e1.User);
                        break;
                }
            };

            var userTriggeredBuildCount = 0;
            target.UserTriggeredBuild += (s2, e2) =>
            {
                userTriggeredBuildCount++;
            };

            var userUpdatedCount = 0;
            target.UserUpdated += (s2, e2) =>
            {
                userUpdatedCount++;
            };

            var userRemovedCount = 0;
            target.UserRemoved += (s3, e3) =>
            {
                userRemovedCount++;
            };


            // User1 and build1.
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(build1));
            Assert.AreEqual(1, userFoundCalledCount);
            Assert.AreEqual(1, userTriggeredBuildCount);
            Assert.AreEqual(1, userUpdatedCount);
            Assert.AreEqual(0, userRemovedCount);

            provider.Raise((p) => p.BuildsRefreshed += null, null, new BuildsRefreshedEventArgs(null, null, null));
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(build1));
            Assert.AreEqual(1, userFoundCalledCount);
            Assert.AreEqual(1, userTriggeredBuildCount);
            Assert.AreEqual(2, userUpdatedCount);
            Assert.AreEqual(0, userRemovedCount);

            Assert.AreEqual(1, target.Users.Count);

            // User2 and build1.
            build1.TriggeredBy = user2;
            user2.Builds.Add(build1);
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(build1));
            Assert.AreEqual(2, userFoundCalledCount);
            Assert.AreEqual(2, userTriggeredBuildCount);
            Assert.AreEqual(3, userUpdatedCount);
            Assert.AreEqual(0, userRemovedCount);

            provider.Raise((p) => p.BuildsRefreshed += null, null, new BuildsRefreshedEventArgs(null, null, null));
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(build1));
            provider.Raise((p) => p.BuildsRefreshed += null, null, new BuildsRefreshedEventArgs(null, null, null));

            Assert.AreEqual(2, userFoundCalledCount);
            Assert.AreEqual(2, userTriggeredBuildCount);
            Assert.AreEqual(4, userUpdatedCount);
            Assert.AreEqual(1, userRemovedCount);

            Assert.AreEqual(1, target.Users.Count);
        }

		[Test]
		[Category("Unity")]
		public void GetUserPhoto_UserKindHuman_FromHumanProviders()
		{
			var user1 = new User { UserName = "u1", Kind = UserKind.Human };
			var user2 = new User { UserName = "u2", Kind = UserKind.Human };
			var user3 = new User { UserName = "u3", Kind = UserKind.RetryTrigger };

			int user1PhotoReceivedCount = 0;
			Action<Texture2D> user1PhotoReceived = (photo) =>
			{
				user1PhotoReceivedCount++;
			};

			int user2PhotoReceivedCount = 0;
			Action<Texture2D> user2PhotoReceived = (photo) =>
			{
				user2PhotoReceivedCount++;
			};

			int user3PhotoReceivedCount = 0;
			Action<Texture2D> user3PhotoReceived = (photo) =>
			{
				user3PhotoReceivedCount++;
			};

			var humanUserAvatarProvider1 = MockRepository.GenerateMock<IUserAvatarProvider> ();
			humanUserAvatarProvider1.Expect (h => h.GetUserPhoto (user1, user1PhotoReceived))
				.IgnoreArguments ()
				.WhenCalled (m =>
			{
					var user = m.Arguments[0] as IUser;
					var action = m.Arguments[1] as Action<Texture2D>;

					if(user == user1) 
					{
						action(new Texture2D(1, 1));
					}
					else 
					{
						action(null);
					}
			});
			
		
			var humanUserAvatarProvider2 = MockRepository.GenerateMock<IUserAvatarProvider> ();
			humanUserAvatarProvider2.Expect (h => h.GetUserPhoto (user1, user1PhotoReceived))
				.IgnoreArguments ()
				.WhenCalled (m =>
				{
					var user = m.Arguments[0] as IUser;
					var action = m.Arguments[1] as Action<Texture2D>;

					if(user == user1) 
					{
						action(null);
					}
					else 
					{
						action(new Texture2D(1, 1));
					}
				});

			var nonHumanUserAvatarProvider1 = MockRepository.GenerateMock<IUserAvatarProvider> ();
			nonHumanUserAvatarProvider1.Expect (h => h.GetUserPhoto (user3, user3PhotoReceived))
				.IgnoreArguments()
				.WhenCalled (m =>
				{
					var action = m.Arguments[1] as Action<Texture2D>;
					action(new Texture2D(1, 1));
				});

			var target = new UserService(
				new IUserAvatarProvider[] { humanUserAvatarProvider1, humanUserAvatarProvider2 },
				new IUserAvatarProvider[] { nonHumanUserAvatarProvider1 }, 
				MockRepository.GenerateMock<ISHLogStrategy>());

			target.GetUserPhoto (user1, user1PhotoReceived);
			target.GetUserPhoto (user2, user2PhotoReceived);
			target.GetUserPhoto (user3, user3PhotoReceived);

			Assert.AreEqual (1, user1PhotoReceivedCount);
			Assert.AreEqual (1, user2PhotoReceivedCount);
			Assert.AreEqual (1, user3PhotoReceivedCount);

			humanUserAvatarProvider1.VerifyAllExpectations ();
			humanUserAvatarProvider2.VerifyAllExpectations ();
			nonHumanUserAvatarProvider1.VerifyAllExpectations ();
		}
    }

}