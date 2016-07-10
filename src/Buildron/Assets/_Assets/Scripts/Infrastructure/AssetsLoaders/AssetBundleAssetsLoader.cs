using System;
using Buildron.Domain.Mods;
using UnityEngine;

namespace Buildron.Infrastructure.AssetsLoaders
{
	public class AssetBundleAssetsLoader : IAssetsLoader
	{
		private AssetBundle m_assetBundle;

		public AssetBundleAssetsLoader(AssetBundle assetBundle)
		{
			m_assetBundle = assetBundle;
		}

		public object Load (string assetName)
		{
			return m_assetBundle.LoadAsset (assetName);
		}
	}
}

