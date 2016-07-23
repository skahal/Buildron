using System;
using Skahal.Logging;

namespace Buildron.Domain.Mods
{
	public class LogDataProxy : IDataProxy
	{
		private IDataProxy m_underlying;
		private ISHLogStrategy m_log;

		public LogDataProxy(IDataProxy underlying, ISHLogStrategy log)
		{
			m_underlying = underlying;
			m_log = log;
		}
			
		#region IDataProxy implementation

		public void SaveValue<TValue> (string key, TValue value)
		{
			m_log.Debug ("Saving key '{0}' with value '{1}'...", key, value);
			m_underlying.SaveValue<TValue> (key, value);
		}

		public TValue GetValue<TValue> (string key)
		{
			m_log.Debug ("Getting key '{0}'...", key);
			var value = m_underlying.GetValue<TValue> (key);
			m_log.Debug ("Value for key '{0}' is value '{1}'.", key, value);

			return value;
		}

		public void RemoveValue<TValue> (string key)
		{
			m_log.Debug ("Removing key '{0}',..", key);
			m_underlying.RemoveValue<TValue> (key);
		}

		#endregion
	}
}

