using System;
using Buildron.Domain;
using NUnit.Framework;
using Rhino.Mocks;
using Skahal.Logging;
using Buildron.Domain.Builds;

namespace Buildron.Domain.UnitTests.Builds
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

        [Test]
        public void RunBuild_NoBuildWithId_LogWarning()
        {
            var log = MockRepository.GenerateMock<ISHLogStrategy>();
            log.Expect(l => l.Warning("No build with id '{0}' could be found to execute the command.", "b1"));

            var provider = MockRepository.GenerateMock<IBuildsProvider>();
            var target = new BuildService(log, MockRepository.GenerateMock<ICIServerService>());
            target.Initialize(provider);
            target.RunBuild(new RemoteControl(), "b1");

            provider.VerifyAllExpectations();
            log.VerifyAllExpectations();
        }

        [Test]
        public void RunBuild_BuildWithId_BuildQueued()
        {
            var provider = MockRepository.GenerateMock<IBuildsProvider>();
            var target = new BuildService(MockRepository.GenerateMock<ISHLogStrategy>(), MockRepository.GenerateMock<ICIServerService>());
            target.Initialize(provider);

            var user = new RemoteControl { UserName = "u1" };
            var build = new Build { Id = "b1", Configuration = new BuildConfiguration { Id = "BC1", Project = new BuildProject { Name = "P1" } } };
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(build));
            provider.Raise((p) => p.BuildsRefreshed += null, null, EventArgs.Empty);

            provider.Expect(p => p.RunBuild(user, build));
            target.RunBuild(user, "b1");

            provider.VerifyAllExpectations();
        }

        [Test]
        public void StopBuild_NoBuildWithId_LogWarning()
        {
            var log = MockRepository.GenerateMock<ISHLogStrategy>();
            log.Expect(l => l.Warning("No build with id '{0}' could be found to execute the command.", "b1"));

            var provider = MockRepository.GenerateMock<IBuildsProvider>();
            var target = new BuildService(log, MockRepository.GenerateMock<ICIServerService>());
            target.Initialize(provider);
            target.StopBuild(new RemoteControl(), "b1");

            provider.VerifyAllExpectations();
            log.VerifyAllExpectations();
        }

        [Test]
        public void StopBuild_BuildWithId_BuildQueued()
        {
            var provider = MockRepository.GenerateMock<IBuildsProvider>();
            var target = new BuildService(MockRepository.GenerateMock<ISHLogStrategy>(), MockRepository.GenerateMock<ICIServerService>());
            target.Initialize(provider);

            var user = new RemoteControl { UserName = "u1" };
            var build = new Build { Id = "b1", Configuration = new BuildConfiguration { Id = "BC1", Project = new BuildProject { Name = "P1" } } };
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(build));
            provider.Raise((p) => p.BuildsRefreshed += null, null, EventArgs.Empty);

            provider.Expect(p => p.StopBuild(user, build));
            target.StopBuild(user, "b1");

            provider.VerifyAllExpectations();
        }

        [Test]
        public void GetMostRelevantBuildForUser_DiffStatus_DiffResults()
        {
            var provider = MockRepository.GenerateMock<IBuildsProvider>();
            var target = new BuildService(MockRepository.GenerateMock<ISHLogStrategy>(), MockRepository.GenerateMock<ICIServerService>());
            target.Initialize(provider);

            var user = new User { UserName = "u1" };
            var runningBuild = new Build { Id = "b1", Status = BuildStatus.Running, Configuration = new BuildConfiguration { Id = "BC1", Project = new BuildProject { Name = "P1" } }, TriggeredBy = user };
            var failedBuild = new Build { Id = "b2", Status = BuildStatus.Failed, Configuration = new BuildConfiguration { Id = "BC2", Project = new BuildProject { Name = "P1" } }, TriggeredBy = user };
            var queuedBuild = new Build { Id = "b3", Status = BuildStatus.Queued, Configuration = new BuildConfiguration { Id = "BC3", Project = new BuildProject { Name = "P1" } }, TriggeredBy = user };
            var successBuild = new Build { Id = "b4", Status = BuildStatus.Success, Configuration = new BuildConfiguration { Id = "BC4", Project = new BuildProject { Name = "P1" } }, TriggeredBy = user };

            // There is a running build.            
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(failedBuild));            
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(successBuild));
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(queuedBuild));
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(runningBuild));
            provider.Raise((p) => p.BuildsRefreshed += null, null, EventArgs.Empty);
            var actual = target.GetMostRelevantBuildForUser(user);
            Assert.AreEqual(runningBuild.Id, actual.Id);

            // There is a queued build.
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(failedBuild));
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(successBuild));
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(queuedBuild));            
            provider.Raise((p) => p.BuildsRefreshed += null, null, EventArgs.Empty);

            actual = target.GetMostRelevantBuildForUser(user);
            Assert.AreEqual(queuedBuild.Id, actual.Id);

            // There is a failed build.            
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(failedBuild));
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(successBuild));         
            provider.Raise((p) => p.BuildsRefreshed += null, null, EventArgs.Empty);

            actual = target.GetMostRelevantBuildForUser(user);
            Assert.AreEqual(failedBuild.Id, actual.Id);

            // There is a success build.
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(successBuild));
            provider.Raise((p) => p.BuildsRefreshed += null, null, EventArgs.Empty);

            actual = target.GetMostRelevantBuildForUser(user);
            Assert.AreEqual(successBuild.Id, actual.Id);

            // There is no user build.
            successBuild.TriggeredBy = new User() { UserName = "u2" };
            provider.Raise((p) => p.BuildUpdated += null, null, new BuildUpdatedEventArgs(successBuild));
            provider.Raise((p) => p.BuildsRefreshed += null, null, EventArgs.Empty);

            actual = target.GetMostRelevantBuildForUser(user);
            Assert.IsNull(actual);

            provider.VerifyAllExpectations();
        }
    }
}