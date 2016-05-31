#region Usings
using UnityEngine;
using Buildron.Infrastructure.UserBuildAvatarProviders;
using System;
using Skahal.Infrastructure.Framework.Commons;
#endregion

namespace Buildron.Domain
{
	
	/// <summary>
	/// Build user service.
	/// </summary>
	public static class BuildUserService
	{
		#region Fields
		private static IBuildUserAvatarProvider s_humanUserAvatarProvider;
		private static IBuildUserAvatarProvider s_nonHumanUserAvatarProvider;
		#endregion
				
		#region Methods
		public static void Initialize (IBuildUserAvatarProvider humanUserAvatarProvider, IBuildUserAvatarProvider nonHumanAvatarProvider)
		{
			s_humanUserAvatarProvider = humanUserAvatarProvider;
			s_nonHumanUserAvatarProvider = nonHumanAvatarProvider;
		}
		
		public static void GetUserPhoto (BuildUser user, Action<Texture2D> photoReceived)
		{
			if (user != null) {
				if (user.Kind == BuildUserKind.Human) {
					s_humanUserAvatarProvider.GetUserPhoto (user, photoReceived);	
				} else {
					s_nonHumanUserAvatarProvider.GetUserPhoto (user, photoReceived);	
				}	
			}
		}
		#endregion
	}
}