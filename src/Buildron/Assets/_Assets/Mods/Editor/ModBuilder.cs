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

	[MenuItem ("Buildron/Build mods2")]
	static void Build ()
	{
		#if UNITY_STANDALONE_OSX
		var modsSourceFolder = "/Users/giacomelli/Dropbox/Skahal/Apps/Buildron/src/Buildron/Assets/_Assets/Mods";
		var deployRootFolder = "/Users/giacomelli/Dropbox/Skahal/Apps/Buildron/build/Mods/";
		var buildTarget = BuildTarget.StandaloneOSXIntel;
		#else
		var modsSourceFolder = @"C:\Dropbox\Skahal\Apps\Buildron\src\Buildron\Assets\_Assets\Mods\";
		var deployRootFolder = @"C:\Dropbox\Skahal\Apps\Buildron\build\Mods";
		var buildTarget = BuildTarget.StandaloneWindows;
		#endif
        
        BuildMods(modsSourceFolder, deployRootFolder, buildTarget);

    }

	static void BuildMods (string modsSourceFolder, string deployRootFolder, BuildTarget buildTarget)
    {
        MoveAssemblies(modsSourceFolder, deployRootFolder);
        BuildAssetBundles(deployRootFolder, buildTarget);
    }   

	static void MoveAssemblies(string modsSourceFolder, string deployRootFolder)
	{
		var modsFolders = Directory.GetDirectories (modsSourceFolder);
		SHLog.Debug ("{0} mods folders found at {1}", modsFolders.Length, modsSourceFolder);

		foreach (var modFolder in modsFolders) {
			var modFolderName = Path.GetFileName (modFolder);

			if (modFolderName.Equals ("Editor")) {
				continue;
			}

			var modDeployFolder = Path.Combine (deployRootFolder, modFolderName);
			if (!Directory.Exists (modDeployFolder)) {
				Directory.CreateDirectory (modDeployFolder);
			}

			var fromAssembly = Path.Combine(modFolder, "{0}.dll".With(modFolderName));

			if (File.Exists (fromAssembly)) {
				var toAssembly = Path.Combine (modDeployFolder, "{0}.dll".With (modFolderName));
				File.Copy (fromAssembly, toAssembly);
			}
		}
	}

    private static void BuildAssetBundles(string deployRootFolder, BuildTarget buildTarget)
    {
        var assetsDeployFolder = Path.Combine(deployRootFolder, "Assets");

        if (!Directory.Exists(assetsDeployFolder))
        {
            Directory.CreateDirectory(assetsDeployFolder);
        }

        BuildPipeline.BuildAssetBundles(assetsDeployFolder, BuildAssetBundleOptions.None, buildTarget);

        MoveAssetsToModsFolders(deployRootFolder, assetsDeployFolder);
    }

    static void MoveAssetsToModsFolders(string deployRootFolder, string assetsDeployFolder)
    {
        SHLog.Debug("Moving assets to mod folder");
        var assetFiles = Directory.GetFiles(assetsDeployFolder, "*.manifest");

        foreach (var assetFile in assetFiles)
        {
            var assetName = Path.GetFileNameWithoutExtension(assetFile);
            var modDeployFolder = Path.Combine(deployRootFolder, assetName);
            File.Move(Path.Combine(assetsDeployFolder, assetName), Path.Combine(modDeployFolder, assetName));

            File.Move(assetFile, Path.Combine(modDeployFolder, Path.GetFileName(assetFile)));
        }

        Directory.Delete(assetsDeployFolder, true);
    }
}
