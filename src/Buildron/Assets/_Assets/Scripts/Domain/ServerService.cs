using System;
using System.Linq;
using Buildron.Domain;
using Buildron.Domain.Sorting;
using Skahal.Infrastructure.Framework.Commons;
using Skahal.Infrastructure.Framework.Repositories;
using Skahal.Logging;

namespace Buildron.Domain
{
	/// <summary>
	/// Server domain service.
	/// </summary>
	public class ServerService : IServerService
	{
		#region Fields
		private IRepository<ServerState> m_repository;
		private IRemoteControlMessagesListener m_rcListener;
		private ISHLogStrategy m_log;
	    #endregion

	    #region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ServerService"/> class.
		/// </summary>
		/// <param name="remoteControlMessagesListener">Remote control messages listener.</param>
		/// <param name="log">Log.</param>
		public ServerService(IRepository<ServerState> repository, IRemoteControlMessagesListener remoteControlMessagesListener, ISHLogStrategy log)
	    {
			m_rcListener = remoteControlMessagesListener;
			m_log = log;
		
			m_repository = repository;
			var all = m_repository.All().ToList();
	        var lastServerState = all.FirstOrDefault();

	        if (lastServerState != null)
	        {
				lastServerState.IsSorting = false;
	            ServerState.Instance = lastServerState;
	        }

			m_rcListener.BuildFilterUpdated += (sender, e) => {
	       		SaveState();
	        };

			m_rcListener.BuildSortUpdated += (sender, e) => {

	            m_log.Warning("BuildSortUpdated: {0} {1}", e.SortingAlgorithm, e.SortBy);			
				ServerState.Instance.BuildSortingAlgorithmType = SortingAlgorithmFactory.GetAlgorithmType(e.SortingAlgorithm);
	            ServerState.Instance.BuildSortBy = e.SortBy;
	            SaveState();
			};        
	    }
		#endregion

		#region Methods
		/// <summary>
		/// Saves the state.
		/// </summary>
		public void SaveState()
		{
			var serverState = ServerState.Instance;

			if (serverState.Id == 0)
			{
				m_log.Debug("Creating a new ServerState:");
				m_repository.Create(serverState);
			}
			else 
			{
				m_log.Debug("Updating an current ServerState:");
				m_repository.Modify(serverState);
			}
		}
	    #endregion
	}
}