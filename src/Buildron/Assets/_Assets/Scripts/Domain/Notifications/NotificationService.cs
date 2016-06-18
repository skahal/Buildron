using Buildron.Domain.Versions;
using Skahal.Common;
using Skahal.Infrastructure.Framework.Commons;
using System;

namespace Buildron.Domain.Notifications
{
	/// <summary>
	/// Notification service to handle messages sent from Buildron Backend server to clients.
	/// </summary>
	public class NotificationService
	{
		#region Fields
		private readonly INotificationClient m_notificationClient;
		private readonly IVersionService m_versionService;
		#endregion
			
		#region Events
		/// <summary>
		/// Occurs when notification received.
		/// </summary>
		public event EventHandler<NotificationReceivedEventArgs> NotificationReceived;
		#endregion 
		
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Notifications.NotificationService"/> class.
		/// </summary>
		/// <param name="notificationClient">Notification client.</param>
		/// <param name="versionService">Version service.</param>
		public NotificationService (INotificationClient notificationClient, IVersionService versionService)
		{
			m_notificationClient = notificationClient;
			m_notificationClient.NotificationReceived += (sender, e) => NotificationReceived.Raise (this, e);

			m_versionService = versionService;
		}
		#endregion
		
		#region Methods
		/// <summary>
		/// Checks the notifications.
		/// </summary>
		/// <param name="kind">Kind.</param>
		/// <param name="device">Device.</param>
		public void CheckNotifications(ClientKind kind, SHDeviceFamily device)
		{
			var version = m_versionService.GetVersion();
			
			if (version != null)
			{
				m_notificationClient.CheckNotifications(version.ClientId, kind, device);
			}
		}
		#endregion
	}

}