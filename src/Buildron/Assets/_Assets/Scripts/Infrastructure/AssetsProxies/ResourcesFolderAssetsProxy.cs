using System;
using Buildron.Domain.Mods;
using UnityEngine;

namespace Buildron.Infrastructure.AssetsProxies
{
	public class ResourcesFolderAssetsProxy : IAssetsProxy
	{
		public object Load (string assetName)
		{
			return Resources.Load (assetName);
		}
	}
}

