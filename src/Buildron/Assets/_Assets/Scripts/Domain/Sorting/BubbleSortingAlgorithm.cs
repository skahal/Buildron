#region Usings
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;
using UnityEngine;
#endregion

namespace Buildron.Domain.Sorting
{	
	/// <summary>
	/// Bubble sorting algorithm: http://www.sorting-algorithms.com/bubble-sort
	/// </summary>
	public class BubbleSortingAlgorithm<TItem> : SortingAlgorithmBase<TItem> where TItem : System.IComparable<TItem>
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Sorting.BubbleSortingAlgorithm{TItem}"/> class.
		/// </summary>
		public BubbleSortingAlgorithm () : base("Bubble Sort")
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
			for (int iterator = 0; iterator < items.Count; iterator++) {
				for (int index = 0; index < items.Count - 1; index++) {
					if (IsGreaterThan(items [index], items [index + 1])) {
						yield return Swap (items, index, index + 1);
					}
				}
			}			
		}
		#endregion`
	}
}