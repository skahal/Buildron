using System;
using Buildron.Domain.Mods;
using UnityEngine;
using Skahal.Common;
using System.Collections.Generic;
using Skahal.Threading;
using Skahal.Logging;

namespace Buildron.Infrastructure.GameObjectsProxies
{
	/// <summary>
	/// Mod game objects pool proxy.
	/// </summary>
	public class ModGameObjectsPoolProxy : IGameObjectsPoolProxy
	{
		#region Fields
		private readonly ModInfo m_modInfo;
		private readonly IGameObjectsProxy m_goProxy;
		#endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Buildron.Infrastructure.GameObjectsPoolProxies.ModGameObjectsPoolProxy"/> class.
        /// </summary>
        /// <param name="gameObjectsProxy">The game objects proxy.</param>
        public ModGameObjectsPoolProxy(ModInfo modInfo, IGameObjectsProxy gameObjectsProxy)
		{
			Throw.AnyNull (new { modInfo, gameObjectsProxy });

			m_modInfo = modInfo;
			m_goProxy = gameObjectsProxy;
		}
        #endregion

        #region Methods
		public void CreatePool (string poolName, Func<GameObject> gameObjectFactory)
		{
			SHPoolsManager.AddPool <ModGameObjectPool> ((pool) => {
				pool.Name = GetModPoolName (poolName);
				pool.Size = 0;
				pool.GameObjectFactory = gameObjectFactory;
			});
		}

		public GameObject GetGameObject (string poolName, float autoDisableTime = 0)
		{
			var go = SHPoolsManager.GetGameObject (GetModPoolName(poolName));

			if (autoDisableTime > 0) {
				SHCoroutine.Start(
					autoDisableTime, 
					() => SHPoolsManager.ReleaseGameObject(GetModPoolName(poolName), go));
			}

			return go;
		}

		public void ReleaseGameObject (string poolName, GameObject go)
		{
			SHPoolsManager.ReleaseGameObject (GetModPoolName(poolName), go);
		}

		private string GetModPoolName(string poolName)
		{
			return "{0}_{1}".With (m_modInfo.Name, poolName);
		}
        #endregion
    }
}
