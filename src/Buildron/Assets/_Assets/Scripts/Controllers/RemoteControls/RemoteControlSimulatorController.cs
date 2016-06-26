#region Usings
using Buildron.Domain;
using UnityEngine;
using Zenject;
using Buildron.Domain.RemoteControls;
using Buildron.Domain.Servers;


#endregion

/// <summary>
/// Remote control simulator controller.
/// </summary>
public class RemoteControlSimulatorController : MonoBehaviour, IInitializable
{
	#region Fields
	[Inject]
	private IRemoteControlMessagesListener m_listener;

	[Inject]
	private IServerService m_serverService;

	private ServerState m_serverState;
	#endregion

	#region Methods
	public void Initialize()
	{
		m_serverState = m_serverService.GetState ();
	}

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
		m_listener.ShowFailedBuilds (!m_serverState.BuildFilter.FailedEnabled);
	}
	
	public void ShowSuccessBuilds ()
	{
		m_listener.ShowSuccessBuilds (!m_serverState.BuildFilter.SuccessEnabled);
	}
	
	public void ShowRunningBuilds ()
	{
		m_listener.ShowRunningBuilds (!m_serverState.BuildFilter.RunningEnabled);
	}
	
	public void ShowQueuedBuilds ()
	{
		m_listener.ShowQueuedBuilds (!m_serverState.BuildFilter.QueuedEnabled);
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