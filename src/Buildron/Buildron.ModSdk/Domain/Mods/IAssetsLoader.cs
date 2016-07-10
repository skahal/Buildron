using System;

namespace Buildron.Domain.Mods
{
	public interface IAssetsLoader
	{
		object Load(string assetName);
	}
}