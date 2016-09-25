using System.Xml;
using Buildron.Domain.Builds;

namespace Buildron.Infrastructure.BuildsProvider.TeamCity
{
	/// <summary>
	/// A parser for BuildConfiguration.
	/// </summary>
	public static class BuildConfigurationParser
	{
		/// <summary>
		/// Parses a BuildConfiguration from a XmlNode.
		/// </summary>
		/// <param name='xmlNode'>
		/// Xml node.
		/// </param>
		public static BuildConfiguration Parse (XmlNode xmlNode)
		{
			var buildTypeNode = xmlNode.SelectSingleNode ("buildType");
			var bc = new BuildConfiguration ();
			bc.Id = buildTypeNode.Attributes ["id"].Value;
			bc.Name = buildTypeNode.Attributes ["name"].Value;
			bc.Project.Name = buildTypeNode.SelectSingleNode ("project").Attributes ["name"].Value;
			bc.Steps = BuildStepParser.Parse (xmlNode);


			return bc;
		}
	}
}