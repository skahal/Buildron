#region Usings
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;
#endregion

namespace Buildron.Domain.Sorting
{
	/// <summary>
	/// Insertion sorting algorithm: http://www.sorting-algorithms.com/insertion-sort
	/// </summary>
	public class InsertionSortingAlgorithm<TItem> : SortingAlgorithmBase<TItem> where TItem : System.IComparable<TItem>
	{
		public InsertionSortingAlgorithm () : base("Insertion Sort")
		{			
		}
		
		protected override IEnumerator PerformSort (IList<TItem> items)
		{
			for (int i = 1; i < items.Count; i++) {
				for (int k = i; k > 0 && IsLowerThan(items[k], items[k-1]); k--) {
					yield return Swap (items, k, k - 1);
				}
			}	

			OnSortingEnded (EventArgs.Empty);
		}
	}
}