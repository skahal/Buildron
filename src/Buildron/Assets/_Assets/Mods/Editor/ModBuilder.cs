using UnityEditor;
using System.Collections;
using Buildron.Domain.Mods;
using UnityEngine;
using System.Diagnostics;
using System.Text;
using System.IO;
using Skahal.Debugging;
using Skahal.Logging;
using System;

public class ModBuilder
{

	[MenuItem ("Buildron/Build mods")]
	static void Build ()
	{
		var deployRootFolder = ModLoader.RootFolder;
		BuildOsxMods ("/Users/giacomelli/Dropbox/Skahal/Apps/Buildron/src/Buildron/Assets/_Assets/Mods", deployRootFolder);
		//BuildWinMods ();
    }

	static void BuildOsxMods (string modsSourceFolder, string deployRootFolder)
	{
		CompileMods (modsSourceFolder, deployRootFolder);

		var assetsDeployFolder = Path.Combine (deployRootFolder, "Assets");

		if (!Directory.Exists (assetsDeployFolder)) {
			Directory.CreateDirectory (assetsDeployFolder);
		}

		BuildPipeline.BuildAssetBundles (assetsDeployFolder, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSXUniversal);
		MoveAssetsToModsFolders (deployRootFolder, assetsDeployFolder);
	}

	static void BuildWinMods ()
	{
		BuildPipeline.BuildAssetBundles (@"..\..\Build\Mods", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
	}

	static void MoveAssetsToModsFolders(string deployRootFolder, string assetsDeployFolder)
	{
		SHLog.Debug ("Moving assets to mod folder");
		var assetFiles = Directory.GetFiles (assetsDeployFolder, "*.manifest");
		 
		foreach (var assetFile in assetFiles) {
			var assetName = Path.GetFileNameWithoutExtension (assetFile);
			var modDeployFolder = Path.Combine (deployRootFolder, assetName);
			File.Move (Path.Combine (assetsDeployFolder, assetName), Path.Combine (modDeployFolder, assetName));

			File.Move (assetFile, Path.Combine (modDeployFolder, Path.GetFileName(assetFile)));
		}

		Directory.Delete (assetsDeployFolder);
	}

	static void CompileMods(string modsSourceFolder, string deployRootFolder)
	{
		var modsFolders = System.IO.Directory.GetDirectories (modsSourceFolder);
		SHLog.Debug ("{0} mods folders found at {1}", modsFolders.Length, modsSourceFolder);

		foreach (var modFolder in modsFolders) {
			var modFolderName = System.IO.Path.GetFileName (modFolder);

			if (modFolderName.Equals ("Editor")) {
				continue;
			}

			var modDeployFolder = Path.Combine (deployRootFolder, modFolderName);
			if (!Directory.Exists (modDeployFolder)) {
				Directory.CreateDirectory (modDeployFolder);
			}

			SHLog.Debug ("Compiling mod {0}...", modFolderName);
			var arguments = new StringBuilder ();
			arguments.AppendLine ("cd {0}".With(modFolder));
			arguments.Append ("mcs");
			arguments.Append (" -reference:\"/Applications/Unity/Unity.app/Contents/Frameworks/Managed/UnityEngine.dll\"");
			arguments.Append (" -reference:\"/Applications/Unity/Unity.app/Contents/UnityExtensions/Unity/GUISystem/UnityEngine.UI.dll\"");
			arguments.Append (" -reference:\"../../References/Buildron.ModSdk.dll\"");
			arguments.Append (" -reference:\"../../References/Skahal.Unity.Scripts.dll\"");
			arguments.Append (" -reference:\"../../References/Skahal.Unity.Scripts.Externals.dll\"");
			arguments.Append (" -target:library");
			arguments.AppendFormat (" -out:{0}.dll", Path.Combine (modDeployFolder, modFolderName));
			arguments.Append (" -sdk:2");

			var csFiles = Directory.GetFiles (modFolder, "*.cs");

			foreach (var csFile in csFiles) {
				arguments.AppendFormat (" {0}", Path.GetFileName (csFile));
			}

			var args = arguments.ToString ();
			SHLog.Debug ("Executing mcs{0}", args);

			var compileModPath = Path.Combine (modFolder, "mod.compile.sh");
			File.WriteAllText (compileModPath, args);
			Run ("open", "-b com.apple.terminal {0}".With(compileModPath));
		}
	}
	static string Run(string exePath, string arguments = "", bool waitForExit = true)
	{
		string output = String.Empty;
		using (var p = new Process())
		{
			var startInfo = p.StartInfo;
			startInfo.CreateNoWindow = true;
			startInfo.WindowStyle = ProcessWindowStyle.Hidden;
			startInfo.UseShellExecute = false;

			if (waitForExit)
			{
				startInfo.StandardOutputEncoding = Encoding.GetEncoding("ibm850");
				startInfo.RedirectStandardOutput = true;
			}

			// Se inicia com uma variável de sistema.
			if (exePath.StartsWith("%", StringComparison.OrdinalIgnoreCase))
			{
				startInfo.FileName = Environment.ExpandEnvironmentVariables(exePath);
			}
			else
			{
				startInfo.FileName = exePath;
			}

			startInfo.Arguments = arguments;

			p.Start();

			if (waitForExit)
			{
				output = p.StandardOutput.ReadToEnd();
				p.WaitForExit();
			}

			return output;
		}
	}
}
