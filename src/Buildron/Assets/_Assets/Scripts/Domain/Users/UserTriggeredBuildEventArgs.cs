using System;
using Buildron.Domain.Builds;

namespace Buildron.Domain.Users
{
    /// <summary>
    /// Arguments for user triggered build events.
    /// </summary>
    public class UserTriggeredBuildEventArgs : UserEventArgsBase
	{
        #region Constructors
        /// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Users.UserTriggeredBuildEventArgs"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="build">The build that user triggered</param>
        public UserTriggeredBuildEventArgs(User user, Build build)
			: base (user)
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