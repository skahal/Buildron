using Buildron.Domain;
using NUnit.Framework;
using Rhino.Mocks;
using Skahal.Logging;

namespace Buildron.Infrastructure.FunctionalTests.Repositories
{
    [Category("Buildron.Domain")]
    public class UserServiceTest
    {
        [Test]
        public void Constructor_BuildsProviderRaiseEvents_UserEventsRaised()
        {
            var provider = MockRepository.GenerateMock<IBuildsProvider>();
            var target = new UserService(null, null, MockRepository.GenerateMock<ISHLogStrategy>());
			target.ListenBuildsProvider (provider);

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
            Assert.AreEqual(1, userRemovedCount);

            provider.Raise((p) => p.BuildsRefreshed += null, null, new BuildsRefreshedEventArgs(null, null, null));
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(build1));
            provider.Raise((p) => p.BuildsRefreshed += null, null, new BuildsRefreshedEventArgs(null, null, null));

            Assert.AreEqual(2, userFoundCalledCount);
            Assert.AreEqual(2, userTriggeredBuildCount);
            Assert.AreEqual(4, userUpdatedCount);
            Assert.AreEqual(1, userRemovedCount);

            Assert.AreEqual(1, target.Users.Count);
        }
    }

}