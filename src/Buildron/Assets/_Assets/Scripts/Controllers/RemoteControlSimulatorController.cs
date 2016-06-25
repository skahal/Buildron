#region Usings
using Buildron.Domain;
using UnityEngine;
using Zenject;


#endregion

/// <summary>
/// Remote control simulator controller.
/// </summary>
public class RemoteControlSimulatorController : MonoBehaviour
{
	#region Fields
	[Inject]
	private IRemoteControlMessagesListener m_listener;
	#endregion

	#region Methods
	public void ShowHistory ()
	{
		m_listener.SendToServerShowHistory ();
	}
	
	public void ShowBuilds ()
	{
		m_listener.SendToServerShowBuilds ();
	}
	
	public void ShowFailedBuilds ()
	{
		m_listener.ShowFailedBuilds (!ServerState.Instance.BuildFilter.FailedEnabled);
	}
	
	public void ShowSuccessBuilds ()
	{
		m_listener.ShowSuccessBuilds (!ServerState.Instance.BuildFilter.SuccessEnabled);
	}
	
	public void ShowRunningBuilds ()
	{
		m_listener.ShowRunningBuilds (!ServerState.Instance.BuildFilter.RunningEnabled);
	}
	
	public void ShowQueuedBuilds ()
	{
		m_listener.ShowQueuedBuilds (!ServerState.Instance.BuildFilter.QueuedEnabled);
	}
	
	public void SendMatrixEasterEgg ()
	{
		m_listener.ShowBuildsWithName ("/matrix");
	}
	
	public void SendZoomIn ()
	{
		m_listener.SendToServerZoomIn();
	}
	
	public void SendZoomOut ()
	{
		m_listener.SendToServerZoomOut ();
	}
	#endregion
}