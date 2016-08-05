using System;
using System.Linq;
using Buildron.Domain;
using Buildron.Domain.Builds;
using Buildron.Domain.CIServers;
using Buildron.Domain.Users;
using Buildron.Domain.Versions;
using Buildron.Infrastructure.BuildsProviders.Filter;
using Buildron.Infrastructure.BuildsProvider.Hudson;
using Buildron.Infrastructure.BuildsProvider.Jenkins;
using Buildron.Infrastructure.BuildsProvider.TeamCity;
using Skahal.Threading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Buildron.Domain.RemoteControls;
using Buildron.Domain.Servers;
using Buildron.Domain.Mods;

/// <summary>
/// Controller for configuration panel.
/// </summary>
[AddComponentMenu ("Buildron/Controllers/ConfigPanelController")]
public class ConfigPanelController : MonoBehaviour, IInitializable
{
	#region Fields

	private ICIServer m_ciServer;
	private IBuildsProvider m_buildsProvider;
	private Animator m_animator;

	[Inject]
	private IUserService m_userService;

	[Inject]
	private IVersionService m_versionService;

	[Inject]
	private IBuildService m_buildService;

	[Inject]
	private ICIServerService m_ciServerService;

	[Inject]
	private IRemoteControlService m_remoteControlService;

	[Inject]
	private IServerService m_serverService;

	[Inject]
	private IModLoader m_modLoader;
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
	public Button ModsButton;
	public Text ModsLabel;
	public Text RefreshSecondsLabel;
	public Slider RefreshSecondsSlider;

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
		m_ciServer = m_ciServerService.GetCIServer ();
		Debug.LogFormat ("Server type: {0}", m_ciServer.ServerType);
		Debug.LogFormat ("IP: {0}", m_ciServer.IP);
		
		PrepareCIServerTypesRadioButtons ();
		CIServerIPInputField.text = m_ciServer.IP;
		CIServerUserNameInputField.text = m_ciServer.DomainAndUserName;
		CIServerPasswordInputField.text = m_ciServer.Password;

		RefreshSecondsSlider.value = m_ciServer.RefreshSeconds;
		UpdateRefreshSecondsLabel ();

		UpdateBuildsProvider ();

		if (AutoStart || HasAutoStartArgument ())
		{
			StartBuildron ();
		}

		m_userService.UserAuthenticationCompleted += HandleUserAuthenticationCompleted;
		
		InitializeVersion ();
	}

	void Start()
	{
		var modsCount = m_modLoader.LoadedMods.Count;
		ModsLabel.text = "{0} mods".With (modsCount);
		ModsButton.enabled = modsCount > 0;
	}

	private bool HasAutoStartArgument ()
	{
		return System.Environment.GetCommandLineArgs ().Contains ("autostart");
	}

	void PrepareCIServerTypesRadioButtons ()
	{
		switch (m_ciServer.ServerType)
		{
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

		m_versionService.ClientRegistered += (object sender, ClientRegisteredEventArgs e) =>
		{ 
			InstallationNumberLabel.text = string.Format ("Installation number: {0}", e.ClientInstance);
		};
		
		m_versionService.Register (ClientKind.Buildron, SHDevice.Family);		

		if (!SHGameInfo.IsBetaVersion)
		{
			m_versionService.UpdateInfoReceived += delegate(object sender, UpdateInfoReceivedEventArgs e)
			{		
				UpdateButtonLabel.text = e.UpdateInfo.Description;
		
				if (!string.IsNullOrEmpty (e.UpdateInfo.Url))
				{
					UpdateButton.onClick.AddListener (() =>
					{
						Application.OpenURL (e.UpdateInfo.Url);
					});
				}
			};
			
			m_versionService.CheckUpdates (ClientKind.Buildron, SHDevice.Family);
		}
		
		VersionNumberButtonLabel.text = string.Format ("Version: {0}", SHGameInfo.Version);
	}

	private bool CanStart
	{
		get
		{
			return !string.IsNullOrEmpty (CIServerIPInputField.text)
			&& (m_buildsProvider != null && m_buildsProvider.AuthenticationRequirement != AuthenticationRequirement.Always
			|| (!string.IsNullOrEmpty (CIServerUserNameInputField.text)
			&& !string.IsNullOrEmpty (CIServerPasswordInputField.text)));
		}
	}

	private void HandleUserAuthenticationCompleted (object sender, UserAuthenticationCompletedEventArgs e)
	{
		if (e.Success)
		{
			CIServerStatusLabel.text = "Authenticated. Loading...";

			m_userService.UserAuthenticationCompleted -= HandleUserAuthenticationCompleted;

			//m_animator.enabled = true;
			PanelTransitionController.Instance.ShowMainPanel ();	
		} 
		else
		{
			CIServerStatusLabel.text = "IP, Username or Password invalid!";

			if (HasAutoStartArgument ())
			{
				SHCoroutine.Start (10f, StartBuildron);
			}
		}
	}

	public void UpdateBuildsProvider ()
	{
		if (CIServerTypeHudsonToggle.isOn)
		{
			m_buildsProvider = new HudsonBuildsProvider (m_ciServer);
			m_ciServer.ServerType = CIServerType.Hudson;
		} 
		else if (CIServerTypeJenkinsToggle.isOn)
		{
			m_buildsProvider = new JenkinsBuildsProvider (m_ciServer);
			m_ciServer.ServerType = CIServerType.Jenkins;
		} 
		else
		{
			m_buildsProvider = new TeamCityBuildsProvider (m_ciServer);
			m_ciServer.ServerType = CIServerType.TeamCity;
		}
		
		CIServerIPLabel.text = string.Format ("{0} IP", m_buildsProvider.Name);
		CIServerAuthenticationTipLabel.text = m_buildsProvider.AuthenticationTip;
	}

	public void DestroyPanel ()
	{
		Destroy (gameObject);
	}

	public void UpdateRefreshSecondsLabel ()
	{
		RefreshSecondsLabel.text = String.Format ("Refresh seconds: {0}", RefreshSecondsSlider.value);
	}

	#endregion

	#region Panel commands

	public void StartBuildron ()
	{	
		Debug.Log ("Starting...");
		m_ciServer.IP = CIServerIPInputField.text;
		m_ciServer.UserName = CIServerUserNameInputField.text;
		m_ciServer.Password = CIServerPasswordInputField.text;
		m_ciServer.RefreshSeconds = Convert.ToInt32 (RefreshSecondsSlider.value);
			
		m_ciServerService.SaveCIServer (m_ciServer);
		
		UpdateBuildsProvider ();
			
		if (!m_ciServerService.Initialized)
		{
		
			if (m_ciServer.IP.Equals ("#TEST_MODE#"))
			{
				m_buildsProvider = new TestBuildsProvider ();
			}

			// Inject the FilterBuildsProvider.
			m_buildsProvider = new FilterBuildsProvider (m_buildsProvider, m_remoteControlService, m_serverService);			
			m_userService.Initialize (m_buildsProvider);

			CIServerStatusLabel.text = string.Format ("Trying to connect to {0}...", m_buildsProvider.Name);
			m_buildService.Initialize (m_buildsProvider);
			m_ciServerService.Initialize (m_buildsProvider);
			m_ciServerService.AuthenticateUser (m_ciServer);
		}
	}

	#endregion
}