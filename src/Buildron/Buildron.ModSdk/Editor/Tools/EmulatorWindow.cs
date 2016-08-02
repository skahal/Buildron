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
using Buildron.Domain.Builds;

public class EmulatorWindow : EditorWindow
{
	#region Fields
	private BuildStatus m_buildStatus = BuildStatus.Success;
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

			EditorGUILayout.Separator ();
			if (GUILayout.Button ("BuildFound")) {
				var build = new EmulatorBuild ();
				build.Status = m_buildStatus;
				EmulatorModContext.Instance.RaiseBuildFound (build);
			}

			if (GUILayout.Button ("BuildStatusChanged")) {
				var build = new EmulatorBuild ();
				build.Status = m_buildStatus;
				EmulatorModContext.Instance.RaiseBuildStatusChanged (build);
			}

			CreateControl("Build status", () =>  m_buildStatus = (BuildStatus) EditorGUILayout.EnumPopup (m_buildStatus));

			EditorGUILayout.Separator ();
			if (GUILayout.Button ("BuildRemoved")) {
				EmulatorModContext.Instance.RaiseBuildRemoved (0);
			}

			EditorGUILayout.Separator ();
			if (GUILayout.Button ("FilterBuildsRemoteControlCommand")) {
				EmulatorModContext.Instance.RaiseRemoteControlCommandReceived (m_filterCmd);
			}

			CreateControl ("KeyWord", () =>  m_filterCmd.KeyWord = GUILayout.TextField (m_filterCmd.KeyWord));
		} else {
			CreateHelpBox ("Emulator is activate on play mode.");
		}
	}

	private void CreateControl(string label, Action createControl)
	{
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("{0}: ".With(label));
		createControl ();
		EditorGUILayout.EndHorizontal ();
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
