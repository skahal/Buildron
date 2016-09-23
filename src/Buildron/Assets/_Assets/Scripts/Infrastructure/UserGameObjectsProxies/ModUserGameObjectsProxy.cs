using System;
using Buildron.Domain.Mods;
using UnityEngine;
using System.Linq;

namespace Buildron.Infrastructure.UserGameObjectsProxies
{
	public class ModUserGameObjectsProxy : IUserGameObjectsProxy
	{
		public IUserController[] GetAll ()
		{
			return GameObject.FindGameObjectsWithTag ("User")
				.Select (b => b.GetComponent<IUserController> ())
				.Where(b => b != null)
				.ToArray ();
		}
	}
}

