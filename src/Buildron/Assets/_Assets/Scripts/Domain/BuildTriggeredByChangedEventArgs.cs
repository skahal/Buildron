using System;

namespace Buildron.Domain
{
    /// <summary>
    /// Arguments for build triggered by changed events.
    /// </summary>
	public class BuildTriggeredByChangedEventArgs : BuildEventArgsBase
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.BuildTriggeredByChangedEventArgs"/> class.
		/// </summary>
		/// <param name="build">Build.</param>
		/// <param name="previousTriggeredBy">Previous triggered by.</param>
		public BuildTriggeredByChangedEventArgs(Build build, BuildUser previousTriggeredBy)
			: base (build)
		{
			PreviousTriggeredBy = previousTriggeredBy;
		}
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the previous triggered by.
        /// </summary>
		public BuildUser PreviousTriggeredBy { get; private set; }
		#endregion
	}
}