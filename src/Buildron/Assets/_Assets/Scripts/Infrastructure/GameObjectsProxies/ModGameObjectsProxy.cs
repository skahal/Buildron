using System;
using Buildron.Domain.Mods;
using UnityEngine;
using Skahal.Common;

namespace Buildron.Infrastructure.GameObjectsProxies
{
	/// <summary>
	/// Mod game objects proxy.
	/// </summary>
	public class ModGameObjectsProxy : IGameObjectsProxy
	{
		#region Fields
		private ModInfo m_modInfo;
		private GameObject m_modRoot;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Infrastructure.GameObjectsProxies.ModGameObjectsProxy"/> class.
		/// </summary>
		/// <param name="mod">Mod.</param>
		public ModGameObjectsProxy (ModInfo modInfo)
		{
			Throw.AnyNull (new { modInfo });
			m_modInfo = modInfo;

			var modsRoot = GameObject.Find ("Mods") ?? new GameObject ("Mods");
			m_modRoot = new GameObject (modInfo.Name);
			m_modRoot.transform.parent = modsRoot.transform;
		}      
        #endregion

        #region Methods
        public TComponent Create<TComponent> (string name = null, Transform parent = null) where TComponent : Component
        {
            var go = new GameObject(name ?? typeof(TComponent).Name);
            SetParent(go, parent);

            return go.AddComponent<TComponent>();
        }

        private void SetParent(GameObject go, Transform parent)
        {
            if (parent == null)
            {
                go.transform.parent = m_modRoot.transform;
            }
            else
            {
                go.transform.parent = parent;
            }
        }

        public GameObject Create(UnityEngine.Object prefab, Transform parent = null)
        {
			var go = GameObject.Instantiate(prefab) as GameObject;
            SetParent(go, parent);

            return go;
        }
        #endregion
    }
}
