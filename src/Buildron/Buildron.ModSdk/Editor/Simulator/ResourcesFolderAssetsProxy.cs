using System;
using Buildron.Domain.Mods;
using UnityEngine;

namespace Buildron.Infrastructure.AssetsProxies
{
	public class ResourcesFolderAssetsProxy : IAssetsProxy
	{
		public UnityEngine.Object Load (string assetName)
		{
			return Resources.Load (assetName);
		}
	}
}

