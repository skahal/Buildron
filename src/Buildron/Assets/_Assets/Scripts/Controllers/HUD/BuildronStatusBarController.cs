#region Usings
using Buildron.Domain;
using UnityEngine;
using UnityEngine.UI;


#endregion

/// <summary>
/// Status bar controller.
/// </summary>
[AddComponentMenu("Buildron/HUD/BuidronStatusBarController")]
public class BuildronStatusBarController : StatusBarController {

	#region Editor properties
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
		FilteredInfo.enabled = !ServerState.Instance.BuildFilter.IsEmpty;
	}
	#endregion
}