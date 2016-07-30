using System;
using UnityEngine;
using Buildron.Domain.Mods;

public static class ModContextExtensions
{
	public static GameObject CreateGameObjectFromPrefab (this IModContext context, string prefabName)
	{
		var prefab = context.Assets.Load (prefabName);

		return context.GameObjects.Create (prefab);
	}
}
