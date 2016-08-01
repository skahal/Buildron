using System;
using System.Linq;
using UnityEngine;
using Buildron.Domain.Mods;

/// <summary>
/// User game object controller extension methods.
/// </summary>
public static class UserGameObjectControllerExtensions
{	
	/// <summary>
	/// Gets an user game object by the user's username model.
	/// </summary>
	/// <returns>The game object.</returns>
	/// <param name="users">The users.</param>
	/// <param name="userName">The username.</param>
	public static GameObject GetByUsername (this IUserController[] users, string userName)
    {
		var controller = users.FirstOrDefault (u => u.Model.UserName.Equals (userName, StringComparison.OrdinalIgnoreCase));
   
		return controller == null ? null : controller.gameObject;
	}
}