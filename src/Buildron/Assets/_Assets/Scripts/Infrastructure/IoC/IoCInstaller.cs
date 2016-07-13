using UnityEngine;
using Zenject;
using Skahal.Logging;
using Buildron.Infrastructure.UserAvatarProviders;
using Buildron.Domain;
using System;
using Skahal.Infrastructure.Framework.Commons;
using Buildron.Infrastructure.Repositories;
using Buildron.Domain.Versions;
using Buildron.Infrastructure.Clients;
using Buildron.Domain.Notifications;
using Buildron.Application;
using Buildron.Domain.EasterEggs;
using Skahal.Infrastructure.Framework.Repositories;
using Skahal.Infrastructure.Repositories;
using Buildron.Domain.Builds;
using Buildron.Domain.Users;
using Buildron.Domain.CIServers;
using Buildron.Domain.RemoteControls;
using Buildron.Controllers;
using Buildron.Domain.Servers;
using Skahal.Threading;
using Buildron.Domain.Mods;
using Buildron.Infrastructure.ModsProvider;

namespace Buildron.Infrastructure.IoC
{
	public class IoCInstaller : MonoInstaller
	{
		#region Properties
		public Texture2D ScheduledTriggerAvatar;
		public Texture2D RetryTriggerAvatar;
		public Texture2D UnunknownAvatar;
		#endregion

		#region Methods
		public override void InstallBindings ()
		{
			var log = InstallLog ();

			log.Debug ("IOC :: Installing domain...");
			InstallDomain ();

			log.Debug ("IOC :: Installing repositories...");
			InstallRepositories ();

			log.Debug ("IOC :: Installing user bindings...");
			InstallUser ();

			log.Debug ("IOC :: Installing build bindings...");
			InstallBuild ();  

			log.Debug ("IOC :: Installing misc bindings...");
			InstallMisc ();			
		
			log.Debug ("IOC :: Bindings installed.");
		}


		ISHLogStrategy InstallLog ()
		{
			var logStrategy = SHLog.LogStrategy;
			Container.Bind<ISHLogStrategy> ().FromInstance (logStrategy).AsSingle ();

			return logStrategy;
		}

		void InstallDomain ()
		{
			Container.Bind<IAsyncActionProvider> ().To<CoroutineAsyncActionProvider>();
			Container.Bind<IVersionService> ().To<VersionService> ().AsSingle ();
            Container.BindInitializableService<IRemoteControlService, RemoteControlService>();
            Container.Bind<ICIServerService>().To<CIServerService>().AsSingle();
            Container.Bind<IBuildService>().To<BuildService>().AsSingle();
			Container.Bind<IServerService>().To<ServerService>().AsSingle();
			Container.Bind<IModLoader>().To<ModLoader>().AsSingle();

			var log = Container.Resolve<ISHLogStrategy> ();

#if UNITY_EDITOR
			var folder =  "../../build/Mods/";
#else
            var folder =  UnityEngine.Application.dataPath.Substring(0, UnityEngine.Application.dataPath.LastIndexOf("/")) + "/mods/";
            folder = folder.Replace(@"\", "/");
#endif
            var fileSystemModsProvider = new FileSystemModsProvider(folder, log);
            var appDomainModsProvider = new AppDomainModsProvider (log);
			Container.Bind<IModsProvider[]> ().FromInstance (new IModsProvider[] { fileSystemModsProvider, appDomainModsProvider });
		}

		void InstallRepositories ()
		{
			Container.Bind<IRepository<ServerState>>().To<GenericPlayerPrefsRepository<ServerState>>().AsSingle ();
            Container.Bind<IVersionRepository> ().To<PlayerPrefsVersionRepository> ().AsSingle ();
			Container.Bind<IRepository<ICIServer>>().To<GenericPlayerPrefsRepository<ICIServer, CIServer>>().AsSingle();
            Container.Bind<IRepository<IRemoteControl>>().To<GenericPlayerPrefsRepository<IRemoteControl, RemoteControl>>().AsSingle();
        }

		void InstallUser ()
		{
			var humanFallbackUserAvatarProvider = new StaticUserAvatarProvider ();
			humanFallbackUserAvatarProvider.AddPhoto (UserKind.Human, UnunknownAvatar);

			var nonHumanUserAvatarProviders = new StaticUserAvatarProvider ();
			nonHumanUserAvatarProviders.AddPhoto (UserKind.ScheduledTrigger, ScheduledTriggerAvatar);
			nonHumanUserAvatarProviders.AddPhoto (UserKind.RetryTrigger, RetryTriggerAvatar);

			var userService = new UserService (new IUserAvatarProvider[] {
				new GravatarUserAvatarProvider (),
				new AcronymUserAvatarProvider (),
				humanFallbackUserAvatarProvider
			}, new IUserAvatarProvider[] {
				nonHumanUserAvatarProviders
			}, Container.Resolve<ISHLogStrategy> ());
			Container.Bind<IUserService> ().FromInstance (userService);

            Container.BindFactory<UserController, UserController.Factory>()
            .FromPrefabResource("UserPrefab")
            .UnderGameObjectGroup("Users");
		}

		void InstallBuild ()
		{
			Container.Bind<BuildGOService> ().AsSingle ();
			Container.BindFactory<BuildController, BuildController.Factory> ()
			.FromPrefabResource ("BuildPrefab")
			.UnderGameObjectGroup ("Builds");
		}

		void InstallMisc ()
		{
			Container.BindController<MatrixEasterEggController> ();
			Container.BindController<KickEasterEggController> ();

			var easterEggService = new EasterEggService (
				new IEasterEggProvider[] { 
					Container.Resolve<MatrixEasterEggController> (),
					Container.Resolve<KickEasterEggController> ()
				}, 
				Container.Resolve<ISHLogStrategy> ());
			Container.Bind<EasterEggService> ().FromInstance (easterEggService).AsSingle ();

			var backEndClient = new BackEndClient ();
			Container.Bind<INotificationClient> ().FromInstance (backEndClient);
			Container.Bind<IVersionClient> ().FromInstance (backEndClient);
			Container.Bind<NotificationService> ().To<NotificationService> ().AsSingle ();

			var listener = Container.BindController<RemoteControlController> (true);	
			Container.Bind<IRemoteControlMessagesListener> ().FromInstance (listener);
			Container.BindController<NotificationController>();
			Container.BindController<ConfigPanelController> ();
			Container.BindController<CameraController> ();
            Container.BindController<MainSceneController>();
			Container.BindController<ModLoaderController>();
        }
#endregion
	}
}