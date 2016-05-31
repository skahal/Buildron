using Buildron.Domain.Versions;
using Skahal.Common;
using Skahal.Infrastructure.Framework.Commons;

namespace Buildron.Domain.Notifications
{
	public static class NotificationService
	{
		#region Fields
		private static INotificationClient s_notificationClient;
		private static System.EventHandler<NotificationReceivedEventArgs> s_notificationReceived;
		#endregion
			
		#region Events
		public static event System.EventHandler<NotificationReceivedEventArgs> NotificationReceived {
			add { s_notificationReceived += value; }
			remove { s_notificationReceived -= value; }	
		}
		#endregion 
		
		#region Constructors
		static NotificationService ()
		{
			s_notificationClient = DependencyService.Create<INotificationClient> ();
			
			s_notificationClient.NotificationReceived += delegate(object sender, NotificationReceivedEventArgs e) {
				s_notificationReceived.Raise(typeof(NotificationService), e);
			};
			
		}
		#endregion
		
		#region Methods
		public static void CheckNotifications(ClientKind kind, SHDeviceFamily device)
		{
			var version = VersionService.GetVersion();
			
			if (version != null)
			{
				s_notificationClient.CheckNotifications(version.ClientId, kind, device);
			}
		}
		#endregion
	}

}