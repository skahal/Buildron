namespace Buildron.Domain.Users
{
    /// <summary>
    /// Arguments for user found events.
    /// </summary>
    public class UserFoundEventArgs: UserEventArgsBase
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Users.UserFoundArgs"/> class.
		/// </summary>
		/// <param name="user">The user.</param>
		public UserFoundEventArgs (IUser user)
			: base (user)
		{
		}
        #endregion
	}
}