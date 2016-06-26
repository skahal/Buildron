using System;
using Buildron.Domain.Sorting;

namespace Buildron.Domain.Builds
{
	/// <summary>
	/// Build sort updated event arguments.
	/// </summary>
	public class BuildSortUpdatedEventArgs : EventArgs
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Builds.BuildSortUpdatedEventArgs"/> class.
		/// </summary>
		/// <param name="sortingAlgorithm">Sorting algorithm.</param>
		/// <param name="sortBy">Sort by.</param>
		public BuildSortUpdatedEventArgs (ISortingAlgorithm<Build> sortingAlgorithm, SortBy sortBy)
		{
			SortingAlgorithm = sortingAlgorithm;
			SortBy = sortBy;
		}
		#endregion
		
		#region Properties
		/// <summary>
		/// Gets the sorting algorithm.
		/// </summary>
		/// <value>The sorting algorithm.</value>
		public ISortingAlgorithm<Build> SortingAlgorithm { get; private set; }

		/// <summary>
		/// Gets the sort by.
		/// </summary>
		/// <value>The sort by.</value>
		public SortBy SortBy { get; private set; }
		#endregion
	}
}