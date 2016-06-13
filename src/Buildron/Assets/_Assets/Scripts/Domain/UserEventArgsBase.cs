using System;

namespace Buildron.Domain
{
    /// <summary>
    /// Base class to arguments for user events.
    /// </summary>
    public abstract class UserEventArgsBase: EventArgs
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.UserEventArgsBase"/> class.
		/// </summary>
		/// <param name="user">The user.</param>
		protected UserEventArgsBase(User user)
		{
			User = user;
		}
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the user.
        /// </summary>
		public User User { get; private set; }
		#endregion
	}
}