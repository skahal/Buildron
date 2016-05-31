using Skahal.Common;
using UnityEngine;
using Buidron.Domain;

namespace Buildron.Domain.Sorting
{	
	/// <summary>
	/// Sorting algorithm factory.
	/// </summary>
	public static class SortingAlgorithmFactory 
	{
		#region Constants
		public const float SwappingTime = 0.4f;
		#endregion
		
		public static ISortingAlgorithm<TItem> CreateSortingAlgorithm<TItem> (SortingAlgorithmType algorithmType) where TItem : System.IComparable<TItem>
		{
			var r = Random.Range (0, 4);
		
			switch (r) {
		
			case 0:
				return new InsertionSortingAlgorithm<TItem> ();
				
			case 1:
				return new SelectionSortingAlgorithm<TItem> ();
			
			case 2:
				return new ShellSortingAlgorithm<TItem> ();
				
			default:
				return new BubbleSortingAlgorithm<TItem> ();
			}
		}
	}
}