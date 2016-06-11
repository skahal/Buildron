//using UnityEngine;
//using System.Collections;
//using System;
//using Buildron.Domain;
//using System.Collections.Generic;
//
//namespace Skahal.Domain.Mods
//{
//	/// <summary>
//	/// Represents the mod context.
//	/// </summary>
//	public class ModContext
//	{    
//		#region Events
//		public event EventHandler<BuildFoundEventArgs> BuildFound;
//		public event EventHandler<BuildRemovedEventArgs> BuildRemoved;
//		public event EventHandler<BuildUpdatedEventArgs> BuildUpdated;    
//		public event EventhHandler<BuildStatusChangedEventArgs> BuildStatusChanged;
//		public event EventhHandler<BuildTriggeredByChangedEventArgs> BuildTriggeredByChanged;
//		public event EventHandler<BuildsRefreshedEventArgs> BuildsRefreshed;    
//		public event EventHandler<CIServerUpEventArgs> CIServerUp;
//		public event EventHandler<CIServerDownEventArgs> CIServerDown;
//		public event EventHandler<UserFoundEventArgs> UserFound;
//		public event EventHandler<UserUpdatedEventArgs> UserUpdated;
//		public event EventHandler<UserTriggeredBuildEventArgs> UserTriggeredBuild;
//		public event EventHandler<UserRemovedEventArgs> UserRemoved;
//		public event EventHandler<UserAuthenticationSuccessfulEventArgs> UserAuthenticationSuccessful;
//		public event EventHandler<UserAuthenticationFailedEventArgs> UserAuthenticationFailed;  
//		public event EventHandler<RemoteControlConnectedEventArgs> RemoteControlConnected;  
//		public event EventHandler<RemoteControlDisconnectedEventArgs> RemoteControlDisconnected;  
//		#endregion
//
//		#region Properties
//		public IEnumerable<Build> Builds { get; private set;}
//		public Camera Camera { get; private set; }      
//		#endregion
//	}
//}
