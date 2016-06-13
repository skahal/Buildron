#region Usings
using System;
using System.Collections.Generic;
using Buildron.Domain;
using UnityEngine;
#endregion

namespace Buildron.Infrastructure.UserAvatarProviders
{
    /// <summary>
    /// An IUserAvatarProvider that use a static list of images to avatars.
    /// </summary>
    public class StaticUserAvatarProvider : IUserAvatarProvider
	{
		#region Fields
		private Dictionary<UserKind, Texture2D> m_photosByKind = new Dictionary<UserKind, Texture2D> ();
		#endregion
		
		#region IUserAvatarProvider implementation
		public void GetUserPhoto (User user, Action<Texture2D> photoReceived)
		{
			if (user == null) {
				return;
			}
			
			if (m_photosByKind.ContainsKey (user.Kind)) {
				photoReceived (m_photosByKind [user.Kind]);
			}
		}
		#endregion
		
		#region Methods
		public void AddPhoto(UserKind userKind, Texture2D photo)
		{
			m_photosByKind.Add(userKind, photo);
		}
		#endregion
	}
}