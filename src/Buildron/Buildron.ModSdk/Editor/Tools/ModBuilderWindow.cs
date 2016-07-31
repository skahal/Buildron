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

public class ModBuilderWindow : EditorWindow
{
	#region Fields
	private bool m_buildToWin;
	private bool m_buildToOsx;
	private bool m_buildToLinux;
	private string m_modsFolder;

	private StringBuilder m_log = new StringBuilder ();
	private Vector2 m_logScroll;
	#endregion

	#region Constructors
	public ModBuilderWindow()
	{
		titleContent.text = "Build mod";
		autoRepaintOnSceneChange = true;
		minSize = new Vector2 (700, 455);
		LoadPrefs ();
	}
	#endregion

	#region Build
	private void Build ()
	{
//		var deployRootFolder = "/Users/giacomelli/Dropbox/Skahal/Apps/Buildron/build/Mods/";
//		var deployRootFolder = @"C:\Dropbox\Skahal\Apps\Buildron\build\Mods";
		m_log = new StringBuilder();

		try 
		{
			Log("Building mods...");

			if (m_buildToWin) {
				Log ("Building for Windows...");
				BuildMods (m_modsFolder, BuildTarget.StandaloneWindows);
			}

			if (m_buildToOsx) {
				Log ("Building for OSX...");
				BuildMods (m_modsFolder, BuildTarget.StandaloneOSXIntel);
			}

			if (m_buildToLinux) {
				Log ("Building for Linux...");
				BuildMods (m_modsFolder, BuildTarget.StandaloneLinux);
			}

			Log("Done.");
		}
		catch(Exception ex) {
			Log ("Error: {0}", ex.Message);
		}
	}

	private void BuildMods(string deployRootFolder, BuildTarget buildTarget)
	{
		var assetsDeployFolder = Path.Combine(deployRootFolder, "Assets");

		if (!Directory.Exists(assetsDeployFolder))
		{
			Log ("Creating folder {0}...", assetsDeployFolder);
			Directory.CreateDirectory(assetsDeployFolder);
		}

		Log ("Building asset bundles...");
		BuildPipeline.BuildAssetBundles(assetsDeployFolder, BuildAssetBundleOptions.None, buildTarget);

		MoveAssetsToModsFolders(deployRootFolder, assetsDeployFolder);
	}

	private void MoveAssetsToModsFolders(string deployRootFolder, string assetsDeployFolder)
	{
		Log("Moving assets to mod folder");
		var assetFile = Directory
			.GetFiles (assetsDeployFolder, "*.manifest")
			.FirstOrDefault (f => !f.EndsWith("Assets.manifest"));

		if (assetFile == null) {
			Log ("No assets manifest file found. Did you remember to mark your assets with asset bundle with same name of your mod project?");
			Log ("Aborted.");
			return;
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
	}

	private void MoveAssemblies(string modName, string deployRootFolder)
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
	#endregion

	#region GUI
	[MenuItem ("Buildron/Build mod")]
	private static void Init ()
	{	
		var instance = GetWindow<ModBuilderWindow> ();
		instance.ShowPopup ();
	}

	/// <summary>
	/// Draws the window's GUI.
	/// </summary>
	private void OnGUI ()
	{
		m_buildToWin = EditorGUILayout.Toggle ("Windows", m_buildToWin);
		m_buildToOsx = EditorGUILayout.Toggle ("OSX", m_buildToOsx);
		m_buildToLinux = EditorGUILayout.Toggle ("Linux", m_buildToLinux);

		m_modsFolder = EditorGUILayout.TextField ("Mods folder", m_modsFolder);
	
		if (GUILayout.Button ("Build")) {
			SavePrefs ();
			Build ();		
		}

		m_logScroll = EditorGUILayout.BeginScrollView (m_logScroll, GUILayout.Height (200));  

		var logText = m_log.ToString ();
		var height = GUIStyle.none.CalcHeight (new GUIContent (logText), minSize.x);
		height = height < 100 ? 100 : height; 
		EditorGUILayout.TextArea (logText, GUILayout.Height (height));        
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

	private bool GetBool (string key)
	{
		return EditorPrefs.GetBool (GetKey (key));
	}

	private void SetString (string key, string value)
	{
		EditorPrefs.SetString (GetKey (key), value);
	}

	private void SetBool (string key, bool value)
	{
		EditorPrefs.SetBool (GetKey (key), value);
	}

	private void LoadPrefs ()
	{
		m_buildToLinux = GetBool("buildToLinux");
		m_buildToOsx = GetBool("buildToOsx");
		m_buildToWin = GetBool("buildToWin");
		m_modsFolder = GetString ("modsFolder");
	}

	private void SavePrefs ()
	{
		SetBool("buildToLinux", m_buildToLinux);
		SetBool("buildToOsx", m_buildToOsx);
		SetBool("buildToWin", m_buildToWin);
		SetString("modsFolder", m_modsFolder);
	}	

	private void Log(string msg, params object[] args)
	{
		var formattedMsg = msg.With (args);
		m_log.AppendLine (formattedMsg);
		Repaint ();
	}
	#endregion
}
