using UnityEngine;
using System.Collections;
using System;

namespace Buildron.Domain
{
	/// <summary>
	/// Build updated event arguments.
	/// </summary>
	public class BuildUpdatedEventArgs : BuildEventArgsBase
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.BuildUpdatedEventArgs"/> class.
		/// </summary>
		/// <param name="build">Build.</param>
		public BuildUpdatedEventArgs (Build build) 
			: base (build)
		{
		}
		#endregion
	}
}