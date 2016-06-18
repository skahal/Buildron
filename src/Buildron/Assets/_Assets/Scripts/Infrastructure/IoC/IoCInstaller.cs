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

public class IoCInstaller : MonoInstaller
{
	#region Editor properties
	public Texture2D ScheduledTriggerAvatar;
	public Texture2D RetryTriggerAvatar;
	public Texture2D UnunknownAvatar;
	#endregion

	public override void InstallBindings ()
	{
        DependencyService.Register<IServerStateRepository>(new PlayerPrefsServerStateRepository());
        DependencyService.Register<IVersionClient>(() => {
            return new BackEndClient();
        });

        DependencyService.Register<IVersionRepository>(() => {
            return new PlayerPrefsVersionRepository();
        });

        DependencyService.Register<INotificationClient>(() => {
            return new BackEndClient();
        });

        InstallLog ();
		InstallUser ();
		InstallBuild ();        
	}

	void InstallLog ()
	{
		var logStrategy = SHLog.LogStrategy;
		logStrategy.Debug ("IOC :: Installing bindings...");
		Container.Bind<ISHLogStrategy> ().FromInstance (logStrategy).AsSingle ();
	}

	void InstallUser ()
	{
		var humanFallbackUserAvatarProvider = new StaticUserAvatarProvider ();
		humanFallbackUserAvatarProvider.AddPhoto(UserKind.Human, UnunknownAvatar);

		var nonHumanUserAvatarProviders = new StaticUserAvatarProvider ();
		nonHumanUserAvatarProviders.AddPhoto(UserKind.ScheduledTrigger, ScheduledTriggerAvatar);
		nonHumanUserAvatarProviders.AddPhoto(UserKind.RetryTrigger, RetryTriggerAvatar);

		var userService = new UserService (new IUserAvatarProvider[] {
			new GravatarUserAvatarProvider (),
			new AcronymUserAvatarProvider (),
			humanFallbackUserAvatarProvider
		}, new IUserAvatarProvider[] {
			nonHumanUserAvatarProviders
		}, Container.Resolve<ISHLogStrategy> ());
		Container.Bind<IUserService> ().FromInstance (userService);

		Container.BindFactory<UserController, UserController.Factory>()
			.FromPrefabResource ("UserPrefab")
			.UnderGameObjectGroup("Users");
	}

	void InstallBuild ()
	{
		Container.Bind<BuildGOService> ().AsSingle();
		Container.BindFactory<BuildController, BuildController.Factory>()
			.FromPrefabResource ("BuildPrefab")
			.UnderGameObjectGroup("Builds");
	}
}