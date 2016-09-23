using System;

namespace Buildron.Domain.Users
{
    /// <summary>
    /// Base class to arguments for user events.
    /// </summary>
    public abstract class UserEventArgsBase: EventArgs
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Users.UserEventArgsBase"/> class.
		/// </summary>
		/// <param name="user">The user.</param>
		protected UserEventArgsBase(IUser user)
		{
			User = user;
		}
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the user.
        /// </summary>
		public IUser User { get; private set; }
		#endregion
	}
}