using System;

namespace Buildron.Domain
{
    /// <summary>
    /// Arguments for build removed events.
    /// </summary>
    public class BuildRemovedEventArgs : EventArgs
	{
		#region Constructors
		public BuildRemovedEventArgs (Build build)
		{
			Build = build;
		}
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the build.
        /// </summary>
        public Build Build { get; private set; }
		#endregion
	}
}