using System;
using System.Linq;
using Skahal.Common;
using Skahal.Infrastructure.Framework.Repositories;

namespace Buildron.Domain
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