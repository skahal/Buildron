using System;
using Buildron.Domain.Mods;
using UnityEngine;

namespace Buildron.Infrastructure.GameObjectsProxies
{
	public class ModGameObjectPool : SHPoolBase
	{

		public Func<GameObject> GameObjectFactory { get; set; }
		#region implemented abstract members of SHPoolBase

		protected override UnityEngine.GameObject CreateObject ()
		{
			return GameObjectFactory ();
		}

		protected override void DisableObject (UnityEngine.GameObject goInPool)
		{
			goInPool.SetActive (false);
		}

		protected override void EnableObject (UnityEngine.GameObject goInPool)
		{
			goInPool.SetActive (true);
		}

		protected override bool IsObjectEnabled (UnityEngine.GameObject goInPool)
		{
			return goInPool.activeSelf;
		}

		#endregion
	}
}

