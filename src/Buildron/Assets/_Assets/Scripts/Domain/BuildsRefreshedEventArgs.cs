using System;
using System.Collections.Generic;

namespace Buildron.Domain
{
    /// <summary>
    /// Arguments for builds refreshed events.
    /// </summary>
    public class BuildsRefreshedEventArgs : EventArgs
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.BuildsRefreshedEventArgs"/> class.
		/// </summary>
		/// <param name="buildsFound">Builds found.</param>
		/// <param name="buildsRemoved">Builds removed.</param>
		public BuildsRefreshedEventArgs(IList<Build> buildsFound, IList<Build> buildsRemoved)
		{
            BuildsFound = buildsFound;
            BuildsRemoved = buildsRemoved;
		}
        #endregion

        #region Properties                
        /// <summary>
        /// Gets the builds found in builds refresh.
        /// </summary>
        public IList<Build> BuildsFound { get; private set; }

        /// <summary>
        /// Gets the builds removed in builds refresh.
        /// </summary>
        public IList<Build> BuildsRemoved { get; private set; }
        #endregion
    }
}