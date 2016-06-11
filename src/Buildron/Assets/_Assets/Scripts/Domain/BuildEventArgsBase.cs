using System;

namespace Buildron.Domain
{
    /// <summary>
    /// Base class to arguments for build  changed events.
    /// </summary>
    public abstract class BuildEventArgsBase: EventArgs
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.BuildEventArgsBase"/> class.
		/// </summary>
		/// <param name="build">The build.</param>
		protected BuildEventArgsBase(Build build)
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