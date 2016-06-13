using System.Collections;
using Buildron.Domain;
using Buildron.Infrastructure.UserAvatarProviders;
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
        var humanFallbackUserAvatarProvider = new StaticUserAvatarProvider();
        humanFallbackUserAvatarProvider.AddPhoto(UserKind.Human, UnunknownAvatar);

        var nonHumanUserAvatarProviders = new StaticUserAvatarProvider();
        nonHumanUserAvatarProviders.AddPhoto(UserKind.ScheduledTrigger, ScheduledTriggerAvatar);
        nonHumanUserAvatarProviders.AddPhoto(UserKind.RetryTrigger, RetryTriggerAvatar);

        UserService.Initialize(
            new IUserAvatarProvider[] { new GravatarUserAvatarProvider(), new AcronymUserAvatarProvider(), humanFallbackUserAvatarProvider },
            new IUserAvatarProvider[] { nonHumanUserAvatarProviders });
    }
	#endregion
}
