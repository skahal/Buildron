using System;
using System.Collections;
using System.Collections.Generic;

namespace Buildron.Domain.Sorting
{
	/// <summary>
	/// Defines an interface for a sorting algorithm.
	/// </summary>
	public interface ISortingAlgorithm<TItem> where TItem : System.IComparable<TItem>
	{
		#region Events
		/// <summary>
		/// Occurs when sorting begin.
		/// </summary>
		event EventHandler<SortingBeginEventArgs> SortingBegin;

		/// <summary>
		/// Occurs when sorting items swapped.
		/// </summary>
		event EventHandler<SortingItemsSwappedEventArgs<TItem>> SortingItemsSwapped;

		/// <summary>
		/// Occurs when sorting end.
		/// </summary>
		event EventHandler<SortingEndedEventArgs> SortingEnded;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the name.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets the comparer.
		/// </summary>
		IComparer<TItem> Comparer { get; }

		/// <summary>
		/// Sorts the specified items.
		/// </summary>
		/// <param name="items">The items.</param>
		/// <param name="comparer">The equality comparer.</param>
		IEnumerator Sort(IList<TItem> items, IComparer<TItem> comparer);
		#endregion
	}
}