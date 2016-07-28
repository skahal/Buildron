using System;
using Buildron.Domain.Builds;
using UnityEngine;

namespace Buildron.Domain.Mods
{
	public interface IBuildController : IGameObjectController
	{
		IBuild Model { get;  }
	}
}