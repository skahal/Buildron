using NUnit.Framework;
using Buildron.Infrastructure.BuildsProviders.Filter;
using Rhino.Mocks;
using Buildron.Domain.Builds;
using System.Xml;
using System;
using System.IO;
using Buildron.Infrastructure.BuildsProvider.Jenkins;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Buildron.Infrastructure.FunctionalTests.BuildsProviders.Jenkins
{
	[Category ("Buildron.Infrastructure")]	
	[Category ("Unity")]	
	public class JenkinsBuildConfigurationParserTest
	{
		[Test]
		public void Parse_XmlDocument_BuildConfigurations ()
		{       
			var filename = Path.Combine (UnityEngine.Application.dataPath, @"_Assets/Scripts/Infrastructure.FunctionalTests/Editor/BuildsProviders/Jenkins/JenkinsBuildConfigurationParser.test.file.1.xml");
			var doc = new XmlDocument ();
			doc.Load (filename);

			var actual = JenkinsBuildConfigurationParser.Parse (doc).Keys.ToList ();
			Assert.AreEqual (22, actual.Count);

			var actualConfig = actual.FirstOrDefault (f => f.Id.Equals ("jenkins_2.0"));
			Assert.IsNotNull (actualConfig);
			Assert.AreEqual ("jenkins_2.0", actualConfig.Name);
			Assert.AreEqual ("Core", actualConfig.Project.Name);

			Assert.AreEqual (22, actual.Count
            (c =>
                    !String.IsNullOrEmpty (c.Id)
			&& !String.IsNullOrEmpty (c.Name)
			&& !String.IsNullOrEmpty (c.Project.Name)
			));
		}
	}
}