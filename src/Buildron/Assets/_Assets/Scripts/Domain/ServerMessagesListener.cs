#region Usings
using Buidron.Domain;
using Buildron.Domain;
using Buildron.Domain.EasterEgss;
using Buildron.Domain.Sorting;
using Skahal.Common;
using Skahal.Logging;
using Skahal.Serialization;
using UnityEngine;
#endregion

[RequireComponent(typeof(NetworkView))]
public class ServerMessagesListener : MonoBehaviour
{
	#region Events
	public event System.EventHandler BuildFilterUpdated;
	public event System.EventHandler<BuildSortUpdatedEventArgs> BuildSortUpdated;
	#endregion
	
	#region Life cycle
	private void Awake ()
	{
		var error = Network.InitializeServer (1, 8181, false);
	
		Messenger.Register (
			gameObject, 
			"OnBuildHidden",
			"OnBuildVisible",
			"OnScreenshotTaken",
			"OnSortingBegin",
			"OnSortingEnded",
			"OnBuildHistoryCreated");
		
		BuildService.UserAuthenticationSuccessful += delegate {
			SendToRCAuthenticationSuccessful ();
		};
		
		BuildService.UserAuthenticationFailed += delegate {
			SendToRCAuthenticationFailed ();
		};
		
		SHLog.Warning ("Server initialize: {0}", error);
	}
	
	private void Start()
	{
		SendFilterLocally();
	}
	
	private void OnPlayerConnected ()
	{
		SHLog.Debug ("Remote control connected.");	
		
		SendToRCCurrentServerState ();
		OnVisibleBuildsCount (BuildController.VisiblesCount);
	}
	
	private void OnPlayerDisconnected ()
	{
		SHLog.Debug ("Remote control disconnected.");
		RemoteControlService.DisconnectRemoteControl ();
		Messenger.Send ("OnRemoteControlDisconnected");	
	}
	
	private void OnBuildHidden ()
	{
		OnVisibleBuildsCount (BuildController.VisiblesCount);
	
	}
	
	private void OnBuildVisible ()
	{
		OnVisibleBuildsCount (BuildController.VisiblesCount);
	}
	
	private void OnScreenshotTaken (Texture2D texture)
	{
		SendToRCScreenshot (texture.width, texture.height, texture.EncodeToPNG ());
	}
	
	private void OnSortingBegin ()
	{
		ServerState.Instance.IsSorting = true;
		SendToRCCurrentServerState ();
	}
	
	private void OnSortingEnded ()
	{
		ServerState.Instance.IsSorting = false;
		SendToRCSortingEnded ();
		SendToRCCurrentServerState ();
	}
	
	private void OnBuildHistoryCreated ()
	{
		ServerState.Instance.HasHistory = true;
		SendToRCCurrentServerState();
	}
	#endregion
	
	#region Messages from remote control
	[RPC]
	public void SendToServerAuthentication (string userName, string password)
	{
		SHLog.Debug ("SendToServerAuthentication:" + userName);
		
		var rc = new RemoteControl 
		{
			UserName = userName,
			Password = password
		};
		
		RemoteControlService.ConnectRemoteControl (rc);
		Messenger.Send ("OnRemoteControlConnected", rc);
	}
	
	[RPC]
	public void SendToServerTakeScreenshot ()
	{
		SHLog.Debug ("SendToServerTakeScreenshot");
		
		Messenger.Send ("OnScreenshotRequested");
	}
	
	[RPC]
	public void SendToServerSortBuilds (int sortingAlgorithmType, int sortBy)
	{
		var sorting = (SortingAlgorithmType)sortingAlgorithmType;
		var sortByProperty = (SortBy)sortBy;
		SHLog.Debug ("SendToServerSortBuilds: {0}, {1}", sorting, sortByProperty);

		var args = new BuildSortUpdatedEventArgs (sorting, sortByProperty);
		Messenger.Send ("OnBuildSortUpdated", args);

		if (BuildSortUpdated != null) {
			BuildSortUpdated (this, args);
		}
	}
	
	[RPC]
	public void SendToServerShowHistory ()
	{
		SHLog.Debug ("SendToServerShowHistory");
		ServerState.Instance.IsShowingHistory = true;
		Messenger.Send ("OnShowHistoryRequested");
	}
	
	[RPC]
	public void SendToServerShowBuilds ()
	{
		SHLog.Debug ("SendToServerShowBuilds");
		ServerState.Instance.IsShowingHistory = false;
		Messenger.Send ("OnShowBuildsRequested");
	}
	
	
	[RPC]
	public void SendToServerZoomIn ()
	{
		SHLog.Debug ("SendToServerZoomIn");
		ServerState.Instance.CameraPositionZ = Camera.main.transform.position.z;
		Messenger.Send ("OnZoomIn");
	}
	
	[RPC]
	public void SendToServerZoomOut ()
	{
		SHLog.Debug ("SendToServerZoomOut");
		ServerState.Instance.CameraPositionZ = Camera.main.transform.position.z;
		Messenger.Send ("OnZoomOut");
	}
	
	[RPC]
	public void SendToServerGoLeft ()
	{
		SHLog.Debug ("SendToServerGoLeft");
		Messenger.Send ("OnGoLeft");
	}
	
	[RPC]
	public void SendToServerGoRight ()
	{
		SHLog.Debug ("SendToServerGoRight");
		Messenger.Send ("OnGoRight");
	}
	
	[RPC]
	public void SendToServerGoUp()
	{
		SHLog.Debug ("SendToServerGoUp");
		Messenger.Send ("OnGoUp");
	}
	
	[RPC]
	public void SendToServerGoDown ()
	{
		SHLog.Debug ("SendToServerGoDown");
		Messenger.Send ("OnGoDown");
	}
	
	[RPC]
	public void SendToServerResetCamera()
	{
		SHLog.Debug ("SendToServerResetCamera");
		Messenger.Send ("OnResetCamera");
	}
	
	[RPC]
	public void ShowSuccessBuilds (bool show)
	{
		SHLog.Debug ("ShowSuccessBuilds:" + show);
		ServerState.Instance.BuildFilter.SuccessEnabled = show;
		SendFilterLocally();
	}
	
	[RPC]
	public void ShowRunningBuilds (bool show)
	{
		SHLog.Debug ("ShowRunningBuilds:" + show);
		ServerState.Instance.BuildFilter.RunningEnabled = show;
		SendFilterLocally ();	
	}
	
	[RPC]
	public void ShowFailedBuilds(bool show)
	{
		SHLog.Debug("ShowFailedBuilds:" + show);
		ServerState.Instance.BuildFilter.FailedEnabled = show;
		SendFilterLocally();	
	}
	
	[RPC]
	public void ShowQueuedBuilds(bool show)
	{
		SHLog.Debug("ShowQueuedBuilds:" + show);
		ServerState.Instance.BuildFilter.QueuedEnabled = show;
		SendFilterLocally();	
	}
	
	[RPC]
	public void ShowBuildsWithName (string partialName)
	{
		if (!EasterEggService.ReceiveEasterEgg (partialName)) {
			SHLog.Debug ("ShowBuildsWithName:" + partialName);
			ServerState.Instance.BuildFilter.KeyWord = partialName;
			SendFilterLocally ();	
		}
	}
	
	[RPC]
	public void RunBuild ()
	{
		SHLog.Debug ("RunBuild");
		Messenger.Send ("OnBuildRunRequested");	
	}
	
	[RPC]
	public void StopBuild ()
	{
		SHLog.Debug ("StopBuild");
		Messenger.Send ("OnBuildStopRequested");	
	}
	#endregion
	
	#region Messages to remote control
	[RPC]
	public void SendToRCAuthenticationSuccessful ()
	{
		GetComponent<NetworkView>().RPC ("SendToRCAuthenticationSuccessful", RPCMode.Others);
	}
	
	[RPC]
	public void SendToRCAuthenticationFailed ()
	{
		GetComponent<NetworkView>().RPC ("SendToRCAuthenticationFailed", RPCMode.Others);
	}
	
	[RPC]
	public void SendToRCScreenshot (int width, int height, byte[] bytes)
	{
		GetComponent<NetworkView>().RPC ("SendToRCScreenshot", RPCMode.Others, width, height, bytes);
	}
	
	[RPC]
	public void SendToRCSortingEnded ()
	{
		GetComponent<NetworkView>().RPC ("SendToRCSortingEnded", RPCMode.Others);		
	}
		
	private void SendToRCCurrentServerState ()
	{
		SendToRCServerState (SHSerializer.SerializeToBytes (ServerState.Instance));
	}
	
	[RPC]
	public void SendToRCServerState (byte[] serverStateSerialized)
	{
		GetComponent<NetworkView>().RPC ("SendToRCServerState", RPCMode.Others, serverStateSerialized);
	}
	
	[RPC]
	public void OnVisibleBuildsCount (int visibleBuildsCount)
	{
		GetComponent<NetworkView>().RPC ("OnVisibleBuildsCount", RPCMode.Others, visibleBuildsCount);
	}
	#endregion
	
	#region Messages send locally
	private void SendFilterLocally ()
	{
		Messenger.Send ("OnBuildFilterUpdated");
		BuildFilterUpdated.Raise(this);
	}
	#endregion
}