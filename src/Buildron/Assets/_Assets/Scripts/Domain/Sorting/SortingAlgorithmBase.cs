#region Usings
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;
using UnityEngine;
#endregion

namespace Buildron.Domain.Sorting
{
	public abstract class SortingAlgorithmBase<TItem> : ISortingAlgorithm<TItem> where TItem : System.IComparable<TItem>
	{
		#region Constructors
		protected SortingAlgorithmBase (string name)
		{
			Name = name;
		}
		#endregion
		
		#region Properties
		public string Name { get; private set;}
		private IComparer<TItem> Comparer { get; set; }
		#endregion
		
		#region Methods
		public void Sort (IList<TItem> items, IComparer<TItem> equalityComparer)
		{
			Messenger.Send ("OnSortingBegin");
			Comparer = equalityComparer;
			SH.StartCoroutine (PerformSort (items));
		}
		
		protected abstract IEnumerator PerformSort (IList<TItem> items);
		
		protected bool IsGreaterThan (TItem it, TItem than)
		{
			return Comparer.Compare(it, than) > 0;
		}
		
		protected bool IsLowerThan (TItem it, TItem than)
		{
			return Comparer.Compare (it, than) < 0;
		}
		
		protected WaitForSeconds Swap (IList<TItem> items, int item1Index, int item2Index)
		{
			var item1 = items [item1Index];	
			var item2 = items [item2Index];
			items [item1Index] = item2;
			items [item2Index] = item1;
			
			Messenger.Send ("OnSortingItemsSwapped", new TItem[] { item1, item2 });
			return new WaitForSeconds(SortingAlgorithmFactory.SwappingTime);
		}
		#endregion
	}
}