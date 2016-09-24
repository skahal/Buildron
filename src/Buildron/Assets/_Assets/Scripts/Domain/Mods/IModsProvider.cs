using System;
using System.Collections.Generic;

namespace Buildron.Domain.Mods
{
	public interface IModsProvider
	{
		IEnumerable<ModInfo> GetModInfos();
		ModInstanceInfo CreateInstance(ModInfo modInfo);
        void DestroyInstance(ModInstanceInfo modInstanceInfo);
	}
}