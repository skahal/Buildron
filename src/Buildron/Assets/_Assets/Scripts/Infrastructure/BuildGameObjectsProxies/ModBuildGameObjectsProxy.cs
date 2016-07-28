using System;
using Buildron.Domain.Mods;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Buildron.Infrastructure.BuildGameObjectsProxies
{
	public class ModBuildGameObjectsProxy : IBuildGameObjectsProxy
	{
        #region IBuildGameObjectsProxy implementation
        public IBuildController[] GetAll ()
		{
			return GameObject.FindGameObjectsWithTag ("Build")
				.Select (b => b.GetComponent<IBuildController> ())
				.ToArray ();
		}
		#endregion
	}
}

