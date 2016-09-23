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
	public class JenkinsUserParserTest
    {
		[Test]
		public void JenkinsUserParser_XmlDocumentWithChangeset_Username ()
		{       
			var filename = Path.Combine (UnityEngine.Application.dataPath, @"_Assets/Scripts/Infrastructure.FunctionalTests/Editor/BuildsProviders/Jenkins/JenkinsBuildParser.test.file.1.xml");
			var doc = new XmlDocument ();
			doc.Load (filename);

			var actual = JenkinsUserParser.ParseUserFromBuildResponse (doc);
			Assert.AreEqual ("kk", actual.UserName);
		}
    }
}