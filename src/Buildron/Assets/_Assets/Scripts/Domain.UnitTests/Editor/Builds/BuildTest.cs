using System;
using Buildron.Domain;
using NUnit.Framework;
using Rhino.Mocks;
using Buildron.Domain.Builds;
using Buildron.Domain.Users;

namespace Buildron.Domain.UnitTests.Builds
{
    [Category("Buildron.Domain")]
    public class BuildTest
    {
        [TearDown]
        public void TearDonw()
        {
            Build.EventInterceptors.Clear();
        }

        [Test]
        public void Status_Changed_StatusChangedRaised()
        {
            var target = new Build();
            var statusChangedRaised = target.CreateAssert<BuildStatusChangedEventArgs>("StatusChanged", 2);

            Assert.AreEqual(BuildStatus.Unknown, target.Status);
            Assert.AreEqual(BuildStatus.Unknown, target.PreviousStatus);

            target.Status = BuildStatus.Running;
            Assert.AreEqual(BuildStatus.Running, target.Status);
            Assert.AreEqual(BuildStatus.Unknown, target.PreviousStatus);

            target.Status = BuildStatus.Success;
            Assert.AreEqual(BuildStatus.Success, target.Status);
            Assert.AreEqual(BuildStatus.Running, target.PreviousStatus);

            statusChangedRaised.Assert();
        }

        [Test]
        public void Status_ChangedInterceptorCancelEvent_StatusChangedNotRaised()
        {
            var target = new Build();
            var statusChangedRaised = target.CreateAssert<BuildStatusChangedEventArgs>("StatusChanged", 4);

            var interceptor = MockRepository.GenerateMock<IBuildEventInterceptor>();
            interceptor.Expect(i => i.OnStatusChanged(null)).IgnoreArguments().WhenCalled((m) =>
            {
                var buildEvent = m.Arguments[0] as BuildEvent;

                buildEvent.Canceled = buildEvent.Build.Status == BuildStatus.Error;

            });

            Build.EventInterceptors.Add(interceptor);

            target.Status = BuildStatus.Running;                       
            target.Status = BuildStatus.Success;
            target.Status = BuildStatus.Error;
            target.Status = BuildStatus.Success;
            target.Status = BuildStatus.Error;
            target.Status = BuildStatus.Failed;

            statusChangedRaised.Assert();
        }

        [Test]
        public void IsProperties_DiffStatus_DiffIsResult()
        {
            var target = new Build();
            Assert.IsFalse(target.IsFailed);
            Assert.IsFalse(target.IsQueued);
            Assert.IsFalse(target.IsRunning);
            Assert.IsFalse(target.IsSuccess);

            target.Status = BuildStatus.Canceled;
            Assert.IsTrue(target.IsFailed);
            Assert.IsFalse(target.IsQueued);
            Assert.IsFalse(target.IsRunning);
            Assert.IsFalse(target.IsSuccess);

            target.Status = BuildStatus.Error;
            Assert.IsTrue(target.IsFailed);
            Assert.IsFalse(target.IsQueued);
            Assert.IsFalse(target.IsRunning);
            Assert.IsFalse(target.IsSuccess);

            target.Status = BuildStatus.Failed;
            Assert.IsTrue(target.IsFailed);
            Assert.IsFalse(target.IsQueued);
            Assert.IsFalse(target.IsRunning);
            Assert.IsFalse(target.IsSuccess);

            target.Status = BuildStatus.Queued;
            Assert.IsFalse(target.IsFailed);
            Assert.IsTrue(target.IsQueued);
            Assert.IsFalse(target.IsRunning);
            Assert.IsFalse(target.IsSuccess);

            target.Status = BuildStatus.Running;
            Assert.IsFalse(target.IsFailed);
            Assert.IsFalse(target.IsQueued);
            Assert.IsTrue(target.IsRunning);
            Assert.IsFalse(target.IsSuccess);

            target.Status = BuildStatus.RunningCodeAnalysis;
            Assert.IsFalse(target.IsFailed);
            Assert.IsFalse(target.IsQueued);
            Assert.IsTrue(target.IsRunning);
            Assert.IsFalse(target.IsSuccess);

            target.Status = BuildStatus.RunningDeploy;
            Assert.IsFalse(target.IsFailed);
            Assert.IsFalse(target.IsQueued);
            Assert.IsTrue(target.IsRunning);
            Assert.IsFalse(target.IsSuccess);

            target.Status = BuildStatus.RunningDuplicatesFinder;
            Assert.IsFalse(target.IsFailed);
            Assert.IsFalse(target.IsQueued);
            Assert.IsTrue(target.IsRunning);
            Assert.IsFalse(target.IsSuccess);

            target.Status = BuildStatus.RunningFunctionalTests;
            Assert.IsFalse(target.IsFailed);
            Assert.IsFalse(target.IsQueued);
            Assert.IsTrue(target.IsRunning);
            Assert.IsFalse(target.IsSuccess);

            target.Status = BuildStatus.RunningUnitTests;
            Assert.IsFalse(target.IsFailed);
            Assert.IsFalse(target.IsQueued);
            Assert.IsTrue(target.IsRunning);
            Assert.IsFalse(target.IsSuccess);

            target.Status = BuildStatus.Success;
            Assert.IsFalse(target.IsFailed);
            Assert.IsFalse(target.IsQueued);
            Assert.IsFalse(target.IsRunning);
            Assert.IsTrue(target.IsSuccess);
        }

        [Test]
        public void TriggeredBy_Changed_TriggeredByChangedRaised()
        {
            var target = new Build();
            var changedRaised = target.CreateAssert<BuildTriggeredByChangedEventArgs>("TriggeredByChanged", 3);

            Assert.IsNull(target.TriggeredBy);

            target.TriggeredBy = new User { UserName = "u1" };
            target.TriggeredBy = new User { UserName = "u1" };
            target.TriggeredBy = new User { UserName = "u2" };
            target.TriggeredBy = new User { UserName = "u2" };
            target.TriggeredBy = new User { UserName = "u1" };

            changedRaised.Assert();
        }

        [Test]
        public void TriggeredBy_ChangedInterceptorCancelEvent_TriggeredByChangedNotRaised()
        {
            var target = new Build();
            var changedRaised = target.CreateAssert<BuildTriggeredByChangedEventArgs>("TriggeredByChanged", 4);

            var interceptor = MockRepository.GenerateMock<IBuildEventInterceptor>();
            interceptor.Expect(i => i.OnTriggeredByChanged(null)).IgnoreArguments().WhenCalled((m) =>
            {
                var buildEvent = m.Arguments[0] as BuildEvent;

                buildEvent.Canceled = buildEvent.Build.TriggeredBy.UserName.Equals("u3");

            });

            Build.EventInterceptors.Add(interceptor);

            target.TriggeredBy = new User { UserName = "u1" };
            target.TriggeredBy = new User { UserName = "u1" };
            target.TriggeredBy = new User { UserName = "u2" };
            target.TriggeredBy = new User { UserName = "u3" };
            target.TriggeredBy = new User { UserName = "u2" };
            target.TriggeredBy = new User { UserName = "u1" };
            target.TriggeredBy = new User { UserName = "u3" };

            changedRaised.Assert();
        }

        [Test]
        public void ToString_ProjectAndConfig_String()
        {
            var target = new Build();
            Assert.AreEqual(" - ", target.ToString());

            target.Configuration.Project.Name = "Project";
            target.Configuration.Name = "Config";

            Assert.AreEqual("Project - Config", target.ToString());
        }

        [Test]
        public void CompareTo_Builds_Sort()
        {
            var build1 = new Build { Configuration = new BuildConfiguration { Name = "C1", Project = new BuildProject { Name = "P1" } } };
            Build build2 = null;
            var build3 = new Build { Configuration = new BuildConfiguration { Name = "C0", Project = new BuildProject { Name = "P1" } } };

            var builds = new Build[] { build1, build2, build3 };
            Array.Sort(builds);

            Assert.AreEqual(build2, builds[0]);
            Assert.AreEqual(build3, builds[1]);
            Assert.AreEqual(build1, builds[2]);
        }

        [Test]
        public void Clone_NoArgs_NewInstance()
        {
            var target = new Build();
            target.Configuration.Name = "C1";
            target.Configuration.Project.Name = "P1";

            var actual = target.Clone() as Build;
            Assert.AreNotSame(target, actual);
            Assert.AreEqual("C1", actual.Configuration.Name);
            Assert.AreEqual("P1", actual.Configuration.Project.Name);
        }
    }

}