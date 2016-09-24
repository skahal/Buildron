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
	public class JenkinsBuildParserTest
	{
		[Test]
		public void Parse_XmlDocument_BuildConfigurations ()
		{       
			var filename = Path.Combine (UnityEngine.Application.dataPath, @"_Assets/Scripts/Infrastructure.FunctionalTests/Editor/BuildsProviders/Jenkins/JenkinsBuildParser.test.file.1.xml");
			var doc = new XmlDocument ();
			doc.Load (filename);

			var bc = new BuildConfiguration();
			var actual = JenkinsBuildParser.Parse (bc, doc, "2016/06/14 06:31:09");
			Assert.AreEqual ("10", actual.Id);
			Assert.AreEqual (10, actual.Sequence);
			Assert.AreEqual ("Iniciado por uma mudança no SCM", actual.LastChangeDescription);
			Assert.IsNotNull (actual.TriggeredBy);
			Assert.AreEqual (BuildStatus.Success, actual.Status);
			Assert.AreSame (bc, actual.Configuration);
			Assert.AreEqual (new DateTime(2016, 6, 14, 6, 31, 9), actual.Date);
			Assert.AreEqual (0f, actual.PercentageComplete);
		}
	}
}