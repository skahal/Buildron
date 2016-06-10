#region Usings
using System.Collections.Generic;
#endregion

namespace Buildron.Domain.Sorting
{
	/// <summary>
	///  sorting algorithm.
	/// </summary>
	public interface ISortingAlgorithm<TItem> where TItem : System.IComparable<TItem>
	{
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Sorts the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        void Sort(IList<TItem> items, IComparer<TItem> equalityComparer);
	}
}