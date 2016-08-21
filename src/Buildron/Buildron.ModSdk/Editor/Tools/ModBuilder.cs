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
using Buildron.ModSdk.Editor;
using System.Xml;

public class ModBuilder : EditorWindow
{
	#region Fields
	private static BuildTarget[] s_availableBuildTargets = new BuildTarget[] { BuildTarget.StandaloneOSXIntel, BuildTarget.StandaloneLinux, BuildTarget.StandaloneWindows };
	private string[] m_plaforms;
	private int m_selectedPlatform;
	private string m_modsFolder;
	private static StringBuilder m_log = new StringBuilder ();
	private Vector2 m_logScroll;
	#endregion

	#region Constructors
	public ModBuilder()
	{
		titleContent.text = "Build mod";
		autoRepaintOnSceneChange = true;
		minSize = new Vector2 (700, 455);
		m_plaforms = s_availableBuildTargets.Select(t => GetPlatform(t)).ToArray();
	}
	#endregion

	#region Build
	private void Build (BuildTarget buildTarget)
	{
		m_log.Remove(0, m_log.Length);

		try 
		{
			Log("Building mods...");
			BuildMod (m_modsFolder, buildTarget);
			Log("Done.");
			ShowStatus("Mod successful built.");
		}
		catch(Exception ex) {
			Log ("Error: {0}", ex.Message);
			Log ("Aborted.");
			ShowStatus("Error building mod: {0}".With(ex.Message));
		}

		Repaint();
	}

	public static void BuildFromCommandLine()
	{
		var args = Environment.GetCommandLineArgs ();
		Log("BuildFromCommandLine: {0} args", args.Length);
		var deployRootFolder = args [args.Length - 2];
		var buildTarget = (BuildTarget) Enum.Parse (typeof(BuildTarget), args [args.Length - 1]);

		Log("\t- deployRootFolder: {0} args", deployRootFolder);
		Log("\t- buildTarget: {0} args", buildTarget);
		BuildMod (deployRootFolder, buildTarget);
	}

	private static void BuildMod(string deployRootFolder, BuildTarget buildTarget)
	{
		var platform = GetPlatform(buildTarget);
		Log("Building for {0}...", platform);

		var assetsDeployFolder = Path.Combine(deployRootFolder, Guid.NewGuid().ToString());

		if (!Directory.Exists(assetsDeployFolder))
		{
			Log("Creating folder {0}...", assetsDeployFolder);
			Directory.CreateDirectory(assetsDeployFolder);
		}

		Log("Building asset bundles...");
		BuildPipeline.BuildAssetBundles(assetsDeployFolder, BuildAssetBundleOptions.None, buildTarget);

		var modDeployFolder = MoveAssetsToModsFolders(deployRootFolder, assetsDeployFolder);
		CreateModManifest(modDeployFolder, platform);
	}

	private static string GetPlatform(BuildTarget buildTarget)
	{
		switch (buildTarget)
		{
			case BuildTarget.StandaloneWindows:
			case BuildTarget.StandaloneWindows64:
				return "Win";

			case BuildTarget.StandaloneOSXIntel:
			case BuildTarget.StandaloneOSXIntel64:
			case BuildTarget.StandaloneOSXUniversal:
				return "Mac";

			case BuildTarget.StandaloneLinux:
			case BuildTarget.StandaloneLinux64:
			case BuildTarget.StandaloneLinuxUniversal:
				return "Linux";
			
			default:
				throw new InvalidOperationException("BuildTarget {0} not supported".With(buildTarget));
		}
	}

	private static string MoveAssetsToModsFolders(string deployRootFolder, string assetsDeployFolder)
	{
		Log("Moving assets to mod folder");
		var folderName = Path.GetFileName (assetsDeployFolder);
		var assetFile = Directory
			.GetFiles (assetsDeployFolder, "*.manifest")
			.FirstOrDefault (f => !f.EndsWith("{0}.manifest".With(folderName), StringComparison.OrdinalIgnoreCase));

		if (assetFile == null) {
			var noAssetFileMsg = @"
No assets manifest file found. Possible reasons:
	* Did you remember to mark your assets with asset bundle with same name of your mod project?
	* The support to selected platform is installed in your Unity Editor?";

			Log(noAssetFileMsg);
			throw new InvalidOperationException ("No assets manifest file found!");
		}

		var modName = Path.GetFileNameWithoutExtension(assetFile);
		Log ("Mod name {0}", modName);

		var modDeployFolder = Path.Combine (deployRootFolder, modName);

		if (!Directory.Exists(modDeployFolder))
		{
			Log ("Creating folder {0}...", modDeployFolder);
			Directory.CreateDirectory(modDeployFolder);
		}

		File.Copy(Path.Combine(assetsDeployFolder, modName), Path.Combine(modDeployFolder, modName), true);
		File.Copy(assetFile, Path.Combine(modDeployFolder, Path.GetFileName(assetFile)), true);

		MoveAssemblies(modName, deployRootFolder);

		Directory.Delete(assetsDeployFolder, true);

		return modDeployFolder;
	}

	private static void MoveAssemblies(string modName, string deployRootFolder)
	{
		var referencesFolder = Path.Combine(Application.dataPath, "Scripts");
		referencesFolder = Path.Combine(referencesFolder, "references");

		var modDeployFolder = Path.Combine (deployRootFolder, modName);
		Log (
			"Copy assemblies from folder {0} to {1}", 
			Path.GetFileName(referencesFolder), 
			Path.GetFileName(modDeployFolder));

		var references = Directory.GetFiles(referencesFolder, "*.dll");

		foreach(var r in references)
		{
			File.Copy(r, Path.Combine(modDeployFolder, Path.GetFileName(r)), true);
		}
	}

	private static void CreateModManifest(string modFolder, string platform)
	{
		Log("Creating mod.manifest.xml...");
		var manifest = new ModManifest();
		manifest.Platform = platform;
		manifest.BuildTime = DateTime.UtcNow;
		manifest.Save(modFolder);
	}
	#endregion

	#region GUI
	[MenuItem ("Buildron/Build mod")]
	private static void Init ()
	{	
		var instance = GetWindow<ModBuilder> ();
		instance.ShowPopup ();
	}

	/// <summary>
	/// Draws the window's GUI.
	/// </summary>
	private void OnGUI ()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Platform: ");
		m_selectedPlatform = GUILayout.SelectionGrid(m_selectedPlatform, m_plaforms,  3, EditorStyles.radioButton);
		GUILayout.EndHorizontal();

		m_modsFolder = EditorGUILayout.TextField ("Mods folder", m_modsFolder);
		CreateHelpBox ("The mods folder used by Buildron");

		if (GUILayout.Button ("Build")) {
			SavePrefs ();
			Build (s_availableBuildTargets[m_selectedPlatform]);		
		}

		m_logScroll = EditorGUILayout.BeginScrollView (m_logScroll, GUILayout.Height (200));  

		var logText = m_log.ToString ();
		var height = GUIStyle.none.CalcHeight (new GUIContent (logText), minSize.x);
		height = height < 100 ? 100 : height;
		GUI.enabled = false;
		EditorGUILayout.TextArea (logText, GUILayout.Height (height));
		GUI.enabled = true; 
		EditorGUILayout.EndScrollView ();
	}

	private void CreateHelpBox (string helpText, string url = null)
	{
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.HelpBox (helpText, MessageType.None, false);

		if (!string.IsNullOrEmpty (url) && GUILayout.Button ("Get", GUILayout.Width(40))) {
			Application.OpenURL (url);
		}

		EditorGUILayout.EndHorizontal ();	
		EditorGUILayout.Separator ();
	}

	private void ShowStatus (string text)
	{
		ShowNotification (new GUIContent (text));
	}

	private void OnLostFocus ()
	{
		SavePrefs ();
	}

	private void OnDestroy ()
	{
		SavePrefs ();
	}
	#endregion

	#region Helpers	
	private string GetKey (string key)
	{
		var fullKey = string.Format ("BuildronModBuilder_{0}_{1}", Application.platform, key);
		return fullKey;
	}

	private string GetString (string key)
	{
		return EditorPrefs.GetString (GetKey(key));
	}

	private int GetInt (string key)
	{
		return EditorPrefs.GetInt (GetKey (key));
	}

	private void SetString (string key, string value)
	{
		EditorPrefs.SetString (GetKey (key), value);
	}

	private void SetInt (string key, int value)
	{
		EditorPrefs.SetInt (GetKey (key), value);
	}

	private void OnEnable ()
	{
		m_selectedPlatform = GetInt("buildTargetSelected");
		m_modsFolder = GetString ("modsFolder");
	}

	private void SavePrefs ()
	{
		SetInt ("buildTargetSelected", m_selectedPlatform);
		SetString("modsFolder", m_modsFolder);
	}	

	private static void Log(string msg, params object[] args)
	{
		var formattedMsg = msg.With (args);
		m_log.AppendLine (formattedMsg);

		Console.WriteLine(formattedMsg);
		//Repaint ();
	}
	#endregion
}
