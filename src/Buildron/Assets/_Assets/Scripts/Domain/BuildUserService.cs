#region Usings
using UnityEngine;
using Buildron.Infrastructure.BuildUserAvatarProviders;
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
		private static IBuildUserAvatarProvider[] s_humanUserAvatarProviders;
		private static IBuildUserAvatarProvider[] s_nonHumanUserAvatarProviders;
		#endregion
				
		#region Methods
		public static void Initialize (IBuildUserAvatarProvider[] humanUserAvatarProviders, IBuildUserAvatarProvider[] nonHumanAvatarProviders)
		{
			s_humanUserAvatarProviders = humanUserAvatarProviders;
			s_nonHumanUserAvatarProviders = nonHumanAvatarProviders;
		}
		
		public static void GetUserPhoto (BuildUser user, Action<Texture2D> photoReceived)
		{
			if (user != null) {
				if (user.Kind == BuildUserKind.Human) {
                    GetUserPhoto(user, photoReceived, s_humanUserAvatarProviders);
				} else {
                    GetUserPhoto(user, photoReceived, s_nonHumanUserAvatarProviders);
                }	
			}
		}

        private static void GetUserPhoto(BuildUser user, Action<Texture2D> photoReceived, IBuildUserAvatarProvider[] providersChain, int providerStartIndex = 0)
        {
            if(providerStartIndex < providersChain.Length)
            {
                providersChain[providerStartIndex].GetUserPhoto(user, (photo) =>
                {
                    if (photo == null)
                    {
                        GetUserPhoto(user, photoReceived, providersChain, ++providerStartIndex);
                    }
                    else
                    {
                        photoReceived(photo);
                    }
                });
            }
            else
            {
                photoReceived(null);
            }
        }
        #endregion
    }
}