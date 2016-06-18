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
		private INotificationClient m_notificationClient;
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
		public NotificationService (INotificationClient notificationClient)
		{
			m_notificationClient = notificationClient;
			m_notificationClient.NotificationReceived += (sender, e) => NotificationReceived.Raise (this, e);
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
			var version = VersionService.GetVersion();
			
			if (version != null)
			{
				m_notificationClient.CheckNotifications(version.ClientId, kind, device);
			}
		}
		#endregion
	}

}