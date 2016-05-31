using UnityEngine;
using System.Collections;
using System;

namespace Buildron.Domain
{
	public class BuildUpdatedEventArgs : EventArgs
	{
		#region Constructors
		public BuildUpdatedEventArgs (Build build)
		{
			Build = build;
		}
		#endregion
		
		#region Properties
		public Build Build { get; private set; }
		#endregion
	}
}