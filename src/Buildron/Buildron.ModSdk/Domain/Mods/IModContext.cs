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
		event EventHandler<BuildFoundEventArgs> BuildFound;
		event EventHandler<BuildRemovedEventArgs> BuildRemoved;
		event EventHandler<BuildUpdatedEventArgs> BuildUpdated;    
		event EventHandler<BuildStatusChangedEventArgs> BuildStatusChanged;
		event EventHandler<BuildTriggeredByChangedEventArgs> BuildTriggeredByChanged;
		event EventHandler<BuildsRefreshedEventArgs> BuildsRefreshed;    
		event EventHandler<CIServerStatusChangedEventArgs> CIServerStatusChanged;
		event EventHandler<UserFoundEventArgs> UserFound;
		event EventHandler<UserUpdatedEventArgs> UserUpdated;
		event EventHandler<UserTriggeredBuildEventArgs> UserTriggeredBuild;
		event EventHandler<UserRemovedEventArgs> UserRemoved;
		event EventHandler<UserAuthenticationCompletedEventArgs> UserAuthenticationCompleted;
		event EventHandler<RemoteControlChangedEventArgs> RemoteControlChanged;
        #endregion
      
        #region Properties
        IList<IBuild> Builds { get; }

        IList<IUser> Users { get; }

        ISHLogStrategy Log { get; }

		IAssetsLoader AssetsLoader { get; }
        #endregion
    }
}
