using System;
using UnityEngine;

namespace Buildron.Domain.Mods
{
	/// <summary>
	/// Defines an interface to a game objects proxy.
	/// </summary>
	public interface IGameObjectsProxy
	{
        /// <summary>
        /// Create the game object.
        /// </summary>
        /// <param name="name">The game object name.</param>
        /// <param name="parent">The parent game object.</param>
        /// <typeparam name="TComponent">The main component of game object.</typeparam>
		TComponent Create<TComponent> (string name = null, Action<GameObject> gameObjectCreatedCallback = null)
			where TComponent : Component;

        /// <summary>
        /// Creates the game object using the specified prefab.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <param name="parent">The parent game object.</param>
        /// <returns>The game object instance.</returns>
		GameObject Create(UnityEngine.Object prefab);    

		GameObject Create(string name);

		TComponent AddComponent<TComponent> (GameObject container)
			where TComponent : Component;

		MonoBehaviour AddComponent (GameObject container, string componentTypeName);
	}
}