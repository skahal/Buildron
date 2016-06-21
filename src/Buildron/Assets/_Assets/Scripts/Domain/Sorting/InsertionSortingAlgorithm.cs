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
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Sorting.InsertionSortingAlgorithm`1"/> class.
		/// </summary>
		public InsertionSortingAlgorithm () : base("Insertion Sort")
		{			
		}
		#endregion

		#region Methods
		/// <summary>
		/// Performs the sort.
		/// </summary>
		/// <returns>The sort.</returns>
		/// <param name="items">Items.</param>
		protected override IEnumerator PerformSort (IList<TItem> items)
		{
			for (int i = 1; i < items.Count; i++) {
				for (int k = i; k > 0 && IsLowerThan(items[k], items[k-1]); k--) {
					yield return Swap (items, k, k - 1);
				}
			}	

			OnSortingEnded (EventArgs.Empty);
		}
		#endregion
	}
}