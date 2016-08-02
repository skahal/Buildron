using UnityEditor;
using Buildron.Domain.Mods;
using UnityEngine;
using System;
using Buildron.Domain.RemoteControls;
using Buildron.Domain.Builds;
using Buildron.Infrastructure.PreferencesProxies;

public class EmulatorWindow : EditorWindow
{
	#region Fields
	private int m_tab;
	private bool m_ciServerConnected;
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
    private void OnGUI()
    {        
        if (EditorApplication.isPlaying && EmulatorModContext.Instance != null)
        {
			m_tab = GUILayout.Toolbar(m_tab, new string[] { "Events", "Preferences" });

			switch (m_tab)
            {
                case 0:
                    ShowEventsTab();
                    break;

                case 1:
                    ShowPreferencesTab();
                    break;
            }
        }
        else
        {
			m_ciServerConnected = false;
            CreateHelpBox("Emulator is activate on play mode.");
        }
    }    

    private void ShowEventsTab()
    {
		if (!m_ciServerConnected && GUILayout.Button("CIServerConnected"))
        {
			m_ciServerConnected = true;
            EmulatorModContext.Instance.RaiseCIServerConnected();
        }

        EditorGUILayout.Separator();
        if (GUILayout.Button("BuildFound"))
        {
            var build = new EmulatorBuild();
            build.Status = m_buildStatus;
            EmulatorModContext.Instance.RaiseBuildFound(build);
        }

        if (GUILayout.Button("BuildStatusChanged"))
        {
			EmulatorModContext.Instance.RaiseBuildStatusChanged(m_buildStatus);
        }

        CreateControl("Build status", () => m_buildStatus = (BuildStatus)EditorGUILayout.EnumPopup(m_buildStatus));

        EditorGUILayout.Separator();
        if (GUILayout.Button("BuildRemoved"))
        {
            EmulatorModContext.Instance.RaiseBuildRemoved(0);
        }

        EditorGUILayout.Separator();
        if (GUILayout.Button("FilterBuildsRemoteControlCommand"))
        {
            EmulatorModContext.Instance.RaiseRemoteControlCommandReceived(m_filterCmd);
        }

        CreateControl("KeyWord", () => m_filterCmd.KeyWord = GUILayout.TextField(m_filterCmd.KeyWord));  
    }

    private void ShowPreferencesTab()
    {
        var preferences = (ModPreferencesProxy)EmulatorModContext.Instance.Preferences;

        foreach (var p in preferences.GetRegisteredPreferences())
        {
            switch(p.Kind)
            {
                case PreferenceKind.Bool:
                    CreateControl(p.Title, () => preferences.SetValue<bool>(p.Name, GUILayout.Toggle(preferences.GetValue<bool>(p.Name), string.Empty)));
                    break;

                case PreferenceKind.Float:
                    CreateControl(p.Title, () => preferences.SetValue<float>(p.Name, Convert.ToSingle(GUILayout.TextField(preferences.GetValue<float>(p.Name).ToString()))));
                    break;

                case PreferenceKind.Int:
				CreateControl(p.Title, () => {
					var intValue = GUILayout.TextField(preferences.GetValue<int>(p.Name).ToString());

					preferences.SetValue<int>(p.Name, String.IsNullOrEmpty(intValue) ? 0 : Convert.ToInt32(intValue));
				});
                    break;

                case PreferenceKind.String:
                    CreateControl(p.Title, () => preferences.SetValue<string>(p.Name, GUILayout.TextField(preferences.GetValue<string>(p.Name))));
                    break;
            }            
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
