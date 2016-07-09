using System;
using System.Collections.Generic;

namespace Buildron.Domain.Builds
{
    /// <summary>
    /// Arguments for builds refreshed events.
    /// </summary>
    public class BuildsRefreshedEventArgs : EventArgs
	{
        #region Constructors
        /// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Builds.BuildsRefreshedEventArgs"/> class.
        /// </summary>
        /// <param name="buildsStatusChanged">Builds that status changed.</param>
        /// <param name="buildsFound">Builds found.</param>
        /// <param name="buildsRemoved">Builds removed.</param>
        public BuildsRefreshedEventArgs(IList<IBuild> buildsStatusChanged, IList<IBuild> buildsFound, IList<IBuild> buildsRemoved)
		{
            BuildsStatusChanged = buildsStatusChanged;
            BuildsFound = buildsFound;
            BuildsRemoved = buildsRemoved;
		}
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the builds that status changed in builds refresh.
        /// </summary>
        public IList<IBuild> BuildsStatusChanged { get; private set; }

        /// <summary>
        /// Gets the builds found in builds refresh.
        /// </summary>
        public IList<IBuild> BuildsFound { get; private set; }

        /// <summary>
        /// Gets the builds removed in builds refresh.
        /// </summary>
        public IList<IBuild> BuildsRemoved { get; private set; }
        #endregion
    }
}