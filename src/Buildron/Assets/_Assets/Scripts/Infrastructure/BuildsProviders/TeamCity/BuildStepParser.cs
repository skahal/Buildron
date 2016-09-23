using System.Collections.Generic;
using System.Xml;
using Buildron.Domain.Builds;

namespace Buildron.Infrastructure.BuildsProvider.TeamCity
{
	public static class BuildStepParser
	{
		/// <summary>
		/// Parses the BuildSteps from a XmlNode.
		/// </summary>
		/// <param name='steps'>
		/// Steps.
		/// </param>
		/// <param name='xmlNode'>
		/// Xml node.
		/// </param>
		public static IList<IBuildStep> Parse (XmlNode xmlNode)
		{
			var steps = new List<IBuildStep> ();
			var nodes = xmlNode.SelectNodes ("buildType/steps/step");
	
			if (nodes != null) {
				foreach (XmlNode n in nodes) {
					var s = new BuildStep ();
					s.Name = n.Attributes ["name"].Value;
				
					switch (n.Attributes ["type"].Value.ToUpperInvariant ()) {
					case "VS.SOLUTION":
						s.StepType = BuildStepType.Compilation;
						break;
					
					case "MSTEST":
						s.StepType = BuildStepType.UnitTest;
						break;
					
					case "DOTNET-TOOLS-DUPFINDER":
						s.StepType = BuildStepType.CodeDuplicationFinder;
						break;
					
					case "FXCOP":
						s.StepType = BuildStepType.CodeAnalysis;
						break;
					
					case "MSBUILD":
						s.StepType = BuildStepType.Deploy;
						break;
					
					default:
						if (IsStatisticsStep (s.Name)) {
							s.StepType = BuildStepType.Statistics;
						} else if (IsUnitTestStep (s.Name)) {
							s.StepType = BuildStepType.UnitTest;
						} else {
							//SHLog.Error ("{0}|{1}", s.Name, n.Attributes ["type"].Value.ToUpperInvariant ());
							s.StepType = BuildStepType.None;
						}
						
						break;
					}
					
					steps.Add (s);
				}
			}
			
			return steps;
		}
		
		private static bool IsStatisticsStep (string name)
		{
			return name.ToLowerInvariant().Contains ("estat") || name.Contains ("stats") || name.Contains ("statistics") || name.Contains("log svn");
		}
		
		private static bool IsUnitTestStep(string name)
		{
			return name.ToLowerInvariant().Contains ("testes unit") || name.Contains ("unit test");
		}
	}
}