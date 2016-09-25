using System;
using System.IO;
using System.Xml;
using Buildron.Domain.Builds;
using Buildron.Infrastructure.BuildsProvider.TeamCity;
using NUnit.Framework;

namespace Buildron.Infrastructure.FunctionalTests.BuildsProviders.TeamCity
{
	[Category("Buildron.Infrastructure")]
	[Category("Unity")]
	public class BuildParserTest
	{
		[Test]
		public void Parse_XmlDocument_Build()
		{
			var filename = Path.Combine(UnityEngine.Application.dataPath, @"_Assets/Scripts/Infrastructure.FunctionalTests/Editor/BuildsProviders/TeamCity/BuildParser.test.file.1.xml");
			var doc = new XmlDocument();
			doc.Load(filename);

			var actual = BuildParser.Parse(null, doc);
			Assert.AreEqual("20438", actual.Id);
			Assert.AreEqual(BuildStatus.Success, actual.Status);
			Assert.AreEqual("sprint-01", actual.Branch.Name);
		}
	}
}

