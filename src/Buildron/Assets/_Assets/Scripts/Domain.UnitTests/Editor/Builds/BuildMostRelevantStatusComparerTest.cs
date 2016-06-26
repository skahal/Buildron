using System;
using Buildron.Domain;
using NUnit.Framework;
using Buildron.Domain.Builds;

namespace Buildron.Domain.UnitTests.Builds
{
    [Category("Buildron.Domain")]
    public class BuildMostRelevantStatusComparerTest
    {
     
        [Test]
        public void Compare_Builds_Order()
        {
            var target = new BuildMostRelevantStatusComparer();            
            var runningBuild = new Build { Id = "b1", Status = BuildStatus.Running, Configuration = new BuildConfiguration { Id = "BC1", Project = new BuildProject { Name = "P1" } }};
            var failedBuild = new Build { Id = "b2", Status = BuildStatus.Failed, Configuration = new BuildConfiguration { Id = "BC2", Project = new BuildProject { Name = "P1" } } };
            var queuedBuild = new Build { Id = "b3", Status = BuildStatus.Queued, Configuration = new BuildConfiguration { Id = "BC3", Project = new BuildProject { Name = "P1" } } };
            var successBuild = new Build { Id = "b4", Status = BuildStatus.Success, Configuration = new BuildConfiguration { Id = "BC4", Project = new BuildProject { Name = "P1" } } };

            var builds = new Build[] { runningBuild, failedBuild, queuedBuild, successBuild };
            Array.Sort(builds, target);
            Assert.AreEqual(runningBuild, builds[0]);
            Assert.AreEqual(queuedBuild, builds[1]);
            Assert.AreEqual(failedBuild, builds[2]);            
            Assert.AreEqual(successBuild, builds[3]);
        }

        [Test]
        public void ToString_NoArgs_Name()
        {
            Assert.AreEqual("status", new BuildMostRelevantStatusComparer().ToString());
        }
    }
}