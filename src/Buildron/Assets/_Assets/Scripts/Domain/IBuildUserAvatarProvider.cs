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
		void GetUserPhoto(BuildUser user, Action<Texture2D> photoReceived);
	}
}