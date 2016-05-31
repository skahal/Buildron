#region Usings
using Buildron.Domain.Versions;
using System;
#endregion

namespace Buildron.Domain.Notifications
{
	public interface INotificationClient
	{
		#region Events
		event EventHandler<NotificationReceivedEventArgs> NotificationReceived;
		#endregion
		
		#region Methods
		void CheckNotifications(string clientId, ClientKind kind, SHDeviceFamily device);
		#endregion
	}
}