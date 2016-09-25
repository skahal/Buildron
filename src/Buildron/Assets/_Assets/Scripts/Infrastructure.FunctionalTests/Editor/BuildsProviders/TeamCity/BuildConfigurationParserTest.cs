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
	public class BuildConfigurationParserTest
	{
		[Test]
		public void Parse_XmlDocument_BuildConfigurations()
		{
			var filename = Path.Combine(UnityEngine.Application.dataPath, @"_Assets/Scripts/Infrastructure.FunctionalTests/Editor/BuildsProviders/TeamCity/BuildConfigurationParser.test.file.1.xml");
			var doc = new XmlDocument();
			doc.Load(filename);

			var actual = BuildConfigurationParser.Parse(doc);
			Assert.AreEqual("Customer_Project_2UnitTests", actual.Id);
			Assert.AreEqual("#2|Unit tests", actual.Name);
			Assert.AreEqual("Project - Project", actual.Project.Name);
			Assert.AreEqual(1, actual.Steps.Count);
		}
	}
}

