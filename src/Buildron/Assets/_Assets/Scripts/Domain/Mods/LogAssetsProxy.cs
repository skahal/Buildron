using System;
using Buildron.Domain.Mods;
using UnityEngine;
using Skahal.Logging;

namespace Buildron.Domain.Mods
{
	public class LogAssetsProxy : IAssetsProxy
	{
		private IAssetsProxy m_underlying;
		private ISHLogStrategy m_log;

		public LogAssetsProxy(IAssetsProxy underlying, ISHLogStrategy log)
		{
			m_underlying = underlying;
			m_log = log;
		}

		public UnityEngine.Object Load (string assetName)
		{
			m_log.Debug ("Loading asset '{0}'...", assetName);
			var asset = m_underlying.Load (assetName);

			if (asset == null) {
				m_log.Warning ("There is no asset with name '{0}'. Do you have associate it to the mod AssetBundle?", assetName);
			} else {
				m_log.Debug ("Asset loaded: {0}", asset);
			}

			return asset;
		}
	}
}

