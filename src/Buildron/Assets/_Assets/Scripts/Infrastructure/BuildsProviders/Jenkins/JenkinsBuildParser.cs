#region Usings
using Buildron.Domain;
using System.Xml;
using System.Text.RegularExpressions;
using System;
using System.Globalization;
#endregion

namespace Buildron.Infrastructure.BuildsProvider.Jenkins
{
	/// <summary>
	/// A parser for Build.
	/// </summary>
	public static class JenkinsBuildParser
	{		
		/// <summary>
		/// Parse a build from a XmlDocument.
		/// </summary>
		/// <param name='config'>
		/// Config.
		/// </param>
		/// <param name='xmlDoc'>
		/// Xml document.
		/// </param>
		public static Build Parse (BuildConfiguration config, XmlDocument xmlDoc, string buildTimestamp)
		{
			var build = new Build ();
			build.Configuration = config;
			build.Id = xmlDoc.SelectSingleNode ("//id").InnerText;
			build.Sequence = Convert.ToInt32 (xmlDoc.SelectSingleNode ("//number").InnerText);
			build.LastChangeDescription = xmlDoc.SelectSingleNode ("//action/cause/shortDescription").InnerText;
			build.TriggeredBy = JenkinsUserParser.ParseUserFromBuildResponse (xmlDoc);
			build.Status = ParseStatus (xmlDoc);
			build.Date = ParseDate (buildTimestamp);
			build.PercentageComplete = ParsePercentageComplete(build, xmlDoc);
			return build;
		}
		
		private static BuildStatus ParseStatus (XmlDocument xmlDoc)
		{
			var inQueue = xmlDoc.SelectSingleNode ("//inQueue");
			
			if (inQueue != null && inQueue.InnerText.Equals ("true", StringComparison.OrdinalIgnoreCase)) {
				return BuildStatus.Queued;
			}
			
			if (xmlDoc.SelectSingleNode ("//building").InnerText.Equals ("true", StringComparison.OrdinalIgnoreCase)) {
				return BuildStatus.Running;
			}
			
			var statusText = xmlDoc.SelectSingleNode ("//result").InnerText.ToUpperInvariant ();
			
			switch (statusText) {
			case "SUCCESS":
				return BuildStatus.Success;
				
			case "ABORTED":
				return BuildStatus.Canceled;
				
			default:
				return BuildStatus.Error;
			}
		}

		public static DateTime ParseDate(string buildTimestamp)
		{
			return DateTime.ParseExact(buildTimestamp.Replace("%20", " "), "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
		}

		public static float ParsePercentageComplete (Build build, XmlDocument xmlDoc)
		{
			var estimatedDuration = xmlDoc.SelectSingleNode ("//estimatedDuration");
			
			if (estimatedDuration != null && build.Status >= BuildStatus.Running) {
				var estimatedDurationMilliseconds = double.Parse (estimatedDuration.InnerText);
				var estimatedEnd = build.Date.AddMilliseconds (estimatedDurationMilliseconds);
				var diffMilliseconds = (estimatedEnd - DateTime.Now).TotalMilliseconds;
			
				return 1f - Math.Abs (Convert.ToSingle (estimatedDurationMilliseconds / diffMilliseconds));	
			}
			
			return 0;
		}
	}
}