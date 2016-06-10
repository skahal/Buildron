#region Usings
using UnityEngine;
using System;
#endregion

namespace Buildron.Domain
{
	/// <summary>
	/// Defines a provider to user avatar.
	/// </summary>
	public interface IBuildUserAvatarProvider
	{
        /// <summary>
        /// Gets the user photo.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="photoReceived">The photo received callback.</param>
        void GetUserPhoto(BuildUser user, Action<Texture2D> photoReceived);
	}
}