using System;
using Buildron.Domain;
using NUnit.Framework;
using Rhino.Mocks;
using Skahal.Infrastructure.Framework.Repositories;
using Skahal.Logging;
using System.Linq;

namespace Buildron.Domain.UnitTests
{
    [Category("Buildron.Domain")]
    public class RemoteControlServiceTest
    {      
        [Test]
        public void Initialize_UserAuthenticationCompletedNotSuccess_NoRemoteControlConnected()
        {
            var ciServerService = MockRepository.GenerateMock<ICIServerService>();
            var userService = MockRepository.GenerateMock<IUserService>();
            var repository = MockRepository.GenerateMock<IRepository<RemoteControl>>();

            var target = new RemoteControlService(ciServerService, userService, repository);
            var rc = new RemoteControl() { UserName = "u1" };
            target.ConnectRemoteControl(rc);
            Assert.AreEqual(rc, target.GetConnectedRemoteControl());
            Assert.IsTrue(rc.Connected);
            Assert.IsTrue(target.HasRemoteControlConnectedSomeDay);

            target.Initialize();
            userService.Raise(u => u.UserAuthenticationCompleted += null, null, new UserAuthenticationCompletedEventArgs(new User(), false));

            Assert.IsNull(target.GetConnectedRemoteControl());
            Assert.IsFalse(rc.Connected);
            Assert.IsTrue(target.HasRemoteControlConnectedSomeDay);
        }
    }
}