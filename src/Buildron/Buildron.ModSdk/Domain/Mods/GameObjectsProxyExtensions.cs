using System;
using UnityEngine;
using Buildron.Domain.Mods;

	public static class GameObjectsProxyExtensions
	{
		public static TComponent Create<TComponent> (this IGameObjectsProxy proxy, string name = null, Transform parent = null) 
			where TComponent : Component
		{
			return proxy.Create<TComponent>(name, (go) => {
				go.transform.parent = parent;
			});
		}

		public static TComponent Create<TComponent> (this IGameObjectsProxy proxy, string name, Vector3 position) 
			where TComponent : Component
		{
			return proxy.Create<TComponent>(name, (go) => {
				go.transform.position = position;
			});
		}

		public static TComponent Create<TComponent> (this IGameObjectsProxy proxy, Vector3 position) 
			where TComponent : Component
		{
			return proxy.Create<TComponent>(null, (go) => {
				go.transform.position = position;
			});
		}

		public static GameObject Create(this IGameObjectsProxy proxy, UnityEngine.Object prefab, Transform parent = null)
		{
			var go = proxy.Create (prefab);
			go.transform.parent = parent;

			return go;
		}

		public static GameObject Create(this IGameObjectsProxy proxy, string name, Transform parent = null)
		{
			var go = proxy.Create (name);
			go.transform.parent = parent;

			return go;
		}
	}

