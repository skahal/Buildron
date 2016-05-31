#region Usings
using Buildron.Domain;
using UnityEngine;
#endregion

/// <summary>
/// Remote control simulator controller.
/// </summary>
public class RemoteControlSimulatorController : MonoBehaviour
{
	#region Methods
	public void ShowHistory ()
	{
		ServerService.Listener.SendToServerShowHistory ();
	}
	
	public void ShowBuilds ()
	{
		ServerService.Listener.SendToServerShowBuilds ();
	}
	
	public void ShowFailedBuilds ()
	{
		ServerService.Listener.ShowFailedBuilds (!ServerState.Instance.BuildFilter.FailedEnabled);
	}
	
	public void ShowSuccessBuilds ()
	{
		ServerService.Listener.ShowSuccessBuilds (!ServerState.Instance.BuildFilter.SuccessEnabled);
	}
	
	public void ShowRunningBuilds ()
	{
		ServerService.Listener.ShowRunningBuilds (!ServerState.Instance.BuildFilter.RunningEnabled);
	}
	
	public void ShowQueuedBuilds ()
	{
		ServerService.Listener.ShowQueuedBuilds (!ServerState.Instance.BuildFilter.QueuedEnabled);
	}
	
	public void SendMatrixEasterEgg ()
	{
		ServerService.Listener.ShowBuildsWithName ("/matrix");
	}
	
	public void SendZoomIn ()
	{
		ServerService.Listener.SendToServerZoomIn();
	}
	
	public void SendZoomOut ()
	{
		ServerService.Listener.SendToServerZoomOut ();
	}
	#endregion
}