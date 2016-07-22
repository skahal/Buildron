using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Buildron.Domain.Mods
{
    public interface IGameObjectsPoolProxy
    {
		void CreatePool (string poolName, Func<GameObject> gameObjectFactory);
		GameObject GetGameObject (string poolName, float autoDisableTime = 0);
		void ReleaseGameObject (string poolName, GameObject go);
//        TComponent Get<TComponent>(string name = null, Transform parent = null, float autoDisableSeconds = 0) where TComponent : Component;
//		GameObject Get(UnityEngine.Object prefab, Transform parent = null, float autoDisableSeconds = 0);
//        void Release(GameObject gameObject);
    }
}
