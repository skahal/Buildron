using System;

namespace Buildron.Domain
{
    /// <summary>
    /// Arguments for build status changed events.
    /// </summary>
    public class BuildStatusChangedEventArgs : BuildEventArgsBase
	{
		#region Constructors
		public BuildStatusChangedEventArgs(Build build, BuildStatus previousStatus)
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