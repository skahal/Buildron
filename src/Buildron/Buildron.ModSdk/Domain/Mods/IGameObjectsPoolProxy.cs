using System;
using UnityEngine;

namespace Buildron.Domain.Mods
{
	/// <summary>
	/// Defines an interface to a game objects pool proxy.
	/// </summary>
	/// <remarks>
	/// Mods can use this interface to create any number of pools of game objects.
	/// </remarks>
	public interface IGameObjectsPoolProxy
    {
		/// <summary>
		/// Creates the pool.
		/// </summary>
		/// <returns>The pool.</returns>
		/// <param name="poolName">The name of the pool. The same name used here should be used on GetGameObject and ReleaseGameObject methods.</param>
		/// <param name="gameObjectFactory">The game object factory or how each instance of game object that will used on the pool will be created.</param>
		void CreatePool (string poolName, Func<GameObject> gameObjectFactory);

		/// <summary>
		/// Gets the game object from the pool.
		/// </summary>
		/// <remarks>
		/// If there is a free game object on the pool, this will be used. If there is no free game object, a new one will be created.
		/// </remarks>
		/// <returns>The game object.</returns>
		/// <param name="poolName">The name of the pool.</param>
		/// <param name="autoDisableTime">The time, in seconds, to auto disable the game object and put it back to pool (free to be used again). Zero (0) to not auto disable.</param>
		GameObject GetGameObject (string poolName, float autoDisableTime = 0);

		/// <summary>
		/// Releases the game object back to pool (free to be used again).
		/// </summary>
		/// <returns>The game object.</returns>
		/// <param name="poolName">The name of the pool.</param>
		/// <param name="go">The game object to release.</param>
		void ReleaseGameObject (string poolName, GameObject go);
    }
}
