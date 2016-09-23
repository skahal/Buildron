using System;
using Buildron.Domain.Mods;
using Skahal.Logging;

namespace Buildron.Infrastructure.AssetsProxies
{
	public class EmptyAssetsProxy : IAssetsProxy
	{
		private ISHLogStrategy m_log;

		public EmptyAssetsProxy(ISHLogStrategy log)
		{
			m_log = log;
			m_log.Debug ("No asset bundle loaded.");
		}

		public UnityEngine.Object Load (string assetName)
		{
			m_log.Warning ("Cannot load asset '{0}', because there is no asset bundle for this mod.", assetName);
		
			return null;
		}
	}
}

