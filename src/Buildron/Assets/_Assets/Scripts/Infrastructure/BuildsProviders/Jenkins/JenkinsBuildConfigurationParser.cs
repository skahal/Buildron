using System;
using System.Collections.Generic;
using System.Xml;
using Buildron.Domain.Builds;

namespace Buildron.Infrastructure.BuildsProvider.Jenkins
{
	/// <summary>
	/// A parser for Build Configuration.
	/// </summary>
	public class JenkinsBuildConfigurationParser
	{
        public static Dictionary<BuildConfiguration, XmlNode> Parse(XmlDocument doc)
        {
            var result = new Dictionary<BuildConfiguration, XmlNode>();
            var configNodes = doc.SelectNodes("//job[buildable='true']");

            foreach(XmlNode configNode in configNodes)
            {
                var bc = new BuildConfiguration();
                bc.Id = configNode["name"].InnerText;
                bc.Name = configNode["displayName"].InnerText;
                bc.Project = new BuildProject();

                var parentNode= configNode.ParentNode;
                bc.Project.Name = parentNode.Name.Equals("job", StringComparison.OrdinalIgnoreCase) ? parentNode["displayName"].InnerText : bc.Name;

                result.Add(bc, configNode);
            }

            return result;
        }	
	}
}