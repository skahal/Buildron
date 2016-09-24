using System;

namespace Buildron.Domain.Mods
{
	public class ModInfo
	{
		public ModInfo (string name)
		{
			Name = name;
		}

		public string Name { get; private set; }
	}
}

