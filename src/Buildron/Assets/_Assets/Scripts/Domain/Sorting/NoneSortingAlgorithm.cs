#region Usings
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;
#endregion

namespace Buildron.Domain.Sorting
{
    /// <summary>
    /// Used to perform no sorting.
    /// </summary>
    public class NoneSortingAlgorithm<TItem> : SortingAlgorithmBase<TItem>
        where TItem : System.IComparable<TItem>
    {
        public NoneSortingAlgorithm() 
            : base("None")
        {

        }

        protected override IEnumerator PerformSort(IList<TItem> items)
        {
            return items.GetEnumerator();
        }
    }
}