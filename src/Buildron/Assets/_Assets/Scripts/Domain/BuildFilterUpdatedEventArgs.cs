using System;

namespace Buildron.Domain
{
	/// <summary>
	/// Build filter updated event arguments.
	/// </summary>
	public class BuildFilterUpdatedEventArgs : EventArgs
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.BuildFilterUpdatedEventArgs"/> class.
		/// </summary>
		/// <param name="filter">Filter.</param>
		public BuildFilterUpdatedEventArgs (BuildFilter filter)
		{
			Filter = filter;
		}
		#endregion
		
		#region Properties
		/// <summary>
		/// Gets the filter.
		/// </summary>
		/// <value>The filter.</value>
		public BuildFilter Filter { get; private set; }
		#endregion
	}
}