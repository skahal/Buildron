using System;

namespace Buildron.Domain
{
    /// <summary>
    /// Base class to arguments for user found events.
    /// </summary>
    public class UserFoundEventArgs: UserEventArgsBase
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.UserFoundArgs"/> class.
		/// </summary>
		/// <param name="user">The user.</param>
		public UserFoundEventArgs (User user)
			: base (user)
		{
		}
        #endregion
	}
}