using System.Collections;
using Buildron.Domain;
using Buildron.Infrastructure.BuildUserAvatarProviders;
using UnityEngine;

public class ServerInfrastructureInitializer : MonoBehaviour {
	
	#region Editor properties
	public Texture2D ScheduledTriggerAvatar;
	public Texture2D RetryTriggerAvatar;
    public Texture2D UnunknownAvatar;
    #endregion

    #region Methods
    void Awake()
    {
        var humanFallbackBuildUserAvatarProvider = new StaticBuildUserAvatarProvider();
        humanFallbackBuildUserAvatarProvider.AddPhoto(BuildUserKind.Human, UnunknownAvatar);

        var nonHumanBuildUserAvatarProviders = new StaticBuildUserAvatarProvider();
        nonHumanBuildUserAvatarProviders.AddPhoto(BuildUserKind.ScheduledTrigger, ScheduledTriggerAvatar);
        nonHumanBuildUserAvatarProviders.AddPhoto(BuildUserKind.RetryTrigger, RetryTriggerAvatar);

        BuildUserService.Initialize(
            new IBuildUserAvatarProvider[] { new GravatarBuildUserAvatarProvider(), new AcronymBuildUserAvatarProvider(), humanFallbackBuildUserAvatarProvider },
            new IBuildUserAvatarProvider[] { nonHumanBuildUserAvatarProviders });
    }
	#endregion
}
