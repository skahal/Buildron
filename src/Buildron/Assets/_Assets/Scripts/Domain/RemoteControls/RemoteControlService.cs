using System;
using System.Linq;
using Buildron.Domain.CIServers;
using Buildron.Domain.Users;
using Skahal.Common;
using Skahal.Infrastructure.Framework.Repositories;
using Zenject;

namespace Buildron.Domain.RemoteControls
{
	/// <summary>
	/// Remote control service.
	/// </summary>
	public class RemoteControlService : IRemoteControlService, IInitializable
	{
		#region Events
		/// <summary>
		/// Occurs when remote control changed.
		/// </summary>
		public event EventHandler<RemoteControlChangedEventArgs> RemoteControlChanged;

		public event EventHandler<RemoteControlCommandReceivedEventArgs> RemoteControlCommandReceived;
		#endregion

		#region Fields
		private readonly ICIServerService m_ciServerService;
		private readonly IUserService m_userService;
		private readonly IRepository<IRemoteControl> m_repository;
		private IRemoteControl m_connectedRC;
		private bool? m_hasRemoteControlConnectedSomeDay;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.RemoteControls.RemoteControlService"/> class.
		/// </summary>
		/// <param name="ciServerService">CI server service.</param>
		/// <param name="userService">User service.</param>
		/// <param name="repository">Repository.</param>
		public RemoteControlService (ICIServerService ciServerService, IUserService userService, IRepository<IRemoteControl> repository)
		{
			m_ciServerService = ciServerService;
			m_userService = userService;
			m_repository = repository;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets a value indicating whether this any remote control has connected some day.
		/// </summary>
		public bool HasRemoteControlConnectedSomeDay
		{
			get
			{
				if (!m_hasRemoteControlConnectedSomeDay.HasValue)
				{
					m_hasRemoteControlConnectedSomeDay = m_repository.All ().Any ();
				}

				return m_hasRemoteControlConnectedSomeDay.Value;
			}	
		}

		/// <summary>
		/// Gets a value indicating whether has a remote control connected.
		/// </summary>
		public bool HasRemoteControlConnected
		{
			get
			{
				return m_connectedRC != null;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Initialize the service.
		/// </summary>
		public void Initialize ()
		{
			m_userService.UserAuthenticationCompleted += (sender, args) =>
			{
				if (m_connectedRC != null)
				{
					if (args.Success)
					{
						if (!m_repository.All ().Any (r => r == m_connectedRC))
						{
							m_repository.Create (m_connectedRC);
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

		/// <summary>
		/// Connects the remote control.
		/// </summary>
		/// <param name="rcToConnect">Rc to connect.</param>
		public void ConnectRemoteControl (IRemoteControl rcToConnect)
		{
			m_connectedRC = rcToConnect;
			m_connectedRC.Connected = true;
			m_ciServerService.AuthenticateUser (rcToConnect);
			m_hasRemoteControlConnectedSomeDay = true;

			RemoteControlChanged.Raise (typeof(RemoteControlService), new RemoteControlChangedEventArgs (m_connectedRC));
		}

		/// <summary>
		/// Disconnects the remote control.
		/// </summary>
		public void DisconnectRemoteControl ()
		{
            if (m_connectedRC != null)
            {
                m_connectedRC.Connected = false;
                RemoteControlChanged.Raise(typeof(RemoteControlService), new RemoteControlChangedEventArgs(m_connectedRC));
                m_connectedRC = null;
            }
		}

		/// <summary>
		/// Gets the connected remote control.
		/// </summary>
		/// <returns>The connected remote control.</returns>
		public IRemoteControl GetConnectedRemoteControl ()
		{
			return m_connectedRC;
		}

		public bool ReceiveCommand(IRemoteControlCommand command)
		{
			if (m_connectedRC == null) {
				throw new InvalidOperationException (
					"Impossible to receive a command from a remote control, when no remote control connected");
			}

			var args = new RemoteControlCommandReceivedEventArgs (m_connectedRC, command);
			RemoteControlCommandReceived.Raise (this, args);

			return !args.Cancel;
		}
		#endregion
	}
}