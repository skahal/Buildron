using System;

namespace Buildron.Domain.Sorting
{
	/// <summary>
	/// Sorting items swapped event arguments.
	/// </summary>
	public class SortingItemsSwappedEventArgs<TItem> : EventArgs
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Sorting.SortingItemsSwappedEventArgs{TItem}"/> class.
		/// </summary>
		/// <param name="item1">Item 1.</param>
		/// <param name="item2">Item 2.</param>
		public SortingItemsSwappedEventArgs(TItem item1, TItem item2)
		{
			Item1 = item1;
			Item2 = item2;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the item 1.
		/// </summary>
		/// <value>The item 1.</value>
		public TItem Item1 { get; private set; }

		/// <summary>
		/// Gets the item 2.
		/// </summary>
		/// <value>The item 2.</value>
		public TItem Item2 { get; private set; }
		#endregion
	}
}