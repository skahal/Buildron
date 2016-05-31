using System;
using Buildron.Domain.Sorting;
#region Usings
#endregion

namespace Buidron.Domain
{
	public class BuildSortUpdatedEventArgs : EventArgs
	{
		#region Constructors
		public BuildSortUpdatedEventArgs (SortingAlgorithmType sortingAlgorithm, SortBy sortBy)
		{
			SortingAlgorithm = sortingAlgorithm;
			SortBy = sortBy;
		}
		#endregion
		
		#region Properties
		public SortingAlgorithmType SortingAlgorithm { get; private set; }
		public SortBy SortBy { get; private set; }
		#endregion
	}
}