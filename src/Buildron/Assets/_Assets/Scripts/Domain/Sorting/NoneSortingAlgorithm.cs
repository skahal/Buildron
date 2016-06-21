using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;

namespace Buildron.Domain.Sorting
{
    /// <summary>
    /// Used to perform no sorting.
    /// </summary>
    public class NoneSortingAlgorithm<TItem> : SortingAlgorithmBase<TItem>
        where TItem : System.IComparable<TItem>
    {
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Sorting.NoneSortingAlgorithm`1"/> class.
		/// </summary>
        public NoneSortingAlgorithm() 
            : base("None")
        {

        }
		#endregion

		#region Methods
		/// <summary>
		/// Performs the sort.
		/// </summary>
		/// <returns>The sort.</returns>
		/// <param name="items">Items.</param>
        protected override IEnumerator PerformSort(IList<TItem> items)
        {
      		OnSortingEnded (EventArgs.Empty);
			yield break;
        }
		#endregion
    }
}