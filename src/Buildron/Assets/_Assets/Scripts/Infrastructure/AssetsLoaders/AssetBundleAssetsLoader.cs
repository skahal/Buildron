using System;
using Buildron.Domain.Mods;
using UnityEngine;
using Skahal.Logging;
using System.Reflection;
using System.Linq;

namespace Buildron.Infrastructure.AssetsLoaders
{
	public class AssetBundleAssetsLoader : IAssetsLoader
	{
		private Assembly m_assembly;
		private Type[] m_monoBehavioursTypes;
		private AssetBundle m_assetBundle;

		public AssetBundleAssetsLoader(Assembly assembly, AssetBundle assetBundle)
		{
			m_assembly = assembly;
			m_monoBehavioursTypes = m_assembly.GetTypes ().Where (t => typeof(MonoBehaviour).IsAssignableFrom (t)).ToArray ();
			m_assetBundle = assetBundle;
		}

		public object Load (string assetName)
		{
			var asset = m_assetBundle.LoadAsset (assetName);
//			var assetAsGO = asset as GameObject;
//
//			assetAsGO
//
//			if (assetAsGO != null) {
//				foreach (var t in m_monoBehavioursTypes) {
//					var components = assetAsGO.GetComponents (t);
//					SHLog.Debug ("Found {0} components of type {1}", components.Length, t.Name);
//
//					foreach (var c in components) {
//						SHLog.Debug ("Component: {0}", c);
//					}
//				}
//			}

			return asset;
		}
	}
}

