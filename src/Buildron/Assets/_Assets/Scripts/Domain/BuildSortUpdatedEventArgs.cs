using System;
using Buildron.Domain.Sorting;

namespace Buildron.Domain
{
	public class BuildSortUpdatedEventArgs : EventArgs
	{
		#region Constructors
		public BuildSortUpdatedEventArgs (ISortingAlgorithm<Build> sortingAlgorithm, SortBy sortBy)
		{
			SortingAlgorithm = sortingAlgorithm;
			SortBy = sortBy;
		}
		#endregion
		
		#region Properties
		public ISortingAlgorithm<Build> SortingAlgorithm { get; private set; }
		public SortBy SortBy { get; private set; }
		#endregion
	}
}