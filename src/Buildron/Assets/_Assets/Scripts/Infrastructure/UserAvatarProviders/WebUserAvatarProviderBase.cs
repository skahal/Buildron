using Buildron.Domain;
using UnityEngine;
using System;
using System.Collections.Generic;
using Buildron.Domain.Users;

namespace Buildron.Infrastructure.UserAvatarProviders
{
    /// <summary>
    /// A base class for user avatar providers that get images from web.
    /// </summary>
    public abstract class WebUserAvatarProviderBase : IUserAvatarProvider
	{
		#region Fields
		private Dictionary<string, Texture2D> m_photosCache = new Dictionary<string, Texture2D>();
		#endregion
		
		#region Methods
		public void GetUserPhoto (IUser user, Action<Texture2D> photoReceived)
		{
			if (user == null) {
				return;
			}
			
			lock (m_photosCache) {
				var cacheKey = user.UserName;
			
				if (!String.IsNullOrEmpty (cacheKey)) {
					if (m_photosCache.ContainsKey (cacheKey)) {
						photoReceived (m_photosCache [cacheKey]);
					} else {
                        var url = BuildImageUrl(user);				
						var r = Requester.Instance;
			
						if (String.IsNullOrEmpty (url)) {
							SetCache (cacheKey, null);
							photoReceived (null);
						} else {
							r.GetTexture (
								url, 
								(photo) => {              
									// Success.     
									SetCache (cacheKey, photo);
									photoReceived (photo);
								},
								(e) => {
									// Error.
									SetCache (cacheKey, null);
									photoReceived (null);
								});
						}
					}
				}
			}
		}

        /// <summary>
        /// Builds the image URL.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The url to get the image.</returns>
        protected abstract string BuildImageUrl(IUser user);

		private void SetCache(string cacheKey, Texture2D photo)
		{
			lock (m_photosCache) {
				if (!m_photosCache.ContainsKey (cacheKey)) {                                    
					m_photosCache.Add (cacheKey, photo);
				}
			}
		}
		#endregion		
	}
}