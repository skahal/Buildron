using System;
using System.Linq;
using Skahal.Common;
using Skahal.Infrastructure.Framework.Repositories;
using Buildron.Domain.Builds;
using Buildron.Domain.Users;
using Skahal.Threading;

namespace Buildron.Domain.CIServers
{
	/// <summary>
	/// Continuous Integration server service.
	/// </summary>
	public class CIServerService : ICIServerService
	{
		#region Events
		/// <summary>
		/// Occurs when continuous integration server is connected.
		/// </summary>
		public event EventHandler<CIServerConnectedEventArgs> CIServerConnected;

		/// <summary>
		/// Occurs when continuous integration server status has changed.
		/// </summary>
		public event EventHandler<CIServerStatusChangedEventArgs> CIServerStatusChanged;
		#endregion

		#region Fields
		private readonly IRepository<ICIServer> m_repository;
		private readonly IAsyncActionProvider m_asyncActionProvider;
		private IAsyncAction m_isDownAsyncAction;
		private ICIServer m_currentServer;
		private IBuildsProvider m_buildsProvider;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.CIServers.CIServerService"/> class.
		/// </summary>
		/// <param name="repository">Repository.</param>
		/// <param name="asyncActionProvider">Async action provider.</param>
		public CIServerService (IRepository<ICIServer> repository, IAsyncActionProvider asyncActionProvider)
		{
			m_repository = repository;
			m_asyncActionProvider = asyncActionProvider;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets a value indicating whether this <see cref="Buildron.Domain.CIServers.CIServerService"/> is initialized.
		/// </summary>
		/// <value><c>true</c> if initialized; otherwise, <c>false</c>.</value>
		public bool Initialized
		{
			get;
			private set;
		}
		#endregion

		#region Methods

		/// <summary>
		/// Initialize the service.
		/// </summary>
		/// <param name="buildsProvider">Builds provider.</param>
		public void Initialize (IBuildsProvider buildsProvider)
		{
			m_buildsProvider = buildsProvider;
			var ciServer = GetCIServer ();

			m_buildsProvider.ServerDown += delegate
			{
				if (m_isDownAsyncAction == null && ciServer.Status != CIServerStatus.Down)
				{
					// Wait a new refresh to check if server is really down.
					m_isDownAsyncAction = m_asyncActionProvider.Start (
						ciServer.RefreshSeconds * 0.5f,
						() =>
						{
							// Still down? Change server status and raise event.
							ciServer.Status = CIServerStatus.Down;
							CIServerStatusChanged.Raise (this, new CIServerStatusChangedEventArgs (ciServer));
						});
				}      
			};

			m_buildsProvider.ServerUp += delegate
			{
				if (m_isDownAsyncAction != null)
				{
					m_isDownAsyncAction.Cancel ();
					m_isDownAsyncAction = null;
				}

				if (ciServer.Status != CIServerStatus.Up)
				{
					ciServer.Status = CIServerStatus.Up;
					CIServerStatusChanged.Raise (this, new CIServerStatusChangedEventArgs (ciServer));
				}
			};

			m_buildsProvider.UserAuthenticationCompleted += (sender, e) => 
			{
				if (e.Success) {
					Initialized = true;
					CIServerConnected.Raise(this, new CIServerConnectedEventArgs(ciServer));

					ciServer.Status = CIServerStatus.Up;
					CIServerStatusChanged.Raise (this, new CIServerStatusChangedEventArgs (ciServer));
				}
			};
		}

		/// <summary>
		/// Authenticates the user.
		/// </summary>
		/// <param name="user">User.</param>
		public void AuthenticateUser (IAuthUser user)
		{
			if (m_buildsProvider != null)
			{
				m_buildsProvider.AuthenticateUser (user);
			}
		}

		/// <summary>
		/// Saves the CI server.
		/// </summary>
		/// <param name="server">Server.</param>
		public void SaveCIServer (ICIServer server)
		{
			var oldServer = GetCIServer ();

			if (oldServer.IsNew)
			{
				m_repository.Create (server);
			} else
			{
				server.Id = oldServer.Id;
				m_repository.Modify (server);
			}

			m_currentServer = server;
		}

		/// <summary>
		/// Gets the CI server.
		/// </summary>
		/// <returns>The CI server.</returns>
		public ICIServer GetCIServer ()
		{
			if (m_currentServer == null)
			{
				m_currentServer = m_repository.All ().FirstOrDefault () ?? new CIServer ();

				// Always starts as down.
				m_currentServer.Status = CIServerStatus.Down;
			}

			return m_currentServer;
		}

		#endregion
	}
}