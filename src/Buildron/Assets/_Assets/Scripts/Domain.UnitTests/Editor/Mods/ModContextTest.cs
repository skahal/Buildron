using NUnit.Framework;
using Rhino.Mocks;
using Buildron.Domain.Builds;
using Buildron.Domain.Users;
using Buildron.Domain.CIServers;
using Buildron.Domain.RemoteControls;
using System.Collections.Generic;
using Buildron.Domain.Mods;
using Skahal.Logging;

namespace Buildron.Domain.UnitTests.Builds
{
    [Category("Buildron.Domain")]
    [Category("Mods")]
    public class ModContextTest
    {
        #region Fields
        private ModContext m_target;
        private IBuildService m_buildService;
        private ICIServerService m_ciService;
        private IRemoteControlService m_remoteControlService;
        private IUserService m_userService;
        #endregion

        #region Initialize
        [SetUp]
        public void Initialize()
        {
            var mod = MockRepository.GenerateMock<IMod>();
            var log = MockRepository.GenerateMock<ISHLogStrategy>();
            m_buildService = MockRepository.GenerateMock<IBuildService>();
            m_ciService = MockRepository.GenerateMock<ICIServerService>();
            m_remoteControlService = MockRepository.GenerateMock<IRemoteControlService>();
            m_userService = MockRepository.GenerateMock<IUserService>();            
            m_target = new ModContext(mod, log, m_buildService, m_ciService, m_remoteControlService, m_userService);
        }
        #endregion

        #region Build
        [Test]
        public void Builds_BuildService_SameValues()
        {
            var builds = new List<IBuild>();
            m_buildService.Expect(u => u.Builds).Return(builds);

            Assert.AreSame(builds, m_target.Builds);
        }

        [Test]
        public void BuildFound_BuildServiceRaise_EventRaised()
        {          
            var raised = m_target.CreateAssert<BuildFoundEventArgs>("BuildFound", 1);
            m_buildService.Raise(b => b.BuildFound += null, null, new BuildFoundEventArgs(new Build()));
            
            raised.Assert();
        }

        [Test]
        public void BuildRemoved_BuildServiceRaise_EventRaised()
        {
            var raised = m_target.CreateAssert<BuildRemovedEventArgs>("BuildRemoved", 1);
            var b1 = new Build { Configuration = new BuildConfiguration { Id = "1", Project = new BuildProject { Name = "2" } } };
            var b2 = new Build { Configuration = new BuildConfiguration { Id = "2", Project = new BuildProject { Name = "2" } } };

            m_buildService.Raise(b => b.BuildFound += null, null, new BuildFoundEventArgs(b1));
            m_buildService.Raise(b => b.BuildFound += null, null, new BuildFoundEventArgs(b2));
            m_buildService.Raise(b => b.BuildRemoved += null, null, new BuildRemovedEventArgs(b2));

            raised.Assert();
        }

        [Test]
        public void BuildsRefreshed_BuildServiceRaise_EventRaised()
        {
            var raised = m_target.CreateAssert<BuildsRefreshedEventArgs>("BuildsRefreshed", 1);
            m_buildService.Raise(b => b.BuildsRefreshed += null, null, new BuildsRefreshedEventArgs(null, null, null));

            raised.Assert();
        }

        [Test]
        public void BuildUpdated_BuildServiceRaise_EventRaised()
        {
            var raised = m_target.CreateAssert<BuildUpdatedEventArgs>("BuildUpdated", 1);
            m_buildService.Raise(b => b.BuildUpdated += null, null, new BuildUpdatedEventArgs(new Build()));

            raised.Assert();
        }

        [Test]
        public void BuildStatusChanged_AnyBuildStatusChanged_EventRaised()
        {
            var raised = m_target.CreateAssert<BuildStatusChangedEventArgs>( "BuildStatusChanged", 3);
            var b1 = new Build { Status = BuildStatus.Success };
            var b2 = new Build { Status = BuildStatus.Running };
            m_buildService.Raise(b => b.BuildFound += null, null, new BuildFoundEventArgs(b1));
            m_buildService.Raise(b => b.BuildFound += null, null, new BuildFoundEventArgs(b2));

            b1.Status = BuildStatus.Running;
            b2.Status = BuildStatus.Failed;
            b1.Status = BuildStatus.Success;

            raised.Assert();
        }

        [Test]
        public void BuildTriggeredByChanged_AnyBuildTriggeredByChanged_EventRaised()
        {
            var raised = m_target.CreateAssert<BuildTriggeredByChangedEventArgs>("BuildTriggeredByChanged", 3);
            var user1 = new User { UserName = "u1" };
            var user2 = new User { UserName = "u2" };
            var b1 = new Build { TriggeredBy = user1 };
            var b2 = new Build ();

            m_buildService.Raise(b => b.BuildFound += null, null, new BuildFoundEventArgs(b1));
            m_buildService.Raise(b => b.BuildFound += null, null, new BuildFoundEventArgs(b2));

            b1.TriggeredBy = user1;
            b1.TriggeredBy = user2;
            b2.TriggeredBy = user1;
            b1.TriggeredBy = user1;

            raised.Assert();
        }
        #endregion

        #region CI server
        [Test]
        public void CIServerStatusChanged_CIServerRaise_EventRaised()
        {
            var raised = m_target.CreateAssert<CIServerStatusChangedEventArgs>("CIServerStatusChanged", 1);
            m_ciService.Raise(b => b.CIServerStatusChanged += null, null, new CIServerStatusChangedEventArgs(new CIServer()));
            
            raised.Assert();
        }
        #endregion

        #region Remote control
        [Test]
        public void RemoteControlChanged_RemoteControlServiceRaise_EventRaised()
        {
            var raised = m_target.CreateAssert<RemoteControlChangedEventArgs>("RemoteControlChanged", 1);
            m_remoteControlService.Raise(b => b.RemoteControlChanged += null, null, new RemoteControlChangedEventArgs(new RemoteControl()));

            raised.Assert();
        }
        #endregion

        #region User
        [Test]
        public void Users_UserService_SameValues()
        {
            var users = new List<User>();
            m_userService.Expect(u => u.Users).Return(users);

            Assert.AreSame(users, m_target.Users);
        }

        [Test]
        public void UserAuthenticationCompleted_UserServiceRaise_EventRaised()
        {
            var raised = m_target.CreateAssert<UserAuthenticationCompletedEventArgs>("UserAuthenticationCompleted", 1);
            m_userService.Raise(b => b.UserAuthenticationCompleted += null, null, new UserAuthenticationCompletedEventArgs(new User(), true));

            raised.Assert();
        }

        [Test]
        public void UserFound_UserServiceRaise_EventRaised()
        {
            var raised = m_target.CreateAssert<UserFoundEventArgs>("UserFound", 3);

            var u1 = new User { UserName = "u1" };
            var u2 = new User { UserName = "u2" };
            m_userService.Raise(b => b.UserFound += null, null, new UserFoundEventArgs(u1));
            m_userService.Raise(b => b.UserFound += null, null, new UserFoundEventArgs(u1));
            m_userService.Raise(b => b.UserFound += null, null, new UserFoundEventArgs(u2));            
            raised.Assert();
        }

        [Test]
        public void UserRemoved_UserServiceRaise_EventRaised()
        {
            var raised = m_target.CreateAssert<UserRemovedEventArgs>("UserRemoved", 2);

            var u1 = new User { UserName = "u1" };
            var u2 = new User { UserName = "u2" };
            m_userService.Raise(b => b.UserFound += null, null, new UserFoundEventArgs(u1));
            m_userService.Raise(b => b.UserFound += null, null, new UserFoundEventArgs(u2));
            m_userService.Raise(b => b.UserRemoved += null, null, new UserRemovedEventArgs(u2));
            m_userService.Raise(b => b.UserRemoved += null, null, new UserRemovedEventArgs(u2));
            
            raised.Assert();
        }

        [Test]
        public void UserTriggeredBuild_UserServiceRaise_EventRaised()
        {
            var raised = m_target.CreateAssert<UserTriggeredBuildEventArgs>("UserTriggeredBuild", 1);
            var u1 = new User { UserName = "u1" };
            m_userService.Raise(b => b.UserTriggeredBuild += null, null, new UserTriggeredBuildEventArgs(u1, new Build()));

            raised.Assert();
        }

        [Test]
        public void UserUpdated_UserServiceRaise_EventRaised()
        {
            var raised = m_target.CreateAssert<UserUpdatedEventArgs>("UserUpdated", 1);
            var u1 = new User { UserName = "u1", Name = "User 1" };
            m_userService.Raise(b => b.UserUpdated += null, null, new UserUpdatedEventArgs(u1));

            raised.Assert();
        }
        #endregion
    }
}