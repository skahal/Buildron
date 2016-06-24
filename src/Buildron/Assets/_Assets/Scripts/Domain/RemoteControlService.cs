using System;
using Skahal.Common;
using Skahal.Infrastructure.Framework.Repositories;
using UnityEngine;
using Zenject;
using System.Linq;

namespace Buildron.Domain
{
    /// <summary>
    /// Remote control service.
    /// </summary>
    public class RemoteControlService : IRemoteControlService, IInitializable
    {
        #region Events
        public event EventHandler<RemoteControlChangedEventArgs> RemoteControlChanged;
        #endregion

        #region Fields
        private readonly ICIServerService m_ciServerService;
        private readonly IUserService m_userService;
        private readonly IRepository<RemoteControl> m_repository;
        private RemoteControl m_connectedRC;
        private bool? m_hasRemoteControlConnectedSomeDay;
        #endregion

        #region Constructors
        public RemoteControlService(ICIServerService ciServerService, IUserService userService, IRepository<RemoteControl> repository)
        {
            m_ciServerService = ciServerService;
            m_userService = userService;
            m_repository = repository;
        }
        #endregion

        #region Properties
        public bool HasRemoteControlConnectedSomeDay
        {
		    get
            {
                if (!m_hasRemoteControlConnectedSomeDay.HasValue)
                {
                    m_hasRemoteControlConnectedSomeDay = m_repository.All().Any();
                }

                return m_hasRemoteControlConnectedSomeDay.Value;
            }	
		}
		
		public bool HasRemoteControlConnected
		{
			get {
				return m_connectedRC != null;
			}
		}
        #endregion

        #region Methods
        public void Initialize()
        {
            m_userService.UserAuthenticationCompleted += (sender, args) => {
                if (m_connectedRC != null)
                {
                    if (args.Success)
                    {
                        if (!m_repository.All().Any(r => r == m_connectedRC))
                        {
                            m_repository.Create(m_connectedRC);
                        }
                    }
                    else
                    {
                        m_connectedRC.Connected = false;
                        m_connectedRC = null;
                    }
                }
            };
        }

        public void ConnectRemoteControl (RemoteControl rcToConnect)
		{
			m_connectedRC = rcToConnect;
            m_connectedRC.Connected = true;
            m_ciServerService.AuthenticateUser (rcToConnect);
            m_hasRemoteControlConnectedSomeDay = true;

            RemoteControlChanged.Raise(typeof(RemoteControlService), new RemoteControlChangedEventArgs(m_connectedRC));
        }
		
		public void DisconnectRemoteControl ()
		{
            m_connectedRC.Connected = false;
            RemoteControlChanged.Raise(typeof(RemoteControlService), new RemoteControlChangedEventArgs(m_connectedRC));
            m_connectedRC = null;
        }
		
		public RemoteControl GetConnectedRemoteControl ()
		{
			return m_connectedRC;
		}
		#endregion
	}
}