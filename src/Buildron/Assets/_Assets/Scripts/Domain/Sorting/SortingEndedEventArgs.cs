using System;

namespace Buildron.Domain.Sorting
{
	/// <summary>
	/// Sorting ended event arguments.
	/// </summary>
	public class SortingEndedEventArgs : SortingBeginEventArgs
	{
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Buildron.Domain.Sorting.SortingEndedEventArgs"/> class.
        /// </summary>
        /// <param name="wasAlreadySorted">A value indicating whether the items was already sorted.</param>
        public SortingEndedEventArgs(bool wasAlreadySorted)
            : base(wasAlreadySorted)
		{
		}
        #endregion
	}
}