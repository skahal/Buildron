using System;
using System.Linq;
using UnityEngine;
using Buildron.Domain.Mods;

public static class UserGameObjectControllerExtensions
{	
	public static GameObject GetByUsername (this IUserController[] users, string userName)
    {
		var controller = users.FirstOrDefault (u => u.Model.UserName.Equals (userName, StringComparison.OrdinalIgnoreCase));
   
		return controller == null ? null : controller.gameObject;
	}
}
