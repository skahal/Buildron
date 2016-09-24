using System;

namespace Buildron.Domain.Notifications
{
	/// <summary>
	/// Notification received event arguments.
	/// </summary>
	public class NotificationReceivedEventArgs : EventArgs
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Notifications.NotificationReceivedEventArgs"/> class.
		/// </summary>
		/// <param name="notification">Notification.</param>
		public NotificationReceivedEventArgs(Notification notification)
		{
			Notification = notification;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the notification.
		/// </summary>
		/// <value>The notification.</value>
		public Notification Notification { get; private set; }
		#endregion
	}
}

