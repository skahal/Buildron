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
	/// A parser for Build Configuration.
	/// </summary>
	public static class JenkinsBuildConfigurationParser
	{
		public static BuildConfiguration Parse (XmlNode node)
		{
			var bc = new BuildConfiguration ();
			bc.Id = node.SelectSingleNode ("name").InnerText;
			bc.Name = bc.Id;
			bc.Project = new BuildProject ();
			bc.Project.Name = bc.Name;
			
			return bc;
		}
		
		public static void ParsePartial (BuildConfiguration bc, XmlDocument doc)
		{
			var lastBuildNumberNode = doc.SelectSingleNode ("//lastBuild/number");
			
			if (lastBuildNumberNode == null) {
				bc.Name = "0";
			} else {
				bc.Name = doc.SelectSingleNode ("//lastBuild/number").InnerText;
			}	
		
			bc.Name = "#" + bc.Name;
			bc.Project.Name = doc.SelectSingleNode ("//displayName").InnerText;
		
		}
	}
}