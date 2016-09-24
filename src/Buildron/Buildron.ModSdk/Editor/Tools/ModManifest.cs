using System;
using System.IO;
using System.Xml;

namespace Buildron.ModSdk.Editor
{
	public class ModManifest
	{
		public string Platform { get; set; }
		public DateTime BuildTime { get; set; }

		public void Save(string modFolder)
		{
			var doc = new XmlDocument();

			var manifestNode = doc.CreateElement("manifest");
			doc.AppendChild(manifestNode);

			var platformNode = doc.CreateElement("platform");
			platformNode.InnerText = Platform;
			manifestNode.AppendChild(platformNode);

			var buildTimeNode = doc.CreateElement("buildTime");
			buildTimeNode.InnerText = BuildTime.ToString("yyyy-MM-dd HH:mm:ss");
			manifestNode.AppendChild(buildTimeNode);

			var filename = Path.Combine(modFolder, "mod.manifest.xml");
			doc.Save(filename);
		}
	}
}