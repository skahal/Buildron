using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Buildron.Domain.Builds;

namespace Buildron.Domain.Users
{
	#region Enums
	/// <summary>
	/// User kind.
	/// </summary>
	public enum UserKind {
		/// <summary>
		/// A human user.
		/// </summary>
		Human,
		
		/// <summary>
		/// A scheduled trigger user.
		/// </summary>
		ScheduledTrigger,
		
		/// <summary>
		/// A retry trigger user.
		/// </summary>
		RetryTrigger
	}
    #endregion

    /// <summary>
    /// Defines an interface for a user that trigger a build.
    /// </summary>
    public interface IUser : IEquatable<IUser>, IComparable<IUser>
    {
		#region Properties
		/// <summary>
		/// Gets the build.
		/// </summary>
		IList<IBuild> Builds { get; }
			
		/// <summary>
		/// Gets or sets the username
		/// </summary>
		string UserName { get; set; }
		
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		string Name { get; set; }
		
		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		string Email { get; set; }
		
		/// <summary>
		/// Gets or sets the kind.
		/// </summary>
		UserKind Kind { get; set; }
		#endregion
		
		#region Methods
		/// <summary>
		/// Determines whether this instance has failed build.
		/// </summary>
		/// <returns><c>true</c> if this instance has failed build; otherwise, <c>false</c>.</returns>
		bool HasFailedBuild ();

		/// <summary>
		/// Determines whether this instance has running build.
		/// </summary>
		/// <returns><c>true</c> if this instance has running build; otherwise, <c>false</c>.</returns>
		bool HasRunningBuild ();

		/// <summary>
		/// Determines whether the specified <see cref="Buildron.Domain.Users.User"/> is equal to the current <see cref="Buildron.Domain.Users.User"/>.
		/// </summary>
		/// <param name="other">The <see cref="Buildron.Domain.Users.User"/> to compare with the current <see cref="Buildron.Domain.Users.User"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="Buildron.Domain.Users.User"/> is equal to the current
		/// <see cref="Buildron.Domain.Users.User"/>; otherwise, <c>false</c>.</returns>
		bool Equals(IUser other);

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
		int CompareTo(IUser other);
	    #endregion
    }
}