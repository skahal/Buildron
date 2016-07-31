using NUnit.Framework;
using Rhino.Mocks;
using Buildron.Domain.Versions;

namespace Buildron.Domain.UnitTests.Notifications
{
	[Category ("Buildron.Domain")]
	public class VersionServiceTest
	{
        #region Fields
        private IVersionClient m_versionClient;
        private IVersionRepository m_versionRepository;
		private VersionService m_target;
		#endregion

		#region Initialize
		[SetUp]
		public void InitializeTest()
		{
            m_versionClient = MockRepository.GenerateMock<IVersionClient>();
            m_versionRepository = MockRepository.GenerateMock<IVersionRepository>();
            m_target = new VersionService(m_versionClient, m_versionRepository);
		}
        #endregion

        #region Tests
        [Test]
        public void Register_NewClient_VersionClientCalled()
        {
            m_versionRepository.Expect(v => v.Save(null)).IgnoreArguments();
            m_versionClient.Expect(v => v.RegisterClient(null, ClientKind.Buildron, SHDeviceFamily.Windows)).IgnoreArguments()
                .WhenCalled((m) =>
                {
                    m_versionClient.Raise((c) => c.ClientRegistered += null, m_versionClient, new ClientRegisteredEventArgs("1", 2));
                });

            var clientRegisteredRaised = m_target.CreateAssert<ClientRegisteredEventArgs>("ClientRegistered", 1);

            m_target.Register(ClientKind.Buildron, SHDeviceFamily.Windows);

            clientRegisteredRaised.Assert();
            m_versionClient.VerifyAllExpectations();
            m_versionRepository.VerifyAllExpectations();
        }

        [Test]
		public void Register_OldClient_ClientRegisteredRaised()
		{
            m_versionRepository.Expect(v => v.Find()).Return(new Versions.Version {  ClientId = "1", ClientInstance = 2 });
            var clientRegisteredRaised = m_target.CreateAssert<ClientRegisteredEventArgs>("ClientRegistered", 1);

            m_target.Register(ClientKind.Buildron, SHDeviceFamily.Windows);

            clientRegisteredRaised.Assert();
		}

        [Test]
        public void CheckUpdates_Client_VersionClientCalled()
        {
            m_versionRepository.Expect(v => v.Find()).Return(new Versions.Version { ClientId = "1", ClientInstance = 2 });
            m_versionClient.Expect(v => v.CheckUpdates(null, ClientKind.Buildron, SHDeviceFamily.Windows)).IgnoreArguments()
                .WhenCalled((m) =>
                {
                    m_versionClient.Raise((c) => c.UpdateInfoReceived += null, m_versionClient, new UpdateInfoReceivedEventArgs(new VersionUpdateInfo()));
                });

            m_target.CheckUpdates(ClientKind.Buildron, SHDeviceFamily.Windows);
            m_versionClient.VerifyAllExpectations();
            m_versionRepository.VerifyAllExpectations();
        }
        #endregion
    }
}