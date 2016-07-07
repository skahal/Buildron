using System.Collections.Generic;
using Buildron.Domain;

namespace Buildron.Domain.Builds
{
    /// <summary>
    /// Build most relevant status comparer.
    /// </summary>
    public class BuildMostRelevantStatusComparer : IComparer<IBuild>
	{     
        #region Methods
        /// <summary>
        /// Compare the specified x and y.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public int Compare (IBuild x, IBuild y)
		{
            return x.Status.CompareTo(y.Status) * -1;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Buildron.Domain.Builds.BuildDateDescendingComparer"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Buildron.Domain.Builds.BuildDateDescendingComparer"/>.</returns>
		public override string ToString()
		{
			return "status";
		}
		#endregion		
	}
}