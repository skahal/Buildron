using System;
using Skahal.Logging;

namespace Buildron.Domain.Mods
{
	public class LogGameObjectsPoolProxy : IGameObjectsPoolProxy
	{
		private IGameObjectsPoolProxy m_underlying;
		private ISHLogStrategy m_log;

		public LogGameObjectsPoolProxy(IGameObjectsPoolProxy underlying, ISHLogStrategy log)
		{
			m_underlying = underlying;
			m_log = log;
		}


		#region IGameObjectsPoolProxy implementation

		public void CreatePool (string poolName, Func<UnityEngine.GameObject> gameObjectFactory)
		{
			m_log.Debug ("Creation pool '{0}'...", poolName);
			m_underlying.CreatePool (poolName, gameObjectFactory);
			m_log.Debug ("Pool '{0}' created.", poolName);
		}

		public UnityEngine.GameObject GetGameObject (string poolName, float autoDisableTime = 0f)
		{
			m_log.Debug ("Getting game object from pool '{0}'...", poolName);
			return m_underlying.GetGameObject (poolName, autoDisableTime);
		}

		public void ReleaseGameObject (string poolName, UnityEngine.GameObject go)
		{
			m_log.Debug ("Releasing game object '{0}' from pool '{1}'...", go.name, poolName);
			m_underlying.ReleaseGameObject (poolName, go);
		}

		#endregion
	}
}

