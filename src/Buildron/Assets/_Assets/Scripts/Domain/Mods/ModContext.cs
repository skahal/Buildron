using System;
using Buildron.Domain;
using System.Collections.Generic;
using Buildron.Domain.Builds;
using Buildron.Domain.Users;
using Buildron.Domain.CIServers;
using Buildron.Domain.RemoteControls;

namespace Skahal.Domain.Mods
{
	/// <summary>
	/// Represents the mod context.
    /// ModContext is, basically, a facade to many events that occurs in Buildron.
	/// </summary>
	public class ModContext
	{    
		#region Events
		public event EventHandler<BuildFoundEventArgs> BuildFound;
		public event EventHandler<BuildRemovedEventArgs> BuildRemoved;
		public event EventHandler<BuildUpdatedEventArgs> BuildUpdated;    
		public event EventHandler<BuildStatusChangedEventArgs> BuildStatusChanged;
		public event EventHandler<BuildTriggeredByChangedEventArgs> BuildTriggeredByChanged;
		public event EventHandler<BuildsRefreshedEventArgs> BuildsRefreshed;    
		public event EventHandler<CIServerStatusChangedEventArgs> CIServerStatusChanged;
		public event EventHandler<UserFoundEventArgs> UserFound;
		public event EventHandler<UserUpdatedEventArgs> UserUpdated;
		public event EventHandler<UserTriggeredBuildEventArgs> UserTriggeredBuild;
		public event EventHandler<UserRemovedEventArgs> UserRemoved;
		public event EventHandler<UserAuthenticationCompletedEventArgs> UserAuthenticationCompleted;
		public event EventHandler<RemoteControlChangedEventArgs> RemoteControlChanged;  
		#endregion

		#region Properties
		public IEnumerable<Build> Builds { get; private set;}
		//public Camera Camera { get; private set; }      
		#endregion
	}
}
