#region Usings
using UnityEngine;
using Buildron.Domain;
using Skahal.Infrastructure.Framework.Commons;
using System;
using System.Linq;
#endregion

/// <summary>
/// Server domain service.
/// </summary>
public static class ServerService
{
	#region Events
	public static event EventHandler Initialized;
	#endregion

	#region Properties
	public static ServerMessagesListener Listener { get; private set; }
	#endregion
	
	#region Methods
	public static void Initialize ()
	{
		Listener = new GameObject ("ServerMessagesListener").AddComponent<ServerMessagesListener> ();
		
		var buildFilterRepository = DependencyService.Create<IBuildFilterRepository> ();	
		var all = buildFilterRepository.All ().ToList();
		var lastBuildFilter = all.FirstOrDefault ();
		
		if (lastBuildFilter != null) {
			ServerState.Instance.BuildFilter = lastBuildFilter;
		}
		
		Listener.BuildFilterUpdated += delegate {
			var buildFilter = ServerState.Instance.BuildFilter;
			
			if (buildFilter.Id == 0) {
				buildFilterRepository.Create (buildFilter);
			} else {
				buildFilterRepository.Modify (buildFilter);
			}
		};

		if (Initialized != null) {
			Initialized (typeof(ServerService), EventArgs.Empty);
		}
	}
	#endregion
}