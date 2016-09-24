#region Usings
using Buildron.Domain;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Buildron.Domain.RemoteControls;
using Buildron.Domain.Servers;


#endregion

/// <summary>
/// Status bar controller.
/// </summary>
[AddComponentMenu("Buildron/HUD/BuildronStatusBarController")]
public class BuildronStatusBarController : StatusBarController 
{
	#region Fields
	[Inject]
	private IServerService m_serverService;

	[Inject]
	private IRemoteControlService m_rcService;
	#endregion

	#region Properties
	public Image RemoteControlInfoImage;
	public Text RemoteControlInfoLabel;
	public Image FilteredInfo;
	#endregion
	
	#region Methods
	private void Start ()
	{
		RemoteControlInfoLabel.enabled = false;
		RemoteControlInfoImage.enabled = false;
     
		// Filter updated.
		m_rcService.RemoteControlCommandReceived += (sender, e) => {
			if(e.Command is FilterBuildsRemoteControlCommand)
			{
				RefreshFilterInfo();
			}
		};

		// RC connected/disconnected
		m_rcService.RemoteControlChanged += (sender, e) => {
			var connected = e.RemoteControl != null;
			RemoteControlInfoLabel.text = connected ? e.RemoteControl.UserName : string.Empty;
			RemoteControlInfoLabel.enabled = true;
			RemoteControlInfoImage.enabled = true;
		};

		RefreshFilterInfo ();
	}

	void RefreshFilterInfo ()
	{
		var state = m_serverService.GetState ();
		FilteredInfo.enabled = !state.BuildFilter.IsEmpty;
	}
	#endregion
}