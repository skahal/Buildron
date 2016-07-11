using System;

namespace Buildron.Domain.Mods
{
	public class ModInstanceInfo
	{
		public ModInstanceInfo (IMod mod, ModInfo info, IAssetsProxy assets)
		{
			Mod = mod;
			Info = info;
			Assets = assets;
		}

		public IMod Mod { get; private set; }
		public ModInfo Info { get; private set; }
		public IAssetsProxy Assets { get; set; }
	}
}

