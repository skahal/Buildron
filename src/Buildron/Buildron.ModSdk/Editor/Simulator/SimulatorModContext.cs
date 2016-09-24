using System;
using System.Collections.Generic;
using System.Linq;
using Buildron.Domain.Builds;
using Buildron.Domain.CIServers;
using Buildron.Domain.Mods;
using Buildron.Domain.RemoteControls;
using Buildron.Domain.Users;
using Buildron.Infrastructure.AssetsProxies;
using Buildron.Infrastructure.BuildGameObjectsProxies;
using Buildron.Infrastructure.CameraProxies;
using Buildron.Infrastructure.GameObjectsProxies;
using Buildron.Infrastructure.UserGameObjectsProxies;
using Skahal.Common;
using Skahal.Logging;
using UnityEngine;
using Buildron.Infrastructure.PreferencesProxies;

/// <summary>
/// Simulator mod context.
/// </summary>
public class SimulatorModContext : MonoBehaviour, IModContext {

	#region Events
	/// <summary>
	/// Occurs when a build is found.
	/// </summary>
	public event EventHandler<BuildFoundEventArgs> BuildFound;

	/// <summary>
	/// Occurs when a build is removed.
	/// </summary>
	public event EventHandler<BuildRemovedEventArgs> BuildRemoved;

	/// <summary>
	/// Occurs when a build is updated.
	/// </summary>
	public event EventHandler<BuildUpdatedEventArgs> BuildUpdated;

	/// <summary>
	/// Occurs when build status changed.
	/// </summary>
	public event EventHandler<BuildStatusChangedEventArgs> BuildStatusChanged;

	/// <summary>
	/// Occurs when build's triggered by changed.
	/// </summary>
	public event EventHandler<BuildTriggeredByChangedEventArgs> BuildTriggeredByChanged;

	/// <summary>
	/// Occurs when builds are refreshed.
	/// </summary>
	public event EventHandler<BuildsRefreshedEventArgs> BuildsRefreshed;

	/// <summary>
	/// Occurs when CI server is connected.
	/// </summary>
	public event EventHandler<CIServerConnectedEventArgs> CIServerConnected;

	/// <summary>
	/// Occurs when CI server status changed.
	/// </summary>
	public event EventHandler<CIServerStatusChangedEventArgs> CIServerStatusChanged;

	/// <summary>
	/// Occurs when an user is found.
	/// </summary>
	public event EventHandler<UserFoundEventArgs> UserFound;

	/// <summary>
	/// Occurs when an user is updated.
	/// </summary>
	public event EventHandler<UserUpdatedEventArgs> UserUpdated;

	/// <summary>
	/// Occurs when an user triggered build.
	/// </summary>
	public event EventHandler<UserTriggeredBuildEventArgs> UserTriggeredBuild;

	/// <summary>
	/// Occurs when an user is removed.
	/// </summary>
	public event EventHandler<UserRemovedEventArgs> UserRemoved;

	/// <summary>
	/// Occurs when an user authentication is completed.
	/// </summary>
	public event EventHandler<UserAuthenticationCompletedEventArgs> UserAuthenticationCompleted;

	/// <summary>
	/// Occurs when a remote control changed.
	/// </summary>
	public event EventHandler<RemoteControlChangedEventArgs> RemoteControlChanged;

	/// <summary>
	/// Occurs when a remote control command is received.
	/// </summary>
	public event EventHandler<RemoteControlCommandReceivedEventArgs> RemoteControlCommandReceived;
    #endregion

    #region Properties
	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static SimulatorModContext Instance { get; private set; }

	/// <summary>
	/// Gets the builds.
	/// </summary>
	/// <value>The builds.</value>
    public IList<IBuild> Builds { get; private set; }

	/// <summary>
	/// Gets the users.
	/// </summary>
	/// <value>The users.</value>
	public IList<IUser> Users { get; private set; }

	/// <summary>
	/// Gets the CIS erver.
	/// </summary>
	/// <value>The CIS erver.</value>
	public ICIServer CIServer { get; private set; }

	/// <summary>
	/// Gets the log.
	/// </summary>
	/// <value>The log.</value>
	public ISHLogStrategy Log { get; private set; }

	/// <summary>
	/// Gets the assets.
	/// </summary>
	/// <value>The assets.</value>
	public IAssetsProxy Assets  { get; private set; }

	/// <summary>
	/// Gets the game objects.
	/// </summary>
	/// <value>The game objects.</value>
	public IGameObjectsProxy GameObjects { get; private set; }

	/// <summary>
	/// Gets the game objects pool.
	/// </summary>
	/// <value>The game objects pool.</value>
	public IGameObjectsPoolProxy GameObjectsPool { get; private set; }

	/// <summary>
	/// Gets the user interface.
	/// </summary>
	/// <value>The user interface.</value>
	public IUIProxy UI { get; private set; } 

	/// <summary>
	/// Gets the file system.
	/// </summary>
	/// <value>The file system.</value>
	public IFileSystemProxy FileSystem { get; private set; }

	/// <summary>
	/// Gets the data.
	/// </summary>
	/// <value>The data.</value>
	public IDataProxy Data { get; private set; }

	/// <summary>
	/// Gets the build game objects.
	/// </summary>
	/// <value>The build game objects.</value>
	public IBuildGameObjectsProxy BuildGameObjects { get; private set; }

	/// <summary>
	/// Gets the user game objects.
	/// </summary>
	/// <value>The user game objects.</value>
	public IUserGameObjectsProxy UserGameObjects { get; private set; }

	/// <summary>
	/// Gets the camera.
	/// </summary>
	/// <value>The camera.</value>
	public ICameraProxy Camera { get; private set; }

	/// <summary>
	/// Gets the preferences.
	/// </summary>
	/// <value>The preferences.</value>
	public IPreferencesProxy Preferences { get; private set; }

	public IRemoteControlProxy RemoteControl { get; private set; }
    #endregion

    #region Methods
    private void Start()
	{
		var modInterfaceType = typeof(IMod);
		var modType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(t => !t.IsAbstract && modInterfaceType.IsAssignableFrom(t));

		if (modType == null)
		{
			throw new InvalidOperationException("IMod interface implementation not found");
		}
		Log = new SHDebugLogStrategy ();
		var modInfo = new ModInfo (modType.Name);

		Camera = new ModCameraProxy (modInfo, UnityEngine.Camera.main);

		Instance = this;
		Builds = new List<IBuild> ();
		Users = new List<IUser> ();
		CIServer = SimulatorCIServer.Instance;

		Assets = new ResourcesFolderAssetsProxy ();
		GameObjects = new ModGameObjectsProxy ();
		GameObjectsPool = new ModGameObjectsPoolProxy (modInfo, GameObjects);
		FileSystem = new SimulatorFileSystemProxy ();
		Preferences = new ModPreferencesProxy(modInfo);
		BuildGameObjects = new ModBuildGameObjectsProxy ();
		UserGameObjects = new ModUserGameObjectsProxy ();
		RemoteControl = new SimulatorRemoteControlProxy();
	
		var mod = Activator.CreateInstance (modType) as IMod;
		mod.Initialize (this);
	}

	public void RaiseCIServerConnected ()
	{
		Log.Debug ("CIServerConnected");
		CIServerConnected.Raise (this, new CIServerConnectedEventArgs (CIServer));
	}

	public void RaiseCIServerStatusChanged()
	{
		Log.Debug("CIServerStatusChanged");
		CIServerStatusChanged.Raise(this, new CIServerStatusChangedEventArgs(CIServer));
	}

	public void RaiseBuildFound (SimulatorBuild build)
	{
		Builds.Add (build);
		Log.Debug ("BuildFound: {0}: {1}", build.Id, build.Status);
		BuildFound.Raise (this, new BuildFoundEventArgs (build));
	}

	public void RaiseBuildUpdated(SimulatorBuild build)
	{		
		Log.Debug("BuildUpdated: {0}: {1}", build.Id, build.Status);
		BuildUpdated.Raise(this, new BuildUpdatedEventArgs(build));
	}

	public void RaiseBuildRemoved (int buildIndex)
	{
		if (buildIndex < Builds.Count) {
			var build = Builds [buildIndex];

			Builds.RemoveAt (buildIndex);
			Log.Debug ("BuildRemoved: {0}: {1}", build.Id, build.Status);
			BuildRemoved.Raise (this, new BuildRemovedEventArgs (build));
		}
	}

	public void RaiseBuildStatusChanged(BuildStatus status)
	{
		var previousStatus = BuildStatus.Unknown;

		if (Builds.Count == 0)
		{
			RaiseBuildFound(new SimulatorBuild()
			{
				Status = BuildStatus.Unknown
			});
		}
		else {
			previousStatus = Builds[Builds.Count - 1].Status;
		}

		var build = Builds [Builds.Count - 1];
		build.Status = status;
		Log.Debug("BuildStatusChanged: {0}: {1}", build.Id, build.Status);
		BuildStatusChanged.Raise(this, new BuildStatusChangedEventArgs(build, previousStatus));
	}

	public void RaiseBuildTriggeredByChanged(SimulatorBuild build, SimulatorUser user)
	{
		Log.Debug("BuildTriggeredByChanged: {0}: {1}", build.Id, user.UserName);
		BuildTriggeredByChanged.Raise(this, new BuildTriggeredByChangedEventArgs(build, user));
	}

	public void RaiseBuildsRefreshed(IList<IBuild> buildsStatusChanged, IList<IBuild> buildsFound, IList<IBuild> buildsRemoved)
	{
		Log.Debug("BuildsRefreshed: {0}, {1}, {2}", buildsStatusChanged.Count, buildsFound.Count, buildsRemoved.Count);
		BuildsRefreshed.Raise(this, new BuildsRefreshedEventArgs(buildsStatusChanged, buildsFound, buildsRemoved));
	}

	public void RaiseUserFound(SimulatorUser user)
	{
		Log.Debug("RaiseUserFound: {0}", user.UserName);
		UserFound.Raise(this, new UserFoundEventArgs(user));
	}

	public void RaiseUserUpdated(SimulatorUser user)
	{
		Log.Debug("UserUpdated: {0}", user.UserName);
		UserUpdated.Raise(this, new UserUpdatedEventArgs(user));
	}

	public void RaiseUserRemoved(SimulatorUser user)
	{
		Log.Debug("UserRemoved: {0}", user.UserName);
		UserRemoved.Raise(this, new UserRemovedEventArgs(user));
	}

	public void RaiseUserAuthenticationCompleted(IAuthUser user, bool success)
	{
		Log.Debug("UserAuthenticationCompleted: {0}:{1}", user.UserName, success);
		UserAuthenticationCompleted.Raise(this, new UserAuthenticationCompletedEventArgs(user, success));
	}

	public void RaiseUserTriggeredBuild(SimulatorUser user, SimulatorBuild build)
	{
		Log.Debug("UserTriggeredBuild: {0}:{1}", user.UserName, build.Id);
		UserTriggeredBuild.Raise(this, new UserTriggeredBuildEventArgs(user, build));
	}

	public void RaiseRemoteControlChanged(IRemoteControlCommand cmd)
	{
		Log.Debug("RemoteControlChanged");
		var rc = new SimulatorRemoteControl();
		RemoteControlChanged.Raise(this, new RemoteControlChangedEventArgs(rc));
	}

	public void RaiseRemoteControlCommandReceived (IRemoteControlCommand cmd)
	{
		Log.Debug("RemoteControlCommandReceived: {0}", cmd);
		var rc = new SimulatorRemoteControl ();
		RemoteControlCommandReceived.Raise (this, new RemoteControlCommandReceivedEventArgs (rc, cmd));
	}
	#endregion
}
