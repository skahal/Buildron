using System;

namespace Buildron.Domain
{
    /// <summary>
    /// Argumento for build status changed events.
    /// </summary>
    public class BuildStatusChangedEventArgs : EventArgs
	{
		#region Constructors
		public BuildStatusChangedEventArgs(Build build, BuildStatus previosStatus)
		{
			Build = build;
            PreviousStatus = previosStatus;
		}
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the build.
        /// </summary>
        public Build Build { get; private set; }

        /// <summary>
        /// Gets the previous build status.
        /// </summary>
        public BuildStatus PreviousStatus { get; private set; }
		#endregion
	}
}