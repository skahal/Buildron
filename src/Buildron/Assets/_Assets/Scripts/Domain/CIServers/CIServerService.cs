using System;
using System.Linq;
using Skahal.Common;
using Skahal.Infrastructure.Framework.Repositories;
using Buildron.Domain.Builds;
using Buildron.Domain.Users;

namespace Buildron.Domain.CIServers
{
    /// <summary>
    /// Continuous Integration server service.
    /// </summary>
    public class CIServerService : ICIServerService
    {        
        #region Events        
        /// <summary>
        /// Occurs when continuous integration server status has changed.
        /// </summary>
        public event EventHandler<CIServerStatusChangedEventArgs> CIServerStatusChanged;
        #endregion

        #region Fields
        private IRepository<CIServer> m_repository;
        private CIServer m_currentServer;
        private IBuildsProvider m_buildsProvider;
        #endregion

        #region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.CIServers.CIServerService"/> class.
		/// </summary>
		/// <param name="repository">Repository.</param>
        public CIServerService(IRepository<CIServer> repository)
        {
            m_repository = repository;           
        }
        #endregion

        /// <summary>
        /// Gets a value indicating whether this <see cref="Buildron.Domain.BuildService"/> is initialized.
        /// </summary>
        /// <value><c>true</c> if initialized; otherwise, <c>false</c>.</value>
        public bool Initialized
        {
            get;
            private set;
        }

        #region Methods
		/// <summary>
		/// Initialize the service.
		/// </summary>
		/// <param name="buildsProvider">Builds provider.</param>
        public void Initialize(IBuildsProvider buildsProvider)
        {
            m_buildsProvider = buildsProvider;
            var ciServer = GetCIServer();

            m_buildsProvider.ServerDown += delegate
            {
                ciServer.Status = CIServerStatus.Down;
                CIServerStatusChanged.Raise(this, new CIServerStatusChangedEventArgs(ciServer));
            };

            m_buildsProvider.ServerUp += delegate
            {
                ciServer.Status = CIServerStatus.Up;
                CIServerStatusChanged.Raise(this, new CIServerStatusChangedEventArgs(ciServer));
            };

            m_buildsProvider.UserAuthenticationSuccessful += delegate
            {
                Initialized = true;
                ciServer.Status = CIServerStatus.Up;
                CIServerStatusChanged.Raise(this, new CIServerStatusChangedEventArgs(ciServer));
            };
        }

        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <param name="user">User.</param>
        public void AuthenticateUser(UserBase user)
        {
            if (m_buildsProvider != null)
            {
                m_buildsProvider.AuthenticateUser(user);
            }
        }

		/// <summary>
		/// Saves the CI server.
		/// </summary>
		/// <param name="server">Server.</param>
        public void SaveCIServer (CIServer server)
		{
            var oldServer = GetCIServer();

            if (oldServer.IsNew)
            {
                m_repository.Create(server);
            }
            else
            {
                server.Id = oldServer.Id;
                m_repository.Modify(server);
            }

            m_currentServer = server;
        }

		/// <summary>
		/// Gets the CI server.
		/// </summary>
		/// <returns>The CI server.</returns>
		public CIServer GetCIServer ()
		{
            if (m_currentServer == null)
            {
                m_currentServer = m_repository.All().FirstOrDefault() ?? new CIServer();
            }

            return m_currentServer;
        }
		#endregion
	}
}