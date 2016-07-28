using Buildron.Domain;
using Buildron.Domain.EasterEggs;
using Buildron.Domain.Sorting;
using Skahal.Common;
using Skahal.Logging;
using Skahal.Serialization;
using UnityEngine;
using Zenject;
using Buildron.Domain.Builds;
using Buildron.Domain.Users;
using Buildron.Domain.RemoteControls;
using Buildron.Domain.Servers;
using Skahal.Threading;

namespace Buildron.Controllers
{
	[RequireComponent (typeof(NetworkView))]
	public class RemoteControlController : MonoBehaviour, IInitializable, IRemoteControlMessagesListener
	{
		#region Events
		public event System.EventHandler<BuildFilterUpdatedEventArgs> BuildFilterUpdated;
		public event System.EventHandler<BuildSortUpdatedEventArgs> BuildSortUpdated;
		#endregion

		#region Fields
		[Inject]
		private IUserService m_userService;

		[Inject]
		private IBuildService m_buildService;

		[Inject]
		private EasterEggService m_easterEggService;

		[Inject]
		private IRemoteControlService m_remoteControlService;

		[Inject]
		private IServerService m_serverService;

		private ServerState m_serverState;
		#endregion

		#region Life cycle
		private void Awake ()
		{
			var error = Network.InitializeServer (1, 8181, false);
	
			Messenger.Register (
				gameObject, 
				"OnScreenshotTaken");

			SHLog.Warning ("Network server initialize: {0}", error);
		}

		public void Initialize ()
		{
			m_serverState = m_serverService.GetState ();

			m_userService.UserAuthenticationCompleted += (sender, args) =>
			{
				if (args.Success)
				{
					SendToRCAuthenticationSuccessful ();
				} else
				{
					SendToRCAuthenticationFailed ();
				}
			};

			m_buildService.BuildFound += (sender, e) => {
				OnVisibleBuildsCount (m_buildService.Builds.Count);
			};

			m_buildService.BuildRemoved += (sender, e) => {
				OnVisibleBuildsCount (m_buildService.Builds.Count);
			};

			SendFilterLocally ();

			SHLog.Warning ("ServerMessagesListener initialized");
		}

		private void OnPlayerConnected ()
		{
			SHLog.Debug ("Remote control connected.");	
		
			// TODO: remove when migrate RC to open source.
			m_serverState.HasHistory = GameObject.FindGameObjectsWithTag("History").Length > 0;
			m_serverState.IsShowingHistory = false;
			m_serverService.SaveState (m_serverState);

			//
			SendToRCCurrentServerState ();

			OnVisibleBuildsCount (m_buildService.Builds.Count);
		}

		private void OnPlayerDisconnected ()
		{
			SHLog.Debug ("Remote control disconnected.");
			m_remoteControlService.DisconnectRemoteControl ();
			Messenger.Send ("OnRemoteControlDisconnected");	
		}

		private void OnScreenshotTaken (Texture2D texture)
		{
			SendToRCScreenshot (texture.width, texture.height, texture.EncodeToPNG ());
		}
		#endregion

		#region Messages from remote control

		[RPC]
		public void SendToServerAuthentication (string userName, string password)
		{
			SHLog.Debug ("SendToServerAuthentication:" + userName);
		
			var rc = new RemoteControl {
				UserName = userName,
				Password = password
			};
					
			m_remoteControlService.ConnectRemoteControl (rc);
			Messenger.Send ("OnRemoteControlConnected", rc);
		}

		[RPC]
		public void SendToServerTakeScreenshot ()
		{
			SHLog.Debug ("SendToServerTakeScreenshot");	
			Messenger.Send ("OnScreenshotRequested");
			m_remoteControlService.ReceiveCommand (new TakeScreenshotRemoteControlCommand ());
		}

		[RPC]
		public void SendToServerSortBuilds (int sortingAlgorithmType, int sortBy)
		{
			var sortingType = (SortingAlgorithmType)sortingAlgorithmType;
			var sortByProperty = (SortBy)sortBy;
			SHLog.Debug ("SendToServerSortBuilds: {0}, {1}", sortingType, sortByProperty);

			// TODO: args.SortingAlgorithm is ignored because RC is not passing this at this time.            
			var sorting = SortingAlgorithmFactory.CreateRandomSortingAlgorithm<IBuild> ();	
			var args = new BuildSortUpdatedEventArgs (sorting, sortByProperty);
			sortingType = SortingAlgorithmFactory.GetAlgorithmType (sorting);

			sorting.SortingBegin += (sender, e) =>
			{
				m_serverState.IsSorting = true;
				SendToRCCurrentServerState ();
			};

			sorting.SortingEnded += (sender, e) =>
			{
				m_serverState.IsSorting = false;
				SendToRCSortingEnded ();
				SendToRCCurrentServerState ();
			};

			SHLog.Warning("BuildSortUpdated: {0} {1}", sortingType, sortByProperty);			
			m_serverState.BuildSortingAlgorithmType = sortingType;
			m_serverState.BuildSortBy = sortByProperty;
			m_serverService.SaveState(m_serverState);

			Messenger.Send ("OnBuildSortUpdated", args);

			if (BuildSortUpdated != null)
			{
				BuildSortUpdated (this, args);
			}
		}

		[RPC]
		public void SendToServerShowHistory ()
		{
			m_serverState.IsShowingHistory = true;
			// TODO: when we port the RC to open source, we should programming some support do Mods register custom commands
			// on RC.
			m_remoteControlService.ReceiveCommand(new CustomRemoteControlCommand("ShowHistory"));
		}

		[RPC]
		public void SendToServerShowBuilds ()
		{
			m_serverState.IsShowingHistory = false;
			m_remoteControlService.ReceiveCommand(new CustomRemoteControlCommand("ShowBuilds"));
		}

		[RPC]
		public void SendToServerZoomIn ()
		{
			m_remoteControlService.ReceiveCommand (new MoveCameraRemoteControlCommand (Vector3.forward));
		}

		[RPC]
		public void SendToServerZoomOut ()
		{
			m_remoteControlService.ReceiveCommand (new MoveCameraRemoteControlCommand (Vector3.back));
		}

		[RPC]
		public void SendToServerGoLeft ()
		{
			m_remoteControlService.ReceiveCommand (new MoveCameraRemoteControlCommand (Vector3.left));
		}

		[RPC]
		public void SendToServerGoRight ()
		{
			m_remoteControlService.ReceiveCommand (new MoveCameraRemoteControlCommand (Vector3.right));
		}

		[RPC]
		public void SendToServerGoUp ()
		{
			m_remoteControlService.ReceiveCommand (new MoveCameraRemoteControlCommand (Vector3.up));
		}

		[RPC]
		public void SendToServerGoDown ()
		{
			m_remoteControlService.ReceiveCommand (new MoveCameraRemoteControlCommand (Vector3.down));
		}

		[RPC]
		public void SendToServerResetCamera ()
		{
			m_remoteControlService.ReceiveCommand (new ResetCameraRemoteControlCommand ());
		}

		[RPC]
		public void ShowSuccessBuilds (bool show)
		{
			SHLog.Debug ("ShowSuccessBuilds:" + show);
			m_serverState.BuildFilter.SuccessEnabled = show;
			SendFilterLocally ();
		}

		[RPC]
		public void ShowRunningBuilds (bool show)
		{
			SHLog.Debug ("ShowRunningBuilds:" + show);
			m_serverState.BuildFilter.RunningEnabled = show;
			SendFilterLocally ();	
		}

		[RPC]
		public void ShowFailedBuilds (bool show)
		{
			SHLog.Debug ("ShowFailedBuilds:" + show);
			m_serverState.BuildFilter.FailedEnabled = show;
			SendFilterLocally ();	
		}

		[RPC]
		public void ShowQueuedBuilds (bool show)
		{
			SHLog.Debug ("ShowQueuedBuilds:" + show);
			m_serverState.BuildFilter.QueuedEnabled = show;
			SendFilterLocally ();	
		}

		[RPC]
		public void ShowBuildsWithName (string partialName)
		{
			if (m_easterEggService.IsEasterEggMessage (partialName))
			{
				m_easterEggService.ReceiveEasterEgg (partialName);
			} else
			{
				SHLog.Debug ("ShowBuildsWithName:" + partialName);
				m_serverState.BuildFilter.KeyWord = partialName;
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
			GetComponent<NetworkView> ().RPC ("SendToRCAuthenticationSuccessful", RPCMode.Others);
		}

		[RPC]
		public void SendToRCAuthenticationFailed ()
		{
			GetComponent<NetworkView> ().RPC ("SendToRCAuthenticationFailed", RPCMode.Others);
		}

		[RPC]
		public void SendToRCScreenshot (int width, int height, byte[] bytes)
		{
			GetComponent<NetworkView> ().RPC ("SendToRCScreenshot", RPCMode.Others, width, height, bytes);
		}

		[RPC]
		public void SendToRCSortingEnded ()
		{
			GetComponent<NetworkView> ().RPC ("SendToRCSortingEnded", RPCMode.Others);		
		}

		private void SendToRCCurrentServerState ()
		{
			SendToRCServerState (SHSerializer.SerializeToBytes (m_serverState));
		}

		[RPC]
		public void SendToRCServerState (byte[] serverStateSerialized)
		{
			GetComponent<NetworkView> ().RPC ("SendToRCServerState", RPCMode.Others, serverStateSerialized);
		}

		[RPC]
		public void OnVisibleBuildsCount (int visibleBuildsCount)
		{
			GetComponent<NetworkView> ().RPC ("OnVisibleBuildsCount", RPCMode.Others, visibleBuildsCount);
		}
		#endregion
		#region Messages send locally

		private void SendFilterLocally ()
		{
			Messenger.Send ("OnBuildFilterUpdated");
			m_serverService.SaveState (m_serverState);
			BuildFilterUpdated.Raise (this, new BuildFilterUpdatedEventArgs(m_serverState.BuildFilter));
		}
		#endregion

		/// <summary>
		/// The ServerMessagesListener factory.
		/// </summary>
		public class Factory : Factory<RemoteControlController>
		{
		}
	}
}