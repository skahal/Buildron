using System;
using System.Linq;
using Buildron.Domain;
using Buildron.Domain.Builds;
using Buildron.Domain.CIServers;
using NUnit.Framework;
using Rhino.Mocks;
using Skahal.Infrastructure.Framework.Repositories;
using Skahal.Logging;

namespace Buildron.Domain.UnitTests
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
            var target = new CIServerService(repository);

            var provider = MockRepository.GenerateMock<IBuildsProvider>();
            target.Initialize(provider);

            var statusChangedRaised = target.CreateAssert<CIServerStatusChangedEventArgs>("CIServerStatusChanged", 4);

            provider.Raise(p => p.ServerUp += null, null, null);
            Assert.AreEqual(CIServerStatus.Up, target.GetCIServer().Status);

            provider.Raise(p => p.ServerDown += null, null, null);
            Assert.AreEqual(CIServerStatus.Down, target.GetCIServer().Status);

            provider.Raise(p => p.ServerUp += null, null, null);
            Assert.AreEqual(CIServerStatus.Up, target.GetCIServer().Status);

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
            var target = new CIServerService(repository);

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

            var target = new CIServerService(repository);
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

            var target = new CIServerService(repository);
            target.SaveCIServer(ciServer);

            Assert.AreEqual("old", ciServer.Title);
            repository.VerifyAllExpectations();
        }
    }
}