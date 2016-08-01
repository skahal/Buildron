using UnityEngine;
using Buildron.Domain.Mods;

/// <summary>
/// Game objects proxy extension methods.
/// </summary>
public static class GameObjectsProxyExtensions
{
	/// <summary>
	/// Create a new game object.
	/// </summary>
	/// <param name="proxy">Proxy.</param>
	/// <param name="name">The game object name.</param>
	/// <param name="parent">The game object parent.</param>
	/// <typeparam name="TComponent">The component type.</typeparam>
	public static TComponent Create<TComponent> (this IGameObjectsProxy proxy, string name = null, Transform parent = null) 
			where TComponent : Component
	{
		return proxy.Create<TComponent> (name, (go) => {
			go.transform.parent = parent;
		});
	}

	/// <summary>
	/// Create a new game object.
	/// </summary>
	/// <param name="proxy">Proxy.</param>
	/// <param name="name">The game object name.</param>
	/// <param name="position">The game object position.</param>
	/// <typeparam name="TComponent">The component type.</typeparam>
	public static TComponent Create<TComponent> (this IGameObjectsProxy proxy, string name, Vector3 position) 
			where TComponent : Component
	{
		return proxy.Create<TComponent> (name, (go) => {
			go.transform.position = position;
		});
	}

	/// <summary>
	/// Create a new game object with the specified component.
	/// </summary>
	/// <param name="proxy">Proxy.</param>
	/// <param name="position">The game object position.</param>
	/// <typeparam name="TComponent">The component type.</typeparam>
	public static TComponent Create<TComponent> (this IGameObjectsProxy proxy, Vector3 position) 
			where TComponent : Component
	{
		return proxy.Create<TComponent> (null, (go) => {
			go.transform.position = position;
		});
	}

	/// <summary>
	/// Create a new game object using the specified prefab.
	/// </summary>
	/// <param name="proxy">Proxy.</param>
	/// <param name="prefab">The prefab.</param>
	/// <param name="parent">The game object parent.</param>
	public static GameObject Create (this IGameObjectsProxy proxy, UnityEngine.Object prefab, Transform parent = null)
	{
		var go = proxy.Create (prefab);
		go.transform.parent = parent;

		return go;
	}

	/// <summary>
	/// Create a new game object.
	/// </summary>
	/// <param name="proxy">Proxy.</param>
	/// <param name="name">The game object name.</param>
	/// <param name="parent">The game object parent.</param>
	public static GameObject Create (this IGameObjectsProxy proxy, string name, Transform parent = null)
	{
		var go = proxy.Create (name);
		go.transform.parent = parent;

		return go;
	}
}