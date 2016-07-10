using System;
using Buildron.Domain.Mods;
using UnityEngine;

namespace Buildron.Infrastructure.AssetsLoaders
{
	public class ResourcesFolderAssetsLoader : IAssetsLoader
	{
		public object Load (string assetName)
		{
			return Resources.Load (assetName);
		}
	}
}

