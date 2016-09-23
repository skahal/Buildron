using System;
using Buildron.Domain.Mods;
using UnityEngine;
using Skahal.Logging;

namespace Buildron.Domain.Mods
{
	public class LogPreferencesProxy : LogDataProxy, IPreferencesProxy
	{
		private IPreferencesProxy m_underlying;
		private ISHLogStrategy m_log;

		public LogPreferencesProxy(IPreferencesProxy underlying, ISHLogStrategy log)
			: base(underlying, log)
		{
			m_underlying = underlying;
			m_log = log;
		}


		#region IPreferencesProxy implementation
		public void Register (params Preference[] preferences)
		{
			m_log.Debug ("Register {0} preferences:", preferences.Length);

			foreach (var p in preferences) {
				m_log.Debug ("{0} ({1}): {2}", p.Name, p.Kind, p.DefaultValue);
			}

			m_underlying.Register (preferences);
		}
		#endregion
	}
}

