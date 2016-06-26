namespace Buildron.Domain.Users
{
	/// <summary>
	/// User authentication completed event arguments.
	/// </summary>
	public class UserAuthenticationCompletedEventArgs : UserEventArgsBase
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Users.UserAuthenticationCompletedEventArgs"/> class.
		/// </summary>
		/// <param name="user">User.</param>
		/// <param name="success">If set to <c>true</c> success.</param>
		public UserAuthenticationCompletedEventArgs(User user, bool success) 
			: base (user)
		{
            Success = success;
		}
        #endregion

        #region Properties
		/// <summary>
		/// Gets a value indicating whether this <see cref="Buildron.Domain.Users.UserAuthenticationCompletedEventArgs"/> is success.
		/// </summary>
		/// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success { get; private set; }
        #endregion
    }
}