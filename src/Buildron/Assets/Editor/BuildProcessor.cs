using UnityEngine;
using System.Collections;
using UnityEditor.Callbacks;
using UnityEditor;
using System.IO;
using System;

public class BuildProcessor {

	[PostProcessBuildAttribute(1)]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) 
	{
		CopyModsFilesToBuild (target, pathToBuiltProject);
	}

	private static void CopyModsFilesToBuild(BuildTarget target, string pathToBuiltProject)
	{
		Debug.Log("BuildProcessor.OnPostprocessBuild");

        switch(target)
        {
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
			case BuildTarget.StandaloneLinux:
			case BuildTarget.StandaloneLinux64:
			case BuildTarget.StandaloneLinuxUniversal:
                pathToBuiltProject = Path.GetDirectoryName(pathToBuiltProject);
                break;
        }

		Debug.LogFormat("pathToBuiltProject: {0}", pathToBuiltProject);
		Debug.LogFormat("current directory: {0}", Application.dataPath);

		var srcModsPath = Application.dataPath.Replace ("/Assets", "/mods");
		Debug.LogFormat("srcModsPath: {0}", srcModsPath);

		var destModsPath = pathToBuiltProject + "/mods";
		Debug.LogFormat("destModsPath: {0}", destModsPath);

		Directory.CreateDirectory(destModsPath);
		var files = Directory.GetFiles (srcModsPath, "*.*", SearchOption.AllDirectories);

		foreach (var file in files) {
			var destFileName = destModsPath + file.Replace(srcModsPath, "");
			Debug.LogFormat("Copying file: {0}", destFileName);

			var destDir = Path.GetDirectoryName (destFileName);

			if (!Directory.Exists (destDir)) {
				Directory.CreateDirectory (destDir);
			}

			File.Copy (file, destFileName, true);
		}
	}
}
