#region Usings
using System;
using System.Collections.Generic;
using Buildron.Domain;
using Skahal.Common;


#endregion

namespace Buildron.Domain
{
	/// <summary>
	/// Builds provider.
	/// </summary>
	public static class BuildsProvider
	{
		#region Events
		/// <summary>
		/// Occurs when the builds provider is initialized.
		/// </summary>
		public static event EventHandler Initialized;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the current builds provider.
		/// </summary>
		public static IBuildsProvider Current { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Initialize the builds provider.
		/// </summary>
		/// <param name="buildsProvider">Builds provider.</param>
		public static void Initialize (IBuildsProvider buildsProvider)
		{
			Current = buildsProvider;

			Initialized.Raise (typeof(BuildsProvider));
		}
		#endregion
	}
}