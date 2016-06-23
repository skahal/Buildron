using System;
using Buidron.Domain;
using Buildron.Domain;
using NUnit.Framework;
using Rhino.Mocks;
using Skahal.Logging;

namespace Buildron.Domain.UnitTests
{
    [Category("Buildron.Domain")]
    public class BuildDateDescendingComparerTest
    {
     
        [Test]
        public void Compare_Builds_Order()
        {
            var target = new BuildDateDescendingComparer();
            var build1 = new Build { Date = DateTime.Now.AddMinutes(-2) };
            var build2 = new Build { Date = DateTime.Now };
            var build3 = new Build { Date = DateTime.Now.AddMinutes(-1) };

            var builds = new Build[] { build1, build2, build3 };
            Array.Sort(builds, target);
            Assert.AreEqual(build2, builds[0]);
            Assert.AreEqual(build3, builds[1]);
            Assert.AreEqual(build1, builds[2]);
        }

        [Test]
        public void ToString_NoArgs_Name()
        {
            Assert.AreEqual("date", new BuildDateDescendingComparer().ToString());
        }
    }
}