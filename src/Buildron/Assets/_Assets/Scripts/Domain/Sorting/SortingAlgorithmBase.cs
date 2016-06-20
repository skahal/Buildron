#region Usings
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;
using UnityEngine;
using Skahal.Common;


#endregion

namespace Buildron.Domain.Sorting
{
	/// <summary>
	/// Sorting algorithm base class.
	/// </summary>
	public abstract class SortingAlgorithmBase<TItem> 
		: ISortingAlgorithm<TItem> where TItem : System.IComparable<TItem>
	{
		#region Events
		/// <summary>
		/// Occurs when sorting begin.
		/// </summary>
		public event EventHandler SortingBegin;

		/// <summary>
		/// Occurs when sorting items swapped.
		/// </summary>
		public event EventHandler<SortingItemsSwappedEventArgs<TItem>> SortingItemsSwapped;

		/// <summary>
		/// Occurs when sorting end.
		/// </summary>
		public event EventHandler SortingEnded;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Sorting.SortingAlgorithmBase`1"/> class.
		/// </summary>
		/// <param name="name">The algorithm name.</param>
		protected SortingAlgorithmBase (string name)
		{
			Name = name;
		}
		#endregion
		
		#region Properties
		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; private set;}

		/// <summary>
		/// Gets or sets the comparer.
		/// </summary>
		/// <value>The comparer.</value>
		private IComparer<TItem> Comparer { get; set; }
		#endregion
		
		#region Methods
		/// <summary>
		/// Sorts the specified items.
		/// </summary>
		/// <param name="items">The items.</param>
		/// <param name="equalityComparer">The equality comparer.</param>
		public IEnumerator Sort (IList<TItem> items, IComparer<TItem> equalityComparer)
		{
			//Messenger.Send ("OnSortingBegin");
			SortingBegin.Raise(this);
			Comparer = equalityComparer;
			//SH.StartCoroutine (CallPerformSort(items));

			//return CallPerformSort (items);
			return PerformSort(items);
		}

		/// <summary>
		/// Performs the sort.
		/// </summary>
		/// <returns>The sort.</returns>
		/// <param name="items">Items.</param>
		protected abstract IEnumerator PerformSort (IList<TItem> items);

		/// <summary>
		/// Determines whether item is greater than the other item.
		/// </summary>
		/// <returns><c>true</c> if this instance is greater than the specified item other; otherwise, <c>false</c>.</returns>
		/// <param name="item">Item.</param>
		/// <param name="other">Other.</param>
		protected bool IsGreaterThan (TItem item, TItem other)
		{
			return Comparer.Compare(item, other) > 0;
		}

		/// <summary>
		/// Determines whether item is lower than the other item.
		/// </summary>
		/// <returns><c>true</c> if this instance is greater than the specified item other; otherwise, <c>false</c>.</returns>
		/// <param name="item">Item.</param>
		/// <param name="other">Other.</param>
		protected bool IsLowerThan (TItem item, TItem other)
		{
			return Comparer.Compare (item, other) < 0;
		}

		/// <summary>
		/// Swap the specified items.
		/// </summary>
		/// <param name="items">Items.</param>
		/// <param name="item1Index">Item1 index.</param>
		/// <param name="item2Index">Item2 index.</param>
		protected WaitForSeconds Swap (IList<TItem> items, int item1Index, int item2Index)
		{
			var item1 = items [item1Index];	
			var item2 = items [item2Index];
			items [item1Index] = item2;
			items [item2Index] = item1;
			
			//Messenger.Send ("OnSortingItemsSwapped", new TItem[] { item1, item2 });
			SortingItemsSwapped.Raise(this, new SortingItemsSwappedEventArgs<TItem>(item1, item2));
			return new WaitForSeconds(SortingAlgorithmFactory.SwappingTime);
		}

		/// <summary>
		/// Raises the sorting ended event.
		/// </summary>
		/// <param name="args">The event arguments.</param>
		protected virtual void OnSortingEnded(EventArgs args)
		{
			if (SortingEnded != null) {
				SortingEnded (this, args);
			}
		}

//        private IEnumerator CallPerformSort(IList<TItem> items)
//        {
//            var enumerator = PerformSort(items);
//            yield return enumerator;
//
//			SortingEnded.Raise (this);
//            //Messenger.Send("OnSortingEnded");
//        }
		#endregion
	}
}