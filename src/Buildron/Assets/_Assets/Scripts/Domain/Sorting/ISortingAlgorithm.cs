#region Usings
using System.Collections.Generic;
using System.Collections;
using System;


#endregion

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
		event EventHandler SortingBegin;

		/// <summary>
		/// Occurs when sorting items swapped.
		/// </summary>
		event EventHandler<SortingItemsSwappedEventArgs<TItem>> SortingItemsSwapped;

		/// <summary>
		/// Occurs when sorting end.
		/// </summary>
		event EventHandler SortingEnded;
		#endregion

		#region Properties
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Sorts the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        IEnumerator Sort(IList<TItem> items, IComparer<TItem> equalityComparer);
		#endregion
	}
}