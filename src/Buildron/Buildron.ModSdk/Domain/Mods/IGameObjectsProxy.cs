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
        /// Create a new game object with the specified component.
        /// </summary>
        /// <param name="name">The game object name.</param>
        /// <param name="gameObjectCreatedCallback">This callback will be caller right after the game object creation and before the AddComponent method..</param>
        /// <typeparam name="TComponent">The 1st type parameter.</typeparam>
		TComponent Create<TComponent> (string name = null, Action<GameObject> gameObjectCreatedCallback = null)
			where TComponent : Component;

        /// <summary>
        /// Create a new game object using the specified prefab.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
       /// <returns>The game object instance.</returns>
		GameObject Create(UnityEngine.Object prefab);    

		/// <summary>
		/// Create a new game object with the specified name.
		/// </summary>
		/// <param name="name">The game object name.</param>
		GameObject Create(string name);

		/// <summary>
		/// Adds the component to the specified game object.
		/// </summary>
		/// <returns>The component.</returns>
		/// <param name="gameObject">The game object.</param>
		/// <typeparam name="TComponent">The 1st type parameter.</typeparam>
		TComponent AddComponent<TComponent> (GameObject gameObject)
			where TComponent : Component;

		/// <summary>
		/// Adds the component to the specified game object.
		/// </summary>
		/// <remarks>
		/// This method is useful when you want to add a component to your mod game object, but the type of component is not available in your code, like camera effects that are available on Buildron runtime.
		/// </remarks>
		/// <returns>The component.</returns>
		/// <param name="gameObject">The game object.</param>
		/// <param name="componentTypeName">The component type name.</param>
		MonoBehaviour AddComponent (GameObject gameObject, string componentTypeName);
	}
}