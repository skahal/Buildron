using UnityEngine;
using System.Collections;
using System;

namespace Buildron.Domain
{
	public class BuildFoundEventArgs : EventArgs
	{
		#region Constructors
		public BuildFoundEventArgs (Build build)
		{
			Build = build;
		}
		#endregion
		
		#region Properties
		public Build Build { get; private set; }
		#endregion
	}
}