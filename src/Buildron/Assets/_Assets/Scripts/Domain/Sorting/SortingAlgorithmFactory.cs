using System;
using System.Collections.Generic;
using Skahal.Common;

namespace Buildron.Domain.Sorting
{
    /// <summary>
    /// Sorting algorithm factory.
    /// </summary>
    public static class SortingAlgorithmFactory
    {
        #region Constants
		/// <summary>
		/// The swapping time.
		/// </summary>
        public const float SwappingTime = 0.4f;
        #endregion

        #region Methods
		/// <summary>
		/// Creates the sorting algorithm.
		/// </summary>
		/// <returns>The sorting algorithm.</returns>
		/// <param name="algorithmType">Algorithm type.</param>
		/// <typeparam name="TItem">The 1st type parameter.</typeparam>
        public static ISortingAlgorithm<TItem> CreateSortingAlgorithm<TItem>(SortingAlgorithmType algorithmType) where TItem : System.IComparable<TItem>
        {            
            switch (algorithmType)
            {

                case SortingAlgorithmType.None:
                    return new NoneSortingAlgorithm<TItem>();

                case SortingAlgorithmType.Insertion:
                    return new InsertionSortingAlgorithm<TItem>();

                case SortingAlgorithmType.Selection:
                    return new SelectionSortingAlgorithm<TItem>();

                case SortingAlgorithmType.Shell:
                    return new ShellSortingAlgorithm<TItem>();

                case SortingAlgorithmType.Bubble:
                    return new BubbleSortingAlgorithm<TItem>();

                default:
                    throw new InvalidOperationException("There is no sorting algorithm mapped to value " + algorithmType);
            }
        }

		/// <summary>
		/// Creates the random sorting algorithm.
		/// </summary>
		/// <returns>The random sorting algorithm.</returns>
		/// <typeparam name="TItem">The 1st type parameter.</typeparam>
        public static ISortingAlgorithm<TItem> CreateRandomSortingAlgorithm<TItem>() where TItem : System.IComparable<TItem>
        {
            var algorithmType = SHRandomHelper.NextEnum<SortingAlgorithmType>();

            return CreateSortingAlgorithm<TItem>(algorithmType);
        }

		/// <summary>
		/// Gets the type of the algorithm.
		/// </summary>
		/// <returns>The algorithm type.</returns>
		/// <param name="algorithm">Algorithm.</param>
		/// <typeparam name="TItem">The 1st type parameter.</typeparam>
		public static SortingAlgorithmType GetAlgorithmType<TItem>(ISortingAlgorithm<TItem> algorithm)
			where TItem : System.IComparable<TItem>
		{
			if (algorithm is BubbleSortingAlgorithm<TItem>) {
				return SortingAlgorithmType.Bubble;
			}

			if (algorithm is InsertionSortingAlgorithm<TItem>) {
				return SortingAlgorithmType.Insertion;
			}

			if (algorithm is SelectionSortingAlgorithm<TItem>) {
				return SortingAlgorithmType.Selection;
			}

			if (algorithm is ShellSortingAlgorithm<TItem>) {
				return SortingAlgorithmType.Shell;
			}

			return SortingAlgorithmType.None;
		}        
        #endregion
    }
}