using System;

namespace Buildron.Domain.Users
{
    /// <summary>
    /// Arguments for user removed events.
    /// </summary>
    public class UserRemovedEventArgs : UserEventArgsBase
	{
        #region Constructors
        /// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Users.UserRemovedEventArgs"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public UserRemovedEventArgs(User user)
			: base (user)
		{
		}
        #endregion
	}
}