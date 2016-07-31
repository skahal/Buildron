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
using Buildron.Domain.RemoteControls;

public class EmulatorWindow : EditorWindow
{
	#region Fields
	private FilterBuildsRemoteControlCommand m_filterCmd = new FilterBuildsRemoteControlCommand(String.Empty);
	#endregion

	#region Constructors

	public EmulatorWindow ()
	{
		titleContent.text = "Emulator";
		autoRepaintOnSceneChange = true;
		minSize = new Vector2 (200, 400);
	}

	#endregion

	#region GUI

	[MenuItem ("Buildron/Show emulator %e")]
	private static void Init ()
	{	
		EnsureEmulatorGameObjects ();


		var instance = GetWindow<EmulatorWindow> ();
		instance.ShowUtility ();
	}

	static void EnsureEmulatorGameObjects ()
	{
		var go = GameObject.Find ("Emulator");
		if (go == null) {
			go = new GameObject ("Emulator");
			go.AddComponent<EmulatorModContext> ();

			var userConfig = new GameObject ("UserConfig");
			userConfig.transform.parent = go.transform;
			userConfig.AddComponent<EmulatorUserConfig> ();

			var poolsManager = new GameObject ("PoolsManager");
			poolsManager.transform.parent = go.transform;
			poolsManager.AddComponent<SHPoolsManager> ();
		}
	}

	/// <summary>
	/// Draws the window's GUI.
	/// </summary>
	private void OnGUI ()
	{
		if (EditorApplication.isPlaying && EmulatorModContext.Instance != null) {
			if (GUILayout.Button ("CIServerConnected")) {
				EmulatorModContext.Instance.RaiseCIServerConnected ();
			}

			if (GUILayout.Button ("BuildFound")) {
				var build = new EmulatorBuild ();
				EmulatorModContext.Instance.RaiseBuildFound (build);
			}

			if (GUILayout.Button ("BuildRemoved")) {
				EmulatorModContext.Instance.RaiseBuildRemoved (0);
			}

			EditorGUILayout.Separator ();
			if (GUILayout.Button ("FilterBuildsRemoteControlCommand")) {
				EmulatorModContext.Instance.RaiseRemoteControlCommandReceived (m_filterCmd);
			}

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label ("KeyWord: ");
			m_filterCmd.KeyWord = GUILayout.TextField (m_filterCmd.KeyWord);
			EditorGUILayout.EndHorizontal ();
		
		} else {
			CreateHelpBox ("Emulator is activate on play mode.");
		}
	}

	private void CreateHelpBox (string helpText, string url = null)
	{
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.HelpBox (helpText, MessageType.None, false);

		if (!string.IsNullOrEmpty (url) && GUILayout.Button ("Get", GUILayout.Width (40))) {
			Application.OpenURL (url);
		}

		EditorGUILayout.EndHorizontal ();	
		EditorGUILayout.Separator ();
	}

	#endregion
}
