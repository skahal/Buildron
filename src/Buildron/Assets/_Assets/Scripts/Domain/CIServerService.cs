using System.Linq;
using UnityEngine;
using Buildron.Infrastructure.BuildsProvider.TeamCity;
using Skahal.Infrastructure.Framework.Repositories;

namespace Buildron.Domain
{
	public class CIServerService : ICIServerService
    {
        #region Fields
        private IRepository<CIServer> m_repository;
        private CIServer m_currentServer;
        #endregion

        #region Constructors
        public CIServerService(IRepository<CIServer> repository)
        {
            m_repository = repository;
        }
        #endregion

        #region Methods
        public void SaveCIServer (CIServer server)
		{
            var oldServer = GetCIServer();

            if (oldServer == null)
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
                m_currentServer = m_repository.All().FirstOrDefault();

                // Creates the default.
                if (m_currentServer == null)
                {
                    m_currentServer = new CIServer
                    {
                        ServerType = CIServerType.TeamCity,
                        Title = "Buildron",
                        IP = string.Empty,
                        UserName = string.Empty,
                        Domain = string.Empty,
                        Password = string.Empty,
                        RefreshSeconds = 10,
                        FxSoundsEnabled = true,
                        HistoryTotemEnabled = true,
                        BuildsTotemsNumber = 2
                    };
                }
            }

            return m_currentServer;
        }
		#endregion
	}
}