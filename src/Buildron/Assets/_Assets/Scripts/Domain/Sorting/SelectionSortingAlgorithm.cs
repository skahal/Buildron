#region Usings
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;
#endregion

namespace Buildron.Domain.Sorting
{
	/// <summary>
	/// Selection sorting algorithm: http://www.sorting-algorithms.com/selection-sort
	/// </summary>
	public class SelectionSortingAlgorithm<TItem> : SortingAlgorithmBase<TItem> where TItem : System.IComparable<TItem>
	{
		public SelectionSortingAlgorithm () : base("Selection Sort")
		{
			
		}
		
		protected override IEnumerator PerformSort (IList<TItem> items)
		{
			int indexOfMin = 0;
			
			for (int iterator = 0; iterator < items.Count - 1; iterator++) {
				
				indexOfMin = iterator;
				
				for (int index = iterator+1; index < items.Count; index++) {
					if (IsLowerThan(items [index], items [indexOfMin]))
						indexOfMin = index;
				}
				
				yield return Swap (items, iterator, indexOfMin);
			}							
		}
	}
}