using System;
using Buildron.Domain;
using UnityEngine;
using Skahal.Common;
using Zenject;
#region Usings
#endregion

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
        [Inject]
        private IBuildService m_buildService;

        [Inject]
        private IUserService m_userService;

        private RemoteControl m_remoteControl;
		#endregion	
		
		#region Properties
		public bool HasRemoteControlConnectedSomeDay {
			get {
				return PlayerPrefs.GetInt ("_HasRemoteControlConnectedSomeDay_", 0) == 1;
			}
			
			set {
				PlayerPrefs.SetInt ("_HasRemoteControlConnectedSomeDay_", value ? 1 : 0);
			}
		}
		
		public bool HasRemoteControlConnected
		{
			get {
				return m_remoteControl != null;
			}
		}
        #endregion

        #region Methods
        public void  Initialize()
        {
            m_userService.UserAuthenticationCompleted += (sender, args) => {
                if (!args.Success)
                {
                    m_remoteControl = null;
                }
            };
        }

        public void ConnectRemoteControl (RemoteControl rcToConnect)
		{
			m_remoteControl = rcToConnect;
            m_remoteControl.Connected = true;
            m_buildService.AuthenticateUser (rcToConnect);
			HasRemoteControlConnectedSomeDay = true;

            RemoteControlChanged.Raise(typeof(RemoteControlService), new RemoteControlChangedEventArgs(m_remoteControl));
        }
		
		public void DisconnectRemoteControl ()
		{
            m_remoteControl.Connected = false;
            RemoteControlChanged.Raise(typeof(RemoteControlService), new RemoteControlChangedEventArgs(m_remoteControl));
            m_remoteControl = null;
        }
		
		public RemoteControl GetConnectedRemoteControl ()
		{
			return m_remoteControl;
		}
		#endregion
	}
}