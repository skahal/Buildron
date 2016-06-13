#region Usings
using UnityEngine;
using Buildron.Infrastructure.UserAvatarProviders;
using System;
using Skahal.Infrastructure.Framework.Commons;
using System.Collections.Generic;
using Skahal.Common;


#endregion

namespace Buildron.Domain
{
	/// <summary>
	/// User service.
	/// </summary>
	public static class UserService
	{
		#region Events
		/// <summary>
		/// Occurs when an user is found.
		/// </summary>
		public static event EventHandler<UserFoundEventArgs> UserFound;
		#endregion

		#region Fields
		private static IUserAvatarProvider[] s_humanUserAvatarProviders;
		private static IUserAvatarProvider[] s_nonHumanUserAvatarProviders;
		#endregion
				
		#region Properties
		/// <summary>
		/// Gets the users.
		/// </summary>
		public static IList<User> Users { get; private set; }
		#endregion

		#region Methods
		/// <summary>
		/// Initialize the user service.
		/// </summary>
		/// <param name="humanUserAvatarProviders">Human user avatar providers.</param>
		/// <param name="nonHumanAvatarProviders">Non human avatar providers.</param>
		public static void Initialize (IUserAvatarProvider[] humanUserAvatarProviders, IUserAvatarProvider[] nonHumanAvatarProviders)
		{
			Users = new List<User> ();
			s_humanUserAvatarProviders = humanUserAvatarProviders;
			s_nonHumanUserAvatarProviders = nonHumanAvatarProviders;

			BuildsProvider.Initialized += delegate {
				var buildsProvider = BuildsProvider.Current;

				buildsProvider.BuildUpdated += (sender, e) => {
					var user = e.Build.TriggeredBy;

					if (user != null) {
						if (!Users.Contains(user)) {
							Users.Add(user);

							UserFound.Raise(typeof(UserService), new UserFoundEventArgs(user));
						}
					}
				};
			};
		}

		/// <summary>
		/// Gets the user photo.
		/// </summary>
		/// <param name="user">User.</param>
		/// <param name="photoReceived">Photo received callback.</param>
		public static void GetUserPhoto (User user, Action<Texture2D> photoReceived)
		{
			if (user != null) {
				if (user.Kind == UserKind.Human) {
                    GetUserPhoto(user, photoReceived, s_humanUserAvatarProviders);
				} else {
                    GetUserPhoto(user, photoReceived, s_nonHumanUserAvatarProviders);
                }	
			}
		}

        private static void GetUserPhoto(User user, Action<Texture2D> photoReceived, IUserAvatarProvider[] providersChain, int providerStartIndex = 0)
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