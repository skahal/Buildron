using System;

namespace Buildron.Domain.Notifications
{
	public class NotificationReceivedEventArgs : EventArgs
	{
		public NotificationReceivedEventArgs(Notification notification)
		{
			Notification = notification;
		}
		
		public Notification Notification { get; private set; }
	}
}

