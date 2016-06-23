using System;
using Buildron.Domain;
using NUnit.Framework;
using Rhino.Mocks;
using Skahal.Logging;

namespace Buildron.Domain.UnitTests
{
    [Category("Buildron.Domain")]
    public class BuildServiceTest
    {
        [Test]
        public void Initialize_NewBuildsAndOldBuilds_EventsRaised()
        {
            var provider = MockRepository.GenerateMock<IBuildsProvider>();
            var target = new BuildService(MockRepository.GenerateMock<ISHLogStrategy>(), MockRepository.GenerateMock<ICIServerService>());
            target.Initialize(provider);

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
                TriggeredBy = user1,
            };
            build1.Configuration.Id = "bc1";
            user1.Builds.Add(build1);

            var build2 = new Build
            {
                Id = "2",
                TriggeredBy = user1,
            };
            build2.Configuration.Id = "bc2";

            var buildFoundRaised = target.CreateAssert<BuildFoundEventArgs>("BuildFound", 2);
            var buildUpdatedRaised = target.CreateAssert<BuildUpdatedEventArgs>("BuildUpdated", 5);
            var buildRemovedRaised = target.CreateAssert<BuildRemovedEventArgs>("BuildRemoved", 1);
            var buildsRefreshedRaised = target.CreateAssert<BuildsRefreshedEventArgs>("BuildsRefreshed", 3);

            // User1 and build1.
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(build1));


            provider.Raise((p) => p.BuildsRefreshed += null, null, EventArgs.Empty);
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(build1));

            // User2 and build1.
            build1.TriggeredBy = user2;
            user2.Builds.Add(build1);
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(build1));

            // User1 and build2.
            user1.Builds.Add(build2);
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(build2));

            // Build1 removed.
            provider.Raise((p) => p.BuildsRefreshed += null, null, new BuildsRefreshedEventArgs(null, null, null));
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(build2));
            provider.Raise((p) => p.BuildsRefreshed += null, null, new BuildsRefreshedEventArgs(null, null, null));


            provider.VerifyAllExpectations();
            buildFoundRaised.Assert();
            buildUpdatedRaised.Assert();
            buildRemovedRaised.Assert();
            buildsRefreshedRaised.Assert();
        }

        [Test]
        public void RefreshAllBuilds_NoArgs_BuildsProviderCalled()
        {
            var provider = MockRepository.GenerateMock<IBuildsProvider>();
            var target = new BuildService(MockRepository.GenerateMock<ISHLogStrategy>(), MockRepository.GenerateMock<ICIServerService>());
            target.Initialize(provider);
            provider.Expect(p => p.RefreshAllBuilds());
            target.RefreshAllBuilds();

            provider.VerifyAllExpectations();
        }
    }
}