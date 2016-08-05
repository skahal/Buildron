using System;

namespace Buildron.Domain.Sorting
{
	/// <summary>
	/// Sorting begin event arguments.
	/// </summary>
	public class SortingBeginEventArgs : EventArgs
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Sorting.SortingBeginEventArgs"/> class.
		/// </summary>
		/// <param name="wasAlreadySorted">A value indicating whether the items was already sorted.</param>
		public SortingBeginEventArgs(bool wasAlreadySorted)
		{
			WasAlreadySorted = wasAlreadySorted;
		}
		#endregion

		#region Properties        		
		/// <summary>
		/// Gets a value indicating whether the itemswas already sorted.
		/// </summary>
		/// <value>
		///   <c>true</c> if items was already sorted; otherwise, <c>false</c>.
		/// </value>
		public bool WasAlreadySorted { get; private set; }
		#endregion
	}
}