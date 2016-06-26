using Buildron.Domain.Users;

namespace Buildron.Domain.Builds
{
    /// <summary>
    /// Arguments for build triggered by changed events.
    /// </summary>
	public class BuildTriggeredByChangedEventArgs : BuildEventArgsBase
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Builds.BuildTriggeredByChangedEventArgs"/> class.
		/// </summary>
		/// <param name="build">Build.</param>
		/// <param name="previousTriggeredBy">Previous triggered by.</param>
		public BuildTriggeredByChangedEventArgs(Build build, User previousTriggeredBy)
			: base (build)
		{
			PreviousTriggeredBy = previousTriggeredBy;
		}
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the previous triggered by.
        /// </summary>
		public User PreviousTriggeredBy { get; private set; }
		#endregion
	}
}