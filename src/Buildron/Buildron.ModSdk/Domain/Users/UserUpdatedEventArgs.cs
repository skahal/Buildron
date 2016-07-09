namespace Buildron.Domain.Users
{
    /// <summary>
    /// Arguments for user updated events.
    /// </summary>
    public class UserUpdatedEventArgs : UserEventArgsBase
	{
        #region Constructors
        /// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Users.UserUpdatedEventArgs"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public UserUpdatedEventArgs(IUser user)
			: base (user)
		{
		}
        #endregion
	}
}