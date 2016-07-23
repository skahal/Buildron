using System;
using Skahal.Logging;

namespace Buildron.Domain.Mods
{
	public class LogGameObjectsProxy : IGameObjectsProxy
	{
		private IGameObjectsProxy m_underlying;
		private ISHLogStrategy m_log;

		public LogGameObjectsProxy(IGameObjectsProxy underlying, ISHLogStrategy log)
		{
			m_underlying = underlying;
			m_log = log;
		}

	
		#region IGameObjectsProxy implementation

		public TComponent Create<TComponent> (string name = null, Action<UnityEngine.GameObject> gameObjectCreatedCallback = null) where TComponent : UnityEngine.Component
		{	
			m_log.Debug ("Creating game object of type '{0}'...", typeof(TComponent).Name);
			return m_underlying.Create<TComponent> (name, gameObjectCreatedCallback);
		}

		public UnityEngine.GameObject Create (UnityEngine.Object prefab)
		{
			m_log.Debug ("Creating game object using the prefab '{0}'...", prefab.name);
			return m_underlying.Create (prefab);
		}

		public UnityEngine.GameObject Create (string name)
		{
			m_log.Debug ("Creating game object with name '{0}'...", name);
			return m_underlying.Create (name);
		}

		public UnityEngine.MonoBehaviour AddComponent (UnityEngine.GameObject container, string componentTypeName)
		{
			m_log.Debug ("Adding component of type with name '{0}' to game object '{1}'...", componentTypeName, container.name);
			return m_underlying.AddComponent (container, componentTypeName);
		}

		public TComponent AddComponent<TComponent> (UnityEngine.GameObject container)
			where TComponent : UnityEngine.Component
		{
			m_log.Debug ("Adding component of type '{0}' to game object '{1}'...", typeof(TComponent).Name, container.name);
			return m_underlying.AddComponent<TComponent>(container);
		}

		#endregion
	}
}

