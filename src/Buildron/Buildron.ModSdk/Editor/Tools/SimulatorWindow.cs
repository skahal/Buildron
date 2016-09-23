using UnityEditor;
using Buildron.Domain.Mods;
using UnityEngine;
using System;
using Buildron.Domain.RemoteControls;
using Buildron.Domain.Builds;
using Buildron.Infrastructure.PreferencesProxies;
using Skahal.Common;

public class SimulatorWindow : EditorWindow
{
	#region Fields
	private int m_tab;
	private bool m_ciServerConnected;
	private BuildStatus m_buildStatus = BuildStatus.Success;
	private bool m_randomStatus;
	private FilterBuildsRemoteControlCommand m_filterCmd = new FilterBuildsRemoteControlCommand(String.Empty);
	#endregion

	#region Constructors
	public SimulatorWindow ()
	{
		titleContent.text = "Simulator";
		autoRepaintOnSceneChange = true;
		minSize = new Vector2 (200, 400);
	}
	#endregion

	#region GUI
	[MenuItem ("Buildron/Show Simulator %e")]
	private static void Init ()
	{	
		EnsureSimulatorGameObjects ();

		var instance = GetWindow<SimulatorWindow> ();
		instance.ShowUtility ();
	}

	static void EnsureSimulatorGameObjects ()
	{
		var go = GameObject.Find ("Simulator");
		if (go == null) {
			go = new GameObject ("Simulator");
			go.AddComponent<SimulatorModContext> ();

			var userConfig = new GameObject ("UserConfig");
			userConfig.transform.parent = go.transform;
			userConfig.AddComponent<SimulatorUserConfig> ();

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
        if (EditorApplication.isPlaying && SimulatorModContext.Instance != null)
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
            CreateHelpBox("Simulator is activate on play mode.");
        }
    }    

    private void ShowEventsTab()
    {
		if (!m_ciServerConnected && GUILayout.Button("CIServerConnected"))
        {
			m_ciServerConnected = true;
            SimulatorModContext.Instance.RaiseCIServerConnected();
        }

        EditorGUILayout.Separator();
        if (GUILayout.Button("BuildFound"))
        {
            var build = new SimulatorBuild();
			build.Status = GetBuildStatus();
            SimulatorModContext.Instance.RaiseBuildFound(build);
        }

        if (GUILayout.Button("BuildStatusChanged"))
        {
			SimulatorModContext.Instance.RaiseBuildStatusChanged(GetBuildStatus());
        }

		GUILayout.BeginHorizontal();
		GUI.enabled = !m_randomStatus;
        CreateControl("Build status", () => m_buildStatus = (BuildStatus)EditorGUILayout.EnumPopup(m_buildStatus));
		GUI.enabled = true;
		m_randomStatus = GUILayout.Toggle(m_randomStatus, "random");
		GUILayout.EndHorizontal();

        EditorGUILayout.Separator();
        if (GUILayout.Button("BuildRemoved"))
        {
            SimulatorModContext.Instance.RaiseBuildRemoved(0);
        }

        EditorGUILayout.Separator();
        if (GUILayout.Button("FilterBuildsRemoteControlCommand"))
        {
            SimulatorModContext.Instance.RaiseRemoteControlCommandReceived(m_filterCmd);
        }

        CreateControl("KeyWord", () => m_filterCmd.KeyWord = GUILayout.TextField(m_filterCmd.KeyWord));  
    }

    private void ShowPreferencesTab()
    {
        var preferences = (ModPreferencesProxy)SimulatorModContext.Instance.Preferences;

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

	private BuildStatus GetBuildStatus()
	{
		if (m_randomStatus)
		{
			m_buildStatus = SHRandomHelper.NextEnum(BuildStatus.Failed, BuildStatus.Success, BuildStatus.Queued, BuildStatus.Running);
		}

		return m_buildStatus;
	}
	#endregion
}
