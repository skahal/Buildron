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
		private GameObject m_modRoot;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Infrastructure.GameObjectsProxies.ModGameObjectsProxy"/> class.
		/// </summary>
		public ModGameObjectsProxy ()
		{
			m_modRoot = new GameObject ("Mod");
		}        
        #endregion

        #region Methods
		public TComponent Create<TComponent> (string name = null, Action<GameObject> gameObjectCreatedCallback = null) where TComponent : Component
		{
			var go = new GameObject (name ?? typeof(TComponent).Name);
			go.transform.parent = m_modRoot.transform;

			if (gameObjectCreatedCallback != null) {
				gameObjectCreatedCallback (go);
			}

			return go.AddComponent<TComponent> ();
		}

        public GameObject Create(UnityEngine.Object prefab)
        {
            var go = GameObject.Instantiate(prefab) as GameObject;
			go.transform.parent = m_modRoot.transform;

            return go;
        }

		public GameObject Create(string name)
		{
			var go = new GameObject (name);
			go.transform.parent = m_modRoot.transform;

			return go;
		}

		public TComponent AddComponent<TComponent> (GameObject container) where TComponent : Component
		{
			return container.AddComponent<TComponent> ();
		}

		public MonoBehaviour AddComponent (GameObject container, string componentTypeName)
		{
			return container.AddComponent (Type.GetType (componentTypeName)) as MonoBehaviour;
		}
        #endregion
    }
}
