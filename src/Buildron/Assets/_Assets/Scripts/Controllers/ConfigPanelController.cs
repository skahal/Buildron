#region Usings
using System.Linq;
using Buildron.Domain;
using Buildron.Domain.Versions;
using Buildron.Infrastructure.BuildsProvider.Filter;
using Buildron.Infrastructure.BuildsProvider.Hudson;
using Buildron.Infrastructure.BuildsProvider.Jenkins;
using Buildron.Infrastructure.BuildsProvider.TeamCity;
using UnityEngine;
using UnityEngine.UI;
using System;
using Skahal.Threading;
using Zenject;


#endregion

/// <summary>
/// Controller for configuration panel.
/// </summary>
[AddComponentMenu("Buildron/Controllers/ConfigPanelController")]
public class ConfigPanelController : MonoBehaviour, IInitializable
{
	#region Fields
	private CIServer m_CIServer;
	private IBuildsProvider m_buildsProvider;
	private Animator m_animator;

	[Inject]
	private IUserService m_userService;

	[Inject]
	private IVersionService m_versionService; 
    #endregion

    #region Editor properties
    public Toggle CIServerTypeHudsonToggle;
	public Toggle CIServerTypeJenkinsToggle;
	public Toggle CIServerTypeTeamCityToggle;

	// CI Server.
	public Text CIServerIPLabel;
	public InputField CIServerIPInputField;

	// CI Username.
	public InputField CIServerUserNameInputField;

	// CI Password.
	public InputField CIServerPasswordInputField;

	// CI tip and status.
	public Text CIServerAuthenticationTipLabel;
	public Text CIServerStatusLabel;

	// Options.
	public Text RefreshSecondsLabel;
	public Slider RefreshSecondsSlider;

	public Text BuildsTotemsLabel;
	public Slider BuildsTotemsSlider;

	public Toggle FxSounsToggle;
	public Toggle HistoryTotemToggle;

	public Text InstallationNumberLabel;
	public Button UpdateButton;
	public Text UpdateButtonLabel;
	public Text VersionNumberButtonLabel;


	public Button StartButton;
	public bool AutoStart;
	#endregion
	
	#region Life cycle
	public void Initialize ()
	{		
		m_CIServer = CIServerService.GetCIServer ();
		Debug.LogFormat ("Server type: {0}", m_CIServer.ServerType);
		Debug.LogFormat ("IP: {0}", m_CIServer.IP);
		
		PrepareCIServerTypesRadioButtons ();
		CIServerIPInputField.text = m_CIServer.IP;
		CIServerUserNameInputField.text = m_CIServer.DomainAndUserName;
		CIServerPasswordInputField.text = m_CIServer.Password;

		RefreshSecondsSlider.value = m_CIServer.RefreshSeconds;
		UpdateRefreshSecondsLabel ();

		BuildsTotemsSlider.value = m_CIServer.BuildsTotemsNumber;
		UpdateBuildTotemsLabel ();

		FxSounsToggle.isOn = m_CIServer.FxSoundsEnabled;
		HistoryTotemToggle.isOn = m_CIServer.HistoryTotemEnabled;
		UpdateBuildsProvider ();

		UpdateStartButton ();	
		
		if (AutoStart || HasAutoStartArgument ()) {
			StartBuildron ();
		}

        m_userService.UserAuthenticationCompleted += HandleUserAuthenticationCompleted;
		
		InitializeVersion ();
		//m_animator = GetComponent<Animator> ();
	}
	
	private bool HasAutoStartArgument ()
	{
		return System.Environment.GetCommandLineArgs().Contains("autostart");
	}
	
	void PrepareCIServerTypesRadioButtons ()
	{
		switch (m_CIServer.ServerType) {
		case CIServerType.Hudson:
			Debug.Log ("CIServerTypeHudsonToggle.isOn");
			CIServerTypeHudsonToggle.isOn = true;
			break;
				
		case CIServerType.Jenkins:
			Debug.Log ("CIServerTypeJenkinsToggle.isOn");
			CIServerTypeJenkinsToggle.isOn = true;
			break;
			
		default:
			Debug.Log ("CIServerTypeTeamCityToggle.isOn");
			CIServerTypeTeamCityToggle.isOn = true;
			break;
		}
	}

	void InitializeVersion ()
	{
		InstallationNumberLabel.text = string.Empty;

		m_versionService.ClientRegistered += (object sender, ClientRegisteredEventArgs e) => { 
			InstallationNumberLabel.text = string.Format ("Installation number: {0}", e.ClientInstance);
		};
		
		m_versionService.Register (ClientKind.Buildron, SHDevice.Family);		

		if (!SHGameInfo.IsBetaVersion) {
			m_versionService.UpdateInfoReceived += delegate(object sender, UpdateInfoReceivedEventArgs e) {		
				UpdateButtonLabel.text = e.UpdateInfo.Description;
		
				if (!string.IsNullOrEmpty (e.UpdateInfo.Url)) {
					UpdateButton.onClick.AddListener(() => 
					{
						Application.OpenURL(e.UpdateInfo.Url);
					});
				}
			};
			
			m_versionService.CheckUpdates (ClientKind.Buildron, SHDevice.Family);
		}
		
		VersionNumberButtonLabel.text = string.Format ("Version: {0}", SHGameInfo.Version);
	}
	
	private bool CanStart 
	{
		get {
			return !string.IsNullOrEmpty (CIServerIPInputField.text) 
				&& (m_buildsProvider != null && m_buildsProvider.AuthenticationRequirement != AuthenticationRequirement.Always
				|| (!string.IsNullOrEmpty (CIServerUserNameInputField.text)
				&& !string.IsNullOrEmpty (CIServerPasswordInputField.text)));
		}
	}

	private void HandleUserAuthenticationCompleted(object sender, UserAuthenticationCompletedEventArgs e)
	{
        if (e.Success)
        {
            CIServerStatusLabel.text = "Authenticated. Loading...";

            m_userService.UserAuthenticationCompleted -= HandleUserAuthenticationCompleted;

            //m_animator.enabled = true;
            PanelTransitionController.Instance.ShowMainPanel();
            Messenger.Send("OnCIServerReady");
        }
        else
        {
            CIServerStatusLabel.text = "IP, Username or Password invalid!";

            if (HasAutoStartArgument())
            {
                SHThread.Start(1f, StartBuildron);
            }
        }
	}

	public void UpdateBuildsProvider ()
	{
		if (CIServerTypeHudsonToggle.isOn) 
		{
			m_buildsProvider = new HudsonBuildsProvider (m_CIServer);
			m_CIServer.ServerType = CIServerType.Hudson;
		} 
		else if (CIServerTypeJenkinsToggle.isOn)
		{
			m_buildsProvider = new JenkinsBuildsProvider (m_CIServer);
			m_CIServer.ServerType = CIServerType.Jenkins;
		} 
		else 
		{
			m_buildsProvider = new TeamCityBuildsProvider (m_CIServer);
			m_CIServer.ServerType = CIServerType.TeamCity;
		}
		
		CIServerIPLabel.text = string.Format ("{0} IP", m_buildsProvider.Name);
		CIServerAuthenticationTipLabel.text = m_buildsProvider.AuthenticationTip;
		UpdateStartButton ();
	}

	public void DestroyPanel()
	{
		Destroy (gameObject);
	}

	public void UpdateRefreshSecondsLabel()
	{
		RefreshSecondsLabel.text = String.Format ("Refresh seconds: {0}", RefreshSecondsSlider.value);
	}

	public void UpdateBuildTotemsLabel()
	{
		BuildsTotemsLabel.text = String.Format ("Build totems: {0}", BuildsTotemsSlider.value);
	}

	public void UpdateStartButton ()
	{
		StartButton.interactable = CanStart;
	}
	#endregion
	
	#region Panel commands
	public void StartBuildron ()
	{	
		Debug.Log ("Starting...");
		m_CIServer.IP = CIServerIPInputField.text;
		m_CIServer.UserName = CIServerUserNameInputField.text;
		m_CIServer.Password = CIServerPasswordInputField.text;
		m_CIServer.RefreshSeconds = Convert.ToInt32 (RefreshSecondsSlider.value);

		m_CIServer.FxSoundsEnabled = FxSounsToggle.isOn;
		m_CIServer.HistoryTotemEnabled = HistoryTotemToggle.isOn;
		m_CIServer.BuildsTotemsNumber = Convert.ToInt32(BuildsTotemsSlider.value);
		
		CIServerService.SaveCIServer (m_CIServer);
		
		UpdateBuildsProvider ();
			
		if (!BuildService.Initialized) {
		
			if (m_CIServer.IP.Equals ("#TEST_MODE#")) {
				m_buildsProvider = new TestBuildsProvider ();
			}

            // Inject the FilterBuildsProvider.
            m_buildsProvider = new FilterBuildsProvider(m_buildsProvider);

			BuildsProvider.Initialize (m_buildsProvider);
			m_userService.ListenBuildsProvider (m_buildsProvider);

			CIServerStatusLabel.text = string.Format ("Trying to connect to {0}...", m_buildsProvider.Name);
			BuildService.Initialize (m_buildsProvider);
			BuildService.AuthenticateUser (m_CIServer);
		}
	}
	#endregion
}