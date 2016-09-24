using System;
using Skahal.Common;
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
            Throw.AnyNull(new { prefab });
            m_log.Debug ("Creating game object using the prefab '{0}'...", prefab.name);

            try
            {
                var go = m_underlying.Create(prefab);
                m_log.Debug("Prefab '{0}' created.", prefab.name);

                return go;
            }
            catch(Exception ex)
            {
                m_log.Error("Error creating prefab '{0}': {1}.{2}", prefab.name, ex.Message, ex.StackTrace);
                throw;
            }
		}

		public UnityEngine.GameObject Create (string name)
		{
			m_log.Debug ("Creating game object with name '{0}'...", name);
			return m_underlying.Create (name);
		}

		public UnityEngine.MonoBehaviour AddComponent (UnityEngine.GameObject container, string componentTypeName)
		{
            Throw.AnyNull(new { container });

            m_log.Debug ("Adding component of type with name '{0}' to game object '{1}'...", componentTypeName, container.name);
			return m_underlying.AddComponent (container, componentTypeName);
		}

		public TComponent AddComponent<TComponent> (UnityEngine.GameObject container)
			where TComponent : UnityEngine.Component
		{
            Throw.AnyNull(new { container });

            m_log.Debug ("Adding component of type '{0}' to game object '{1}'...", typeof(TComponent).Name, container.name);
			return m_underlying.AddComponent<TComponent>(container);
		}

		#endregion
	}
}

