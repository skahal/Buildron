using System;
using Buildron.Domain.Mods;
using UnityEngine;
using Skahal.Logging;
using System.Reflection;
using System.Linq;

namespace Buildron.Infrastructure.AssetsProxies
{
	public class AssetBundleAssetsProxy : IAssetsProxy
	{
		private AssetBundle m_assetBundle;

		public AssetBundleAssetsProxy(AssetBundle assetBundle)
		{
			m_assetBundle = assetBundle;
		}

		public object Load (string assetName)
		{
			return m_assetBundle.LoadAsset (assetName);
		}
	}
}