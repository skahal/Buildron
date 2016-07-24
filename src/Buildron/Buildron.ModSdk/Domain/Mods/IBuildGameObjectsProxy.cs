using System;
using UnityEngine;
using System.Collections.Generic;
using Buildron.Controllers.Builds;

namespace Buildron.Domain.Mods
{
	public interface IBuildGameObjectsProxy
	{
		IBuildController[] GetAll();
	}
}

