using NUnit.Framework;
using Rhino.Mocks;
using Buildron.Domain.Notifications;
using Buildron.Domain.Versions;

namespace Buildron.Domain.UnitTests.Notifications
{
	[Category ("Buildron.Domain")]
	public class NotificationServiceTest
	{
		#region Fields
		private NotificationService m_target;
		private INotificationClient m_notificationClient;
		private IVersionService m_versionService;
		#endregion

		#region Initialize
		[SetUp]
		public void InitializeTest()
		{
			m_notificationClient = MockRepository.GenerateMock<INotificationClient> ();
			m_versionService = MockRepository.GenerateMock<IVersionService> ();

			m_target = new NotificationService (m_notificationClient, m_versionService);
		}
		#endregion

		#region Tests
		[Test]
		public void CheckNotifications_Version_NotificationReceivedRaised()
		{
			var notificationReceivedCount = 0;
			m_versionService.Expect (v => v.GetVersion ()).Return (new Version ());
			m_target.NotificationReceived += (sender, e) => notificationReceivedCount++;
			m_target.CheckNotifications (ClientKind.Buildron, SHDeviceFamily.Editor);
			Assert.AreEqual (0, notificationReceivedCount);
			m_notificationClient.Raise (n => n.NotificationReceived += null, null, new NotificationReceivedEventArgs(null));
			Assert.AreEqual (1, notificationReceivedCount);
		}

		#endregion
	}
}