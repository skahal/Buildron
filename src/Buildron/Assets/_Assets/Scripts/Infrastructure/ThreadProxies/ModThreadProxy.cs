using System;
using Buildron.Domain.Mods;
using Skahal.Logging;

namespace Buildron.Infrastructure.ThreadProxies
{
	public class ModThreadProxy : IThreadProxy
	{
		private ISHLogStrategy m_log;
		private UnityMainThreadDispatcher m_dispatcher;

		public ModThreadProxy(ISHLogStrategy log, IGameObjectsProxy goProxy)
		{
			m_log = log;
			goProxy.Create<UnityMainThreadDispatcher>();
			m_dispatcher = UnityMainThreadDispatcher.Instance();
		}

		public void RunOnMainThread(Action action)
		{
			m_log.Debug("Enqueuing action to run on main thread...");
			m_dispatcher.Enqueue(action);
		}
	}
}