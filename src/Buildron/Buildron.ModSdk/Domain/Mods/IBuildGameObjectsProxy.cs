using System;
using UnityEngine;
using System.Collections.Generic;

namespace Buildron.Domain.Mods
{
	public interface IBuildGameObjectsProxy
	{
		IBuildController[] GetAll();
	}
}

