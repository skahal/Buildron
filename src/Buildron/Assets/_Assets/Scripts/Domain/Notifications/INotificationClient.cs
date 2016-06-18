using Buildron.Domain.Versions;
using System;

namespace Buildron.Domain.Notifications
{
	/// <summary>
	/// Defines an interface for a client to notifications sent from Buildron Back-end server.
	/// </summary>
	public interface INotificationClient
	{
		#region Events
		/// <summary>
		/// Occurs when notification received.
		/// </summary>
		event EventHandler<NotificationReceivedEventArgs> NotificationReceived;
		#endregion
		
		#region Methods
		/// <summary>
		/// Checks the notifications.
		/// </summary>
		/// <param name="clientId">Client identifier.</param>
		/// <param name="kind">Kind.</param>
		/// <param name="device">Device.</param>
		void CheckNotifications(string clientId, ClientKind kind, SHDeviceFamily device);
		#endregion
	}
}