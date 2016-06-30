using NUnit.Framework;
using Buildron.Infrastructure.BuildsProviders.Filter;
using Rhino.Mocks;
using Buildron.Domain.Builds;
using System.Xml;
using System;
using System.IO;
using Buildron.Infrastructure.BuildsProvider.Jenkins;
using System.Linq;
using Buildron.Domain.CIServers;

namespace Buildron.Infrastructure.FunctionalTests.BuildsProviders.Filter
{
	[Category("Buildron.Infrastructure")]	
	public class JenkinsBuildsProviderTest
    {
		[Test]
		public void BuildTreeFilter_FilteAndDepth_Filter()
        {
            var actual = JenkinsBuildsProvider.BuildTreeFilter("jobs[name,displayName,buildable", 1);
            Assert.AreEqual("jobs[name,displayName,buildable]", actual);

            actual = JenkinsBuildsProvider.BuildTreeFilter("jobs[name,displayName,buildable", 2);
            Assert.AreEqual("jobs[name,displayName,buildable,jobs[name,displayName,buildable]]", actual);

            actual = JenkinsBuildsProvider.BuildTreeFilter("jobs[name,displayName,buildable", 3);
            Assert.AreEqual("jobs[name,displayName,buildable,jobs[name,displayName,buildable,jobs[name,displayName,buildable]]]", actual);
        }
    }
}