using Buildron.Domain.Mods;
using UnityEngine;

/// <summary>
/// Mod context extension methods.
/// </summary>
public static class ModContextExtensions
{
	/// <summary>
	/// Create a new game object from prefab name.
	/// </summary>
	/// <returns>The game object from prefab.</returns>
	/// <param name="context">Context.</param>
	/// <param name="prefabName">The prefab name.</param>
	public static GameObject CreateGameObjectFromPrefab (this IModContext context, string prefabName)
	{
		var prefab = context.Assets.Load (prefabName);

		return context.GameObjects.Create (prefab);
	}
}
