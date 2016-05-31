#region Usings
using System;
using System.Collections.Generic;
using Buildron.Domain;
using UnityEngine;
#endregion

namespace Buildron.Infrastructure.UserBuildAvatarProviders
{
	public class StaticUserBuildAvatarProvider : IBuildUserAvatarProvider
	{
		#region Fields
		private Dictionary<BuildUserKind, Texture2D> m_photosByKind = new Dictionary<BuildUserKind, Texture2D> ();
		#endregion
		
		#region IBuildUserAvatarProvider implementation
		public void GetUserPhoto (BuildUser user, Action<Texture2D> photoReceived)
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
		public void AddPhoto(BuildUserKind userKind, Texture2D photo)
		{
			m_photosByKind.Add(userKind, photo);
		}
		#endregion
	}
}