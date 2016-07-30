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
using System.Linq;

public class ModBuilder
{

	[MenuItem ("Buildron/Build mods")]
	static void Build ()
	{
		#if UNITY_STANDALONE_OSX
		var deployRootFolder = "/Users/giacomelli/Dropbox/Skahal/Apps/Buildron/build/Mods/";
		var buildTarget = BuildTarget.StandaloneOSXIntel;
		#else
		var deployRootFolder = @"C:\Dropbox\Skahal\Apps\Buildron\build\Mods";
		var buildTarget = BuildTarget.StandaloneWindows;
		#endif
        
        BuildMods(deployRootFolder, buildTarget);
    }

	private static void BuildMods(string deployRootFolder, BuildTarget buildTarget)
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
		var assetFile = Directory
			.GetFiles (assetsDeployFolder, "*.manifest")
			.FirstOrDefault (f => !f.EndsWith("Assets.manifest"));

        var modName = Path.GetFileNameWithoutExtension(assetFile);
		SHLog.Debug ("Mod name {0}", modName);

		var modDeployFolder = Path.Combine (deployRootFolder, modName);

		if (!Directory.Exists(modDeployFolder))
		{
			Directory.CreateDirectory(modDeployFolder);
		}

		File.Copy(Path.Combine(assetsDeployFolder, modName), Path.Combine(modDeployFolder, modName), true);
	    File.Copy(assetFile, Path.Combine(modDeployFolder, Path.GetFileName(assetFile)), true);
    
		MoveAssemblies(modName, deployRootFolder);

		Directory.Delete(assetsDeployFolder, true);
    }

	static void MoveAssemblies(string modName, string deployRootFolder)
	{
		var referencesFolder = Path.Combine(Application.dataPath, "Scripts");
        referencesFolder = Path.Combine(referencesFolder, "references");

		var modDeployFolder = Path.Combine (deployRootFolder, modName);
		SHLog.Debug("Copy assemblies from folder {0} to {1}", referencesFolder, modDeployFolder);

        var references = Directory.GetFiles(referencesFolder, "*.dll");

        foreach(var r in references)
        {
            File.Copy(r, Path.Combine(modDeployFolder, Path.GetFileName(r)), true);
        }
	}
}
