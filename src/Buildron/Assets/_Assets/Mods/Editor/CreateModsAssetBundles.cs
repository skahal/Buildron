using UnityEditor;
using System.Collections;

public class CreateModsAssetBundles
{

	[MenuItem ("Buildron/Build mods")]
	static void BuildAllAssetBundles ()
	{
        BuildPipeline.BuildAssetBundles ("../../Build/Mods", BuildAssetBundleOptions.None, BuildTarget.StandaloneOSXUniversal);
       // BuildPipeline.BuildAssetBundles(@"..\..\Build\Mods", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}
