using System;
using System.Collections.Generic;
using Buildron.Domain.Builds;
using Buildron.Domain.CIServers;
using Buildron.Domain.RemoteControls;
using Buildron.Domain.Users;
using Buildron.Infrastructure.RemoteControlProxies;
using Skahal.Common;
using Skahal.Logging;

namespace Buildron.Domain.Mods
{
    /// <summary>
    /// Represents the mod context.
    /// ModContext is basically a facade to many events that occurs in Buildron.
    /// </summary>
	public sealed class ModContext : IModContext
	{    
		#region Events
		public event EventHandler<BuildFoundEventArgs> BuildFound;
		public event EventHandler<BuildRemovedEventArgs> BuildRemoved;
		public event EventHandler<BuildUpdatedEventArgs> BuildUpdated;    
		public event EventHandler<BuildStatusChangedEventArgs> BuildStatusChanged;
		public event EventHandler<BuildTriggeredByChangedEventArgs> BuildTriggeredByChanged;
		public event EventHandler<BuildsRefreshedEventArgs> BuildsRefreshed;  
		public event EventHandler<CIServerConnectedEventArgs> CIServerConnected;
		public event EventHandler<CIServerStatusChangedEventArgs> CIServerStatusChanged;
		public event EventHandler<UserFoundEventArgs> UserFound;
		public event EventHandler<UserUpdatedEventArgs> UserUpdated;
		public event EventHandler<UserTriggeredBuildEventArgs> UserTriggeredBuild;
		public event EventHandler<UserRemovedEventArgs> UserRemoved;
		public event EventHandler<UserAuthenticationCompletedEventArgs> UserAuthenticationCompleted;
		public event EventHandler<RemoteControlChangedEventArgs> RemoteControlChanged;
		public event EventHandler<RemoteControlCommandReceivedEventArgs> RemoteControlCommandReceived;
        #endregion

        #region Fields
		private ModInstanceInfo m_instance;
        private readonly IBuildService m_buildService;
        private readonly ICIServerService m_ciServerService;
        private readonly IRemoteControlService m_remoteControlService;
        private readonly IUserService m_userService;
        #endregion

        #region Constructors
		public ModContext(ModInstanceInfo instance, ISHLogStrategy log, IBuildService buildService, ICIServerService ciServerService, IRemoteControlService remoteControlService, IUserService userService)
        {            
			m_instance = instance;
			Log = new PrefixedLogStrategy(log, "MOD [{0}]: ".With(m_instance.Info.Name));

			Assets = new LogAssetsProxy (instance.Assets, Log);
			GameObjects = new LogGameObjectsProxy (instance.GameObjects, Log);
			GameObjectsPool = new LogGameObjectsPoolProxy (instance.GameObjectsPool, Log);
			BuildGameObjects = instance.BuildGameObjects;
			UserGameObjects = instance.UserGameObjects;
            UI = instance.UI;
            FileSystem = instance.FileSystem;
			Data = new LogDataProxy(instance.Data, Log);
			Camera = new LogCameraProxy (instance.Camera, Log);
			Preferences = new LogPreferencesProxy(instance.Preferences, Log);
            RemoteControl = new ModRemoteControlProxy(remoteControlService);

            m_buildService = buildService;
            m_ciServerService = ciServerService;
            m_remoteControlService = remoteControlService;
            m_userService = userService;

            AttachToBuildService();
            AttachToCIServerService();
            AttachToRemoteControlService();
            AttachToUserService();            
        }
        #endregion

        #region Properties
        public IList<IBuild> Builds
        {
            get
            {
                return m_buildService.Builds;
            }
        }

        public IList<IUser> Users
        {
            get
            {
                return m_userService.Users;
            }
        }

		public ICIServer CIServer 
		{
			get {
				return m_ciServerService.GetCIServer ();
			}
		}

        public ISHLogStrategy Log { get; private set; }

		public IAssetsProxy Assets { get; private set; }

		public IGameObjectsProxy GameObjects { get; private set; }

        public IGameObjectsPoolProxy GameObjectsPool { get; private set; }

        public IUIProxy UI { get; private set; }

        public IFileSystemProxy FileSystem { get; private set; }

		public IDataProxy Data { get; private set; }

		public IBuildGameObjectsProxy BuildGameObjects { get; private set; }

		public IUserGameObjectsProxy UserGameObjects { get; private set; }

        public ICameraProxy Camera { get; private set; }

        public IPreferencesProxy Preferences { get; private set; }

        public IRemoteControlProxy RemoteControl { get; private set; }
        #endregion

        #region Methods     
        private void AttachToBuildService()
        {
            m_buildService.BuildFound += (s, e) =>
            {
                var build = e.Build;

				build.StatusChanged += (s2, e2) => BuildStatusChanged.RaiseSafe(s2, e2, Log);
				build.TriggeredByChanged += (s2, e2) => BuildTriggeredByChanged.RaiseSafe(s2, e2, Log);

                BuildFound.RaiseSafe(s, e, Log);
            };

            m_buildService.BuildRemoved += (s, e) => BuildRemoved.RaiseSafe(s, e, Log);
            
            m_buildService.BuildsRefreshed += (s, e) => BuildsRefreshed.RaiseSafe(s, e, Log);
            m_buildService.BuildUpdated += (s, e) => BuildUpdated.RaiseSafe(s, e, Log);
        }

        private void AttachToCIServerService()
        {
			m_ciServerService.CIServerConnected += (s, e) => CIServerConnected.RaiseSafe(s, e, Log);
            m_ciServerService.CIServerStatusChanged += (s, e) => CIServerStatusChanged.RaiseSafe(s, e, Log);
        }

        private void AttachToRemoteControlService()
        {
            m_remoteControlService.RemoteControlChanged += (s, e) => RemoteControlChanged.RaiseSafe(s, e, Log);
			m_remoteControlService.RemoteControlCommandReceived += (s, e) => RemoteControlCommandReceived.RaiseSafe(s, e, Log);
        }

        private void AttachToUserService()
        {
            m_userService.UserAuthenticationCompleted += (s, e) => UserAuthenticationCompleted.RaiseSafe(s, e, Log);
            m_userService.UserFound += (s, e) => UserFound.RaiseSafe(s, e, Log);
            m_userService.UserRemoved += (s, e) => UserRemoved.RaiseSafe(s, e, Log);
            m_userService.UserTriggeredBuild += (s, e) => UserTriggeredBuild.RaiseSafe(s, e, Log);
            m_userService.UserUpdated += (s, e) => UserUpdated.RaiseSafe(s, e, Log);
        }
        #endregion
    }
}
