using UnityEngine;
using System.Collections;
using System;

namespace Buildron.Domain
{
	public class BuildFoundEventArgs : BuildEventArgsBase
	{
		#region Constructors
		public BuildFoundEventArgs (Build build) 
			: base (build)
		{
		}
		#endregion
	}
}