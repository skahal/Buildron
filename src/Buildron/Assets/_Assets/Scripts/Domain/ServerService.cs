#region Usings
using UnityEngine;
using Buildron.Domain;
using Skahal.Infrastructure.Framework.Commons;
using System;
using System.Linq;
using Buildron.Infrastructure.Repositories;
using Skahal.Logging;
using Buildron.Domain.Sorting;


#endregion

/// <summary>
/// Server domain service.
/// </summary>
public static class ServerService
{
	#region Events
	public static event EventHandler Initialized;
	#endregion

	#region Fields
	private static IServerStateRepository s_repository;
	#endregion

	#region Properties
	public static ServerMessagesListener Listener { get; private set; }
    #endregion

    #region Methods
    /// <summary>
    /// Initializes this instance.
    /// </summary>
	public static void Initialize(ServerMessagesListener listener)
    {
		Listener = listener;
	
		s_repository = DependencyService.Create<IServerStateRepository>();
		var all = s_repository.All().ToList();
        var lastServerState = all.FirstOrDefault();

        if (lastServerState != null)
        {
			lastServerState.IsSorting = false;
            ServerState.Instance = lastServerState;
        }

		Listener.BuildFilterUpdated += (sender, e) => {
       		SaveState();
        };

		Listener.BuildSortUpdated += (sender, e) => {

            SHLog.Warning("BuildSortUpdated: {0} {1}", e.SortingAlgorithm, e.SortBy);			
			ServerState.Instance.BuildSortingAlgorithmType = SortingAlgorithmFactory.GetAlgorithmType(e.SortingAlgorithm);
            ServerState.Instance.BuildSortBy = e.SortBy;
            SaveState();
		};        

        if (Initialized != null)
        {
            Initialized(typeof(ServerService), EventArgs.Empty);
        }
    }

	public static void SaveState()
	{
		var serverState = ServerState.Instance;

		if (serverState.Id == 0)
		{
			SHLog.Debug("Creating a new ServerState:");
			s_repository.Create(serverState);
		}
		else 
		{
			SHLog.Debug("Updating an current ServerState:");
			s_repository.Modify(serverState);
		}
	}
    #endregion
}