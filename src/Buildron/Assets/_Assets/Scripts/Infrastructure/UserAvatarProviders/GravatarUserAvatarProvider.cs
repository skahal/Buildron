#region Usings
using Buildron.Domain;
using UnityEngine;
using System.Xml;
using System;
using Skahal.Logging;
using System.Collections.Generic;
#endregion

namespace Buildron.Infrastructure.UserAvatarProviders
{
    /// <summary>
    /// User avatar that use user e-mail to look for an Gravatar image.
    /// </summary> 
    public class GravatarUserAvatarProvider : WebUserAvatarProviderBase
    {
        #region Methods
        protected override string BuildImageUrl(User user)
        {
			var email = user.Email;

			if (String.IsNullOrEmpty (email)) {
				return null;
			}
				
            var mailHash = GetMd5Sum(email);
            return "http://www.gravatar.com/avatar/{0}.png?s=256&{1}&d=404".With(mailHash, DateTime.Now.Ticks);
        }

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