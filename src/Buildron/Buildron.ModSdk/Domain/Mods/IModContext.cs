using System;
using System.Collections.Generic;
using Buildron.Domain.Builds;
using Buildron.Domain.CIServers;
using Buildron.Domain.RemoteControls;
using Buildron.Domain.Users;
using Skahal.Logging;

namespace Buildron.Domain.Mods
{
    /// <summary>
    /// Defines an interface to the mod context.
    /// ModContext is basically a facade to many events that occurs in Buildron.
    /// </summary>
    public interface IModContext
	{    
		#region Events
		/// <summary>
		/// Occurs when a build is found.
		/// </summary>
		event EventHandler<BuildFoundEventArgs> BuildFound;

		/// <summary>
		/// Occurs when a build is removed.
		/// </summary>
		event EventHandler<BuildRemovedEventArgs> BuildRemoved;

		/// <summary>
		/// Occurs when a build is updated.
		/// </summary>
		event EventHandler<BuildUpdatedEventArgs> BuildUpdated;

		/// <summary>
		/// Occurs when a build status changed.
		/// </summary>
		event EventHandler<BuildStatusChangedEventArgs> BuildStatusChanged;

		/// <summary>
		/// Occurs when build's triggered by changed.
		/// </summary>
		event EventHandler<BuildTriggeredByChangedEventArgs> BuildTriggeredByChanged;

		/// <summary>
		/// Occurs when builds are refreshed.
		/// </summary>
		event EventHandler<BuildsRefreshedEventArgs> BuildsRefreshed; 

		/// <summary>
		/// Occurs when CI server is connected.
		/// </summary>
		event EventHandler<CIServerConnectedEventArgs> CIServerConnected;

		/// <summary>
		/// Occurs when CI server status changed.
		/// </summary>
		event EventHandler<CIServerStatusChangedEventArgs> CIServerStatusChanged;

		/// <summary>
		/// Occurs when an user is found.
		/// </summary>
		event EventHandler<UserFoundEventArgs> UserFound;

		/// <summary>
		/// Occurs when an user is updated.
		/// </summary>
		event EventHandler<UserUpdatedEventArgs> UserUpdated;

		/// <summary>
		/// Occurs when an user triggered build.
		/// </summary>
		event EventHandler<UserTriggeredBuildEventArgs> UserTriggeredBuild;

		/// <summary>
		/// Occurs when an user is removed.
		/// </summary>
		event EventHandler<UserRemovedEventArgs> UserRemoved;

		/// <summary>
		/// Occurs when an user authentication is completed.
		/// </summary>
		event EventHandler<UserAuthenticationCompletedEventArgs> UserAuthenticationCompleted;

		/// <summary>
		/// Occurs when a remote control changed.
		/// </summary>
		event EventHandler<RemoteControlChangedEventArgs> RemoteControlChanged;

		/// <summary>
		/// Occurs when a remote control command is received.
		/// </summary>
		event EventHandler<RemoteControlCommandReceivedEventArgs> RemoteControlCommandReceived;
        #endregion
      
        #region Properties
		/// <summary>
		/// Gets the builds.
		/// </summary>
		/// <value>The builds.</value>
        IList<IBuild> Builds { get; }

		/// <summary>
		/// Gets the users.
		/// </summary>
		/// <value>The users.</value>
        IList<IUser> Users { get; }

		/// <summary>
		/// Gets the CI server.
		/// </summary>
		/// <value>The CI server.</value>
		ICIServer CIServer { get; }

		/// <summary>
		/// Gets the log.
		/// </summary>
		/// <value>The log.</value>
        ISHLogStrategy Log { get; }

		/// <summary>
		/// Gets the assets.
		/// </summary>
		/// <value>The assets.</value>
		IAssetsProxy Assets { get; }

		/// <summary>
		/// Gets the game objects.
		/// </summary>
		/// <value>The game objects.</value>
		IGameObjectsProxy GameObjects { get; }

		/// <summary>
		/// Gets the game objects pool.
		/// </summary>
		/// <value>The game objects pool.</value>
        IGameObjectsPoolProxy GameObjectsPool { get; }

		/// <summary>
		/// Gets the UI.
		/// </summary>
		/// <value>The U.</value>
		IUIProxy UI { get; } 

		/// <summary>
		/// Gets the file system.
		/// </summary>
		/// <value>The file system.</value>
        IFileSystemProxy FileSystem { get; }

		/// <summary>
		/// Gets the data.
		/// </summary>
		/// <value>The data.</value>
		IDataProxy Data { get; }

		/// <summary>
		/// Gets the build game objects.
		/// </summary>
		/// <value>The build game objects.</value>
		IBuildGameObjectsProxy BuildGameObjects { get; }

		/// <summary>
		/// Gets the user game objects.
		/// </summary>
		/// <value>The user game objects.</value>
		IUserGameObjectsProxy UserGameObjects { get; }

		/// <summary>
		/// Gets the camera.
		/// </summary>
		/// <value>The camera.</value>
        ICameraProxy Camera { get; }

		/// <summary>
		/// Gets the preferences.
		/// </summary>
		/// <value>The preferences.</value>
		IPreferencesProxy Preferences { get; }
        #endregion
    }
}
