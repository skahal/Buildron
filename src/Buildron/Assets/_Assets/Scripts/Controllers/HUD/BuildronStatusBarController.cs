#region Usings
using Buildron.Domain;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Buildron.Domain.RemoteControls;


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

		Messenger.Register (
			gameObject, 
			"OnRemoteControlConnected", 
			"OnRemoteControlDisconnected",
			"OnBuildFilterUpdated");
	}
	
	private void OnRemoteControlConnected (RemoteControl remoteControl)
	{
		RemoteControlInfoLabel.text = remoteControl.UserName;
		RemoteControlInfoLabel.enabled = true;
		RemoteControlInfoImage.enabled = true;
	}
	
	private void OnRemoteControlDisconnected ()
	{
		RemoteControlInfoLabel.enabled = false;
		RemoteControlInfoImage.enabled = false;
	}
	
	private void OnBuildFilterUpdated()
	{
		var state = m_serverService.GetState ();
		FilteredInfo.enabled = !state.BuildFilter.IsEmpty;
	}
	#endregion
}