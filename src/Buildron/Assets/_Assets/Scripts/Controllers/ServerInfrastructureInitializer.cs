using System.Collections;
using Buildron.Domain;
using Buildron.Infrastructure.UserBuildAvatarProviders;
using UnityEngine;

public class ServerInfrastructureInitializer : MonoBehaviour {
	
	#region Editor properties
	public Texture2D ScheduledTriggerAvatar;
	public Texture2D RetryTriggerAvatar;
	#endregion
	
	#region Methods
	void Awake ()
	{
		var staticUserBuildAvatarProvider = new StaticUserBuildAvatarProvider ();
		staticUserBuildAvatarProvider.AddPhoto (BuildUserKind.ScheduledTrigger, ScheduledTriggerAvatar);
		staticUserBuildAvatarProvider.AddPhoto (BuildUserKind.RetryTrigger, RetryTriggerAvatar);
		
		BuildUserService.Initialize (new GravatarUserBuildAvatarProvider (), staticUserBuildAvatarProvider);
	}
	#endregion
}
