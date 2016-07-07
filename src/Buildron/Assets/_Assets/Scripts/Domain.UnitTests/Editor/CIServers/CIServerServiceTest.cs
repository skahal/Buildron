using System;
using System.Linq;
using Buildron.Domain;
using Buildron.Domain.Builds;
using Buildron.Domain.CIServers;
using NUnit.Framework;
using Rhino.Mocks;
using Skahal.Infrastructure.Framework.Repositories;
using Skahal.Logging;
using Skahal.Threading;

namespace Buildron.Domain.UnitTests.CIServers
{
    [Category("Buildron.Domain")]
    public class CIServerServiceTest
    {
        [Test]
        public void Initialize_StatusChanged_EventsRaised()
        {
            var ciServer = new CIServer();
            ciServer.Status = CIServerStatus.Down;
            var repository = MockRepository.GenerateMock<IRepository<CIServer>>();
            repository.Expect(r => r.All()).Return((new CIServer[] { ciServer }).AsQueryable());

			var startCount = 0;
			var asyncActionProvider = MockRepository.GenerateMock<IAsyncActionProvider> ();
			asyncActionProvider.Expect (a => a.Start (0, null)).IgnoreArguments ().WhenCalled (m => {
				var action = (Action) m.Arguments[1];
				startCount++;

				if (startCount > 1)
				{
					action();
				}
			});

            var target = new CIServerService(repository, asyncActionProvider);

            var provider = MockRepository.GenerateMock<IBuildsProvider>();
            target.Initialize(provider);

            var statusChangedRaised = target.CreateAssert<CIServerStatusChangedEventArgs>("CIServerStatusChanged", 2);

			// Should raise ServerUp.
            provider.Raise(p => p.ServerUp += null, null, null);
            Assert.AreEqual(CIServerStatus.Up, target.GetCIServer().Status);

			// Should not raise events, because is waiting isDownAsyncAction
            provider.Raise(p => p.ServerDown += null, null, null);
			Assert.AreEqual(CIServerStatus.Up, target.GetCIServer().Status);

			// Should not raise events, because is still up.
            provider.Raise(p => p.ServerUp += null, null, null);
			Assert.AreEqual(CIServerStatus.Up, target.GetCIServer().Status);

			// Should raise ServerDown, because isDownSyncAction already ran.
            provider.Raise(p => p.ServerDown += null, null, null);
            Assert.AreEqual(CIServerStatus.Down, target.GetCIServer().Status);

			// Should not raise events, because is still down.
			provider.Raise(p => p.ServerDown += null, null, null);
			Assert.AreEqual(CIServerStatus.Down, target.GetCIServer().Status);

            statusChangedRaised.Assert();
        }

        [Test]
        public void AuthenticateUser_AuthenticationSuccessful_Initialized()
        {
            var ciServer = new CIServer();
            ciServer.Status = CIServerStatus.Down;
            var repository = MockRepository.GenerateMock<IRepository<CIServer>>();
            repository.Expect(r => r.All()).Return((new CIServer[] { ciServer }).AsQueryable());
			var target = new CIServerService(repository, MockRepository.GenerateMock<IAsyncActionProvider>());

            var provider = MockRepository.GenerateMock<IBuildsProvider>();
            provider.Expect(p => p.AuthenticateUser(null)).IgnoreArguments().WhenCalled(m =>
            {
                provider.Raise(p => p.UserAuthenticationSuccessful += null, null, null);
            });

            var statusChangedRaised = target.CreateAssert<CIServerStatusChangedEventArgs>("CIServerStatusChanged", 1);

            target.Initialize(provider);
            Assert.IsFalse(target.Initialized);

            target.AuthenticateUser(new CIServer());
            Assert.IsTrue(target.Initialized);
            Assert.AreEqual(CIServerStatus.Up, target.GetCIServer().Status);
            statusChangedRaised.Assert();
        }

        [Test]
        public void SaveCIServer_New_Default()
        {
            var ciServer = new CIServer() { Id = 0, Title = "new"};
            var repository = MockRepository.GenerateMock<IRepository<CIServer>>();
            repository.Expect(r => r.All()).Return((new CIServer[0]).AsQueryable());
            repository.Expect(r => r.Create(ciServer)).Return(ciServer);

			var target = new CIServerService(repository, MockRepository.GenerateMock<IAsyncActionProvider>());
            target.SaveCIServer(ciServer);

            Assert.AreEqual("new", ciServer.Title);
            repository.VerifyAllExpectations();
        }

        [Test]
        public void SaveCIServer_Old_Updates()
        {
            var ciServer = new CIServer() { Id = 1, Title = "old" };
            var repository = MockRepository.GenerateMock<IRepository<CIServer>>();
            repository.Expect(r => r.All()).Return((new CIServer[] { ciServer } ).AsQueryable());
            repository.Expect(r => r.Modify(ciServer));

			var target = new CIServerService(repository, MockRepository.GenerateMock<IAsyncActionProvider>());
            target.SaveCIServer(ciServer);

            Assert.AreEqual("old", ciServer.Title);
            repository.VerifyAllExpectations();
        }
    }
}