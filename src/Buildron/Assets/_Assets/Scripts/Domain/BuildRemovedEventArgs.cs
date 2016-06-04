using UnityEngine;
using System.Collections;
using System;

namespace Buildron.Domain
{
	public class BuildRemovedEventArgs : EventArgs
	{
		#region Constructors
		public BuildRemovedEventArgs (Build build)
		{
			Build = build;
		}
		#endregion

		#region Properties
		public Build Build { get; private set; }
		#endregion
	}
}