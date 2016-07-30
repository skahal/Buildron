using UnityEditor;
using UnityEngine;

public class Emulator
{
	[MenuItem ("Buildron/Create emulator")]
	static void Create ()
	{
		var go = new GameObject ("Emulator");
		go.AddComponent<EmulatorModContext> ();

		var userConfig = new GameObject ("UserConfig");
		userConfig.transform.parent = go.transform;
		userConfig.AddComponent<EmulatorUserConfig> ();
    }
}
