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
		/// Occurs when build found.
		/// </summary>
		event EventHandler<BuildFoundEventArgs> BuildFound;

		/// <summary>
		/// Occurs when build removed.
		/// </summary>
		event EventHandler<BuildRemovedEventArgs> BuildRemoved;

		/// <summary>
		/// Occurs when build updated.
		/// </summary>
		event EventHandler<BuildUpdatedEventArgs> BuildUpdated;

		/// <summary>
		/// Occurs when build status changed.
		/// </summary>
		event EventHandler<BuildStatusChangedEventArgs> BuildStatusChanged;

		/// <summary>
		/// Occurs when build triggered by changed.
		/// </summary>
		event EventHandler<BuildTriggeredByChangedEventArgs> BuildTriggeredByChanged;

		/// <summary>
		/// Occurs when builds refreshed.
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
		/// Occurs when user found.
		/// </summary>
		event EventHandler<UserFoundEventArgs> UserFound;

		/// <summary>
		/// Occurs when user updated.
		/// </summary>
		event EventHandler<UserUpdatedEventArgs> UserUpdated;

		/// <summary>
		/// Occurs when user triggered build.
		/// </summary>
		event EventHandler<UserTriggeredBuildEventArgs> UserTriggeredBuild;

		/// <summary>
		/// Occurs when user removed.
		/// </summary>
		event EventHandler<UserRemovedEventArgs> UserRemoved;

		/// <summary>
		/// Occurs when user authentication completed.
		/// </summary>
		event EventHandler<UserAuthenticationCompletedEventArgs> UserAuthenticationCompleted;

		/// <summary>
		/// Occurs when remote control changed.
		/// </summary>
		event EventHandler<RemoteControlChangedEventArgs> RemoteControlChanged;

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

        IGameObjectsPoolProxy GameObjectsPool { get; }

		/// <summary>
		/// Gets the UI.
		/// </summary>
		/// <value>The U.</value>
		IUIProxy UI { get; } 

        IFileSystemProxy FileSystem { get; }

		IDataProxy Data { get; }

		IBuildGameObjectsProxy BuildGameObjects { get; }

		IUserGameObjectsProxy UserGameObjects { get; }

        ICameraProxy Camera { get; }

		IPreferencesProxy Preferences { get; }
        #endregion
    }
}
