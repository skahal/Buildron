using System;

namespace Buildron.Domain.Mods
{
	/// <summary>
	/// Defines an interface to an assets proxy.
	/// </summary>
	public interface IAssetsProxy
	{
		/// <summary>
		/// Load an asset..
		/// </summary>
		/// <param name="assetName">The asset name.</param>
		object Load(string assetName);
	}
}