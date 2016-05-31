#region Usings
using Buildron.Domain;
using UnityEngine;
using System.Xml;
using System;
using Skahal.Logging;
using System.Collections.Generic;
#endregion

namespace Buildron.Infrastructure.UserBuildAvatarProviders
{
	public class GravatarUserBuildAvatarProvider : IBuildUserAvatarProvider
	{
		#region Fields
		private static Dictionary<string, Texture2D> s_photosCache = new Dictionary<string, Texture2D>();
		#endregion
		
		#region IBuildUserAvatarProvider implementation
		public void GetUserPhoto (BuildUser user, Action<Texture2D> photoReceived)
		{
			if (user == null) {
				return;
			}
			
			lock (s_photosCache) {
				var email = user.Email;
			
				if (!String.IsNullOrEmpty (email)) {
					if (s_photosCache.ContainsKey (email)) {
						photoReceived (s_photosCache [email]);
					} else {
						var mailHash = GetMd5Sum (email);
						var url = string.Format ("http://www.gravatar.com/avatar/{0}.png?s=256&{1}", mailHash, DateTime.Now.Ticks);
				
						var r = Requester.Instance;
			
						r.GetTexture (url, (photo) => 
						{
							lock (s_photosCache) {
								if (!s_photosCache.ContainsKey (email)) {
									s_photosCache.Add (email, photo);
								}
								photoReceived (photo);
							}
						});
					}
				}
			}
		}
		#endregion
		
		#region Private Methods
		private string GetMd5Sum (string strToEncrypt)
		{
			System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding ();
			byte[] bytes = ue.GetBytes (strToEncrypt);

			// encrypt bytes
			System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider ();
			byte[] hashBytes = md5.ComputeHash (bytes);

			// Convert the encrypted bytes back to a string (base 16)
			string hashString = "";

			for (int i = 0; i < hashBytes.Length; i++) {
				hashString += System.Convert.ToString (hashBytes [i], 16).PadLeft (2, '0');
			}

			return hashString.PadLeft (32, '0');
		}
		#endregion
	}
}