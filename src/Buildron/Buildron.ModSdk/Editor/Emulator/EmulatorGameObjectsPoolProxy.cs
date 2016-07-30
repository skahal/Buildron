using System;
using Buildron.Domain.Mods;

public class EmulatorGameObjectsPoolProxy : IGameObjectsPoolProxy
{
	#region IGameObjectsPoolProxy implementation

	public void CreatePool (string poolName, Func<UnityEngine.GameObject> gameObjectFactory)
	{
		throw new NotImplementedException ();
	}

	public UnityEngine.GameObject GetGameObject (string poolName, float autoDisableTime = 0f)
	{
		throw new NotImplementedException ();
	}

	public void ReleaseGameObject (string poolName, UnityEngine.GameObject go)
	{
		throw new NotImplementedException ();
	}

	#endregion
}

