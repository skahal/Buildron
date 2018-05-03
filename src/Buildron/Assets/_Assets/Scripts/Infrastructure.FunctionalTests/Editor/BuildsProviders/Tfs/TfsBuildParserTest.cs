using NUnit.Framework;
using System;
using System.IO;
using UnityEngine;
using Buildron.Infrastructure.BuildsProvider.Tfs;
using Buildron.Infrastructure.BuildsProvider.Tfs.Models;
using System.Collections.Generic;
using Buildron.Domain.Builds;

namespace Buildron.Infrastructure.FunctionalTests.BuildsProviders.Tfs
{
    [Category ("Buildron.Infrastructure")]	
	[Category ("Unity")]	
	public class TfsBuildParserTest
	{
        [Test]
        public void Parse_JsonDocument_BuildConfigurations()
        {
            // Arrange/Act
            var tfsDefinitions = ParseJson<TfsDefinitions>("TfsBuildParser.Definitions");

            // Assert
            Assert.That(tfsDefinitions.count, Is.EqualTo(2));
            //Assert.That(tfsDefinitions.value[0], Is.EqualTo(2));
        }

        [Test]
        public void Parse_JsonDocument_BuildConfiguration()
        {
            // Arrange
            var tfsDefinition = ParseJson<TfsDefinition>("TfsBuildParser.Definition");

            // Act
            var buildConfiguration = TfsBuildParser.Parse(tfsDefinition);

            // Assert
            Assert.That(buildConfiguration.Id, Is.EqualTo("2"));
            Assert.That(buildConfiguration.Name, Is.EqualTo("Story Build"));
            Assert.That(buildConfiguration.Project, Is.Not.Null);
            Assert.That(buildConfiguration.Project.Name, Is.EqualTo("MyFirstProject"));
            Assert.That(buildConfiguration.Steps, Is.Not.Null);
            Assert.That(buildConfiguration.Steps.Count, Is.EqualTo(8));
            Assert.That(buildConfiguration.Steps[0].Name, Is.EqualTo("NuGet restore **/*.sln"));
            Assert.That(buildConfiguration.Steps[0].StepType, Is.EqualTo(BuildStepType.None));
            Assert.That(buildConfiguration.Steps[1].Name, Is.EqualTo("Build solution **\\*.sln"));
            Assert.That(buildConfiguration.Steps[1].StepType, Is.EqualTo(BuildStepType.Compilation));
            Assert.That(buildConfiguration.Steps[2].Name, Is.EqualTo("Test Assemblies **\\*test*.dll;-:**\\obj\\**"));
            Assert.That(buildConfiguration.Steps[2].StepType, Is.EqualTo(BuildStepType.UnitTest));
            Assert.That(buildConfiguration.Steps[3].Name, Is.EqualTo("Publish Test Results **/TEST-*.xml"));
            Assert.That(buildConfiguration.Steps[3].StepType, Is.EqualTo(BuildStepType.UnitTest));
            Assert.That(buildConfiguration.Steps[4].Name, Is.EqualTo("NuGet Packager "));
            Assert.That(buildConfiguration.Steps[4].StepType, Is.EqualTo(BuildStepType.PackagePublishing));
            Assert.That(buildConfiguration.Steps[5].Name, Is.EqualTo("NuGet Publisher "));
            Assert.That(buildConfiguration.Steps[5].StepType, Is.EqualTo(BuildStepType.PackagePublishing));
            Assert.That(buildConfiguration.Steps[6].Name, Is.EqualTo("Publish Artifact: Binaries"));
            Assert.That(buildConfiguration.Steps[6].StepType, Is.EqualTo(BuildStepType.PackagePublishing));
            Assert.That(buildConfiguration.Steps[7].Name, Is.EqualTo("Copy files from MyFirstLib/bin/$(Configuration)/*.dll"));
            Assert.That(buildConfiguration.Steps[7].StepType, Is.EqualTo(BuildStepType.None));
        }

        [Test]
		public void Parse_JsonDocument_Build()
        {
            // Arrange
            var tfsBuild = ParseJson<TfsBuild>("TfsBuildParser.Build");
            var buildConfiguration = new Domain.Builds.BuildConfiguration();

            // Act
            var build = TfsBuildParser.Parse(buildConfiguration, tfsBuild, null);

            // Assert
            Assert.That(build.Id, Is.EqualTo("6"));
            Assert.That(build.Sequence, Is.EqualTo(6));
            //Assert.That(build.LastChangeDescription, Is.EqualTo("Refactor"));
            Assert.That(build.TriggeredBy, Is.Not.Null);
            Assert.That(build.TriggeredBy.Name, Is.EqualTo("Grimace of Despair"));
            Assert.That(build.Status, Is.EqualTo(Domain.Builds.BuildStatus.Success));
            Assert.That(build.Configuration, Is.EqualTo(buildConfiguration));
            Assert.That(build.Date, Is.EqualTo(DateTime.Parse("2017-03-24T23:06:17.0751836Z")));
            //Assert.That(build.PercentageComplete, Is.EqualTo(1.0f));
        }

        private static TModel ParseJson<TModel>(string tfsJson)
        {
            var jsonFile = Path.Combine(UnityEngine.Application.dataPath, string.Format(@"_Assets/Scripts/Infrastructure.FunctionalTests/Editor/BuildsProviders/Tfs/{0}.json", tfsJson));
            var json = File.ReadAllText(jsonFile);

            return JsonUtility.FromJson<TModel>(json);
        }
    }
}