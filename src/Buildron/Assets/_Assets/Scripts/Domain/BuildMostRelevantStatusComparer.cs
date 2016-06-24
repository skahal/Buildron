using System.Collections.Generic;
using Buildron.Domain;

namespace Buildron.Domain
{
    /// <summary>
    /// Build most relevant status comparer.
    /// </summary>
    public class BuildMostRelevantStatusComparer : IComparer<Build>
	{     
        #region IComparer[Build] implementation
        /// <summary>
        /// Compare the specified x and y.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public int Compare (Build x, Build y)
		{
            return x.Status.CompareTo(y.Status) * -1;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Buildron.Domain.BuildDateDescendingComparer"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Buildron.Domain.BuildDateDescendingComparer"/>.</returns>
		public override string ToString()
		{
			return "status";
		}
		#endregion		
	}
}