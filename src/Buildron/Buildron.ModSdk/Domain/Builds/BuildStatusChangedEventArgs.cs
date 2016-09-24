using System;

namespace Buildron.Domain.Builds
{
    /// <summary>
    /// Arguments for build status changed events.
    /// </summary>
    public class BuildStatusChangedEventArgs : BuildEventArgsBase
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Builds.BuildStatusChangedEventArgs"/> class.
		/// </summary>
		/// <param name="build">Build.</param>
		/// <param name="previousStatus">Previous status.</param>
		public BuildStatusChangedEventArgs(IBuild build, BuildStatus previousStatus)
			: base(build)
		{
            PreviousStatus = previousStatus;
		}
        #endregion

        #region Properties        
   	    /// <summary>
        /// Gets the previous build status.
        /// </summary>
        public BuildStatus PreviousStatus { get; private set; }
		#endregion
	}
}