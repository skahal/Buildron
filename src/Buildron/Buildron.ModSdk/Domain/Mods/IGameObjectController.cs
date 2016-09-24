using Skahal.Common;
using UnityEngine;

namespace Buildron.Domain.Mods
{
	/// <summary>
	/// Defines a basic interface to game object controllers.
	/// </summary>
	/// <remarks>
	/// This is interface is commonly used by mods that want to do something about game objects created by other mods, like builds and users.
	/// </remarks>
	public interface IGameObjectController : IEventSubscriber
	{
		/// <summary>
		/// Gets the game object.
		/// </summary>
		/// <value>The game object.</value>
		GameObject gameObject { get; }

		/// <summary>
		/// Gets the rigidbody.
		/// </summary>
		/// <value>The rigidbody.</value>
		Rigidbody Rigidbody { get; }

		/// <summary>
		/// Gets the center collider.
		/// </summary>
		/// <value>The center collider.</value>
		Collider CenterCollider { get; }

		/// <summary>
		/// Gets the top collider.
		/// </summary>
		/// <value>The top collider.</value>
		Collider TopCollider { get; }

		/// <summary>
		/// Gets the left collider.
		/// </summary>
		/// <value>The left collider.</value>
		Collider LeftCollider { get; }

		/// <summary>
		/// Gets the right collider.
		/// </summary>
		/// <value>The right collider.</value>
		Collider RightCollider { get; }

		/// <summary>
		/// Gets the bottom collider.
		/// </summary>
		/// <value>The bottom collider.</value>
		Collider BottomCollider { get; }
	}
}

