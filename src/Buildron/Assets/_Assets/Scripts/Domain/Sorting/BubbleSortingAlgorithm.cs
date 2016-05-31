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
		public BubbleSortingAlgorithm () : base("Bubble Sort")
		{
			
		}
		
		protected override IEnumerator PerformSort (IList<TItem> items)
		{
			for (int iterator = 0; iterator < items.Count; iterator++) {
				for (int index = 0; index < items.Count - 1; index++) {
					if (IsGreaterThan(items [index], items [index + 1])) {
						yield return Swap (items, index, index + 1);
					}
				}
			}
			
			Messenger.Send ("OnSortingEnded");
		}
	}
}