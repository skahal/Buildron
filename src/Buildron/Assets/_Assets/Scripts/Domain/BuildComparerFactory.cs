using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buildron.Domain.Sorting;

namespace Buildron.Domain
{
    /// <summary>
    /// Build comparer factory.
    /// </summary>
    public static class BuildComparerFactory
    {
        /// <summary>
		/// Creates the comparer.
		/// </summary>
		/// <returns>The comparer.</returns>
		/// <param name="sortBy">Sort by.</param>
		public static IComparer<Build> Create(SortBy sortBy)
        {
            switch (sortBy)
            {
                case SortBy.Date:
                    return new BuildDateDescendingComparer();

                case SortBy.RelevantStatus:
                    return new BuildMostRelevantStatusComparer();

                default:
                    return new BuildTextComparer();
            }
        }
    }
}
