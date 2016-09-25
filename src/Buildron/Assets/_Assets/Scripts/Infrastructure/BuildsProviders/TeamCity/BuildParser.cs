using System.Xml;
using System.Text.RegularExpressions;
using System;
using System.Globalization;
using Buildron.Domain.Builds;

namespace Buildron.Infrastructure.BuildsProvider.TeamCity
{
	/// <summary>
	/// A parster for Build.
	/// </summary>
	public static class BuildParser
	{
		#region Fields
		private static Regex s_getStepNumberRegex = new Regex("Step (\\d+)/\\d+", RegexOptions.Compiled);
		#endregion
		
		/// <summary>
		/// Parse a build from a XmlDocument.
		/// </summary>
		/// <param name='config'>
		/// Config.
		/// </param>
		/// <param name='xmlDoc'>
		/// Xml document.
		/// </param>
		public static Build Parse (BuildConfiguration config, XmlDocument xmlDoc)
		{
			Build build = null;
			var e = xmlDoc.SelectSingleNode ("build");
			
			// Has a build at least?
			if (e != null) {
				build = new Build ();
				build.Id = e.Attributes ["id"].Value;
				build.Configuration = config;
				build.Status = ParseStatus (build, e);
				build.PercentageComplete = ParsePercentageComplete (e);
				build.TriggeredBy = UserParser.ParseFromTriggered (build, xmlDoc);
				build.Date = ParseDate(e);

				var branchProperty = e.SelectSingleNode("//properties/property[@name='Branch']");

				if (branchProperty != null)
				{
					build.Branch.Name = branchProperty.Attributes["value"].Value;
				}
			}
			
			return build;
		}
		
		private static BuildStatus ParseStatus (Build build, XmlNode e)
		{
			var running = e.Attributes ["running"];
			
			if (running != null && running.Value.Equals ("true")) {
				return ParseRunningStates (build, e);
			}
			
			var status = e.Attributes ["status"].Value;
			
			switch (status) {
			case "ERROR":
				return BuildStatus.Error;
				
			case "FAILURE":
				return BuildStatus.Failed;
				
			case "CANCELED":
				return BuildStatus.Canceled;
				
			case "SUCCESS":
				return BuildStatus.Success;
				
			default:
				return BuildStatus.Running;
			}
		}

		private static BuildStatus ParseRunningStates (Build build, XmlNode e)
		{
            var runningInfoNode = e.SelectSingleNode("running-info");
            
            if (runningInfoNode == null)
            {
                return BuildStatus.Running;
            }

            var statusText = runningInfoNode.Attributes ["currentStageText"].Value;
			var status = BuildStatus.Running;
			var m = s_getStepNumberRegex.Match (statusText);
			
			if (m.Success) {
				var stepIndex = Convert.ToInt32 (m.Groups [1].Value) - 1;
				var steps = build.Configuration.Steps;
				
				if (stepIndex < steps.Count) {
					var step = steps [stepIndex];
					build.LastRanStep = step;
					
					switch (step.StepType) {
					case BuildStepType.CodeDuplicationFinder:
						status = BuildStatus.RunningDuplicatesFinder;
						break;
						
					case BuildStepType.CodeAnalysis:
						status = BuildStatus.RunningCodeAnalysis;
						break;
					case BuildStepType.Compilation:
						status = BuildStatus.Running;
						break;
						
					case BuildStepType.Deploy:
						status = BuildStatus.RunningDeploy;
						break;
						
					case BuildStepType.UnitTest:
						status = BuildStatus.RunningUnitTests;
						break;
					}
				}
			}
			
			return status;
		}

		public static float ParsePercentageComplete (XmlNode e)
		{
			float percentageComplete = 0f;
			
			var runningInfo = e.SelectSingleNode ("running-info");
			
			if (runningInfo != null) {
				var percentageCompleteAttr = runningInfo.Attributes ["percentageComplete"];
				
				if (percentageCompleteAttr != null) {
					percentageComplete = float.Parse (percentageCompleteAttr.Value) / 100f;		
				}
			}
			
			return percentageComplete;
		}

		public static DateTime ParseDate (XmlNode e)
		{
			var startDate = e.SelectSingleNode ("startDate");
			var finishDate = e.SelectSingleNode ("finishDate");
			var dateValue = finishDate == null ? startDate.InnerText : finishDate.InnerText;
			
			return DateTime.ParseExact(dateValue, "yyyyMMddTHHmmmsszzz", CultureInfo.InvariantCulture);
		}
	}
}