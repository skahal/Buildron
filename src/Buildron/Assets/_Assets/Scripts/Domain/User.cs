#region Usings
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
#endregion

namespace Buildron.Domain
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
    /// Defines a user that trigger a build.
    /// </summary>
    [DebuggerDisplay("{UserName}")]
    public class User : IEquatable<User>, IComparable<User>
    {
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.User"/> class.
		/// </summary>
		public User ()
		{
			Builds = new List<Build> ();
			Kind = UserKind.Human;
		}
		#endregion
		
		#region Properties
		/// <summary>
		/// Gets or sets the build.
		/// </summary>
		public IList<Build> Builds { get; private set; }
			
		/// <summary>
		/// Gets or sets the username
		/// </summary>
		public string UserName { get; set; }
		
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		public string Email { get; set; }
		
		/// <summary>
		/// Gets or sets the kind.
		/// </summary>
		public UserKind Kind { get; set; }
		#endregion
		
		#region Methods
		/// <summary>
		/// Determines whether this instance has failed build.
		/// </summary>
		/// <returns><c>true</c> if this instance has failed build; otherwise, <c>false</c>.</returns>
		public bool HasFailedBuild ()
		{
			return Builds.Count (b => b.Status == BuildStatus.Failed || b.Status == BuildStatus.Error) > 0;
		}

		/// <summary>
		/// Determines whether this instance has running build.
		/// </summary>
		/// <returns><c>true</c> if this instance has running build; otherwise, <c>false</c>.</returns>
		public bool HasRunningBuild ()
		{
			return Builds.Count (b => b.Status >= BuildStatus.Running) > 0;
		}

		/// <summary>
		/// Determines whether the specified <see cref="Buildron.Domain.User"/> is equal to the current <see cref="Buildron.Domain.User"/>.
		/// </summary>
		/// <param name="other">The <see cref="Buildron.Domain.User"/> to compare with the current <see cref="Buildron.Domain.User"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="Buildron.Domain.User"/> is equal to the current
		/// <see cref="Buildron.Domain.User"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(User other)
        {
            return other != null && other.UserName.Equals(UserName, StringComparison.Ordinal);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {            
            return Equals(obj as User);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return UserName.GetHashCode();
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public int CompareTo(User other)
        {
            if (other == null)
            {
                return 1;
            }

            return "{0}_{1}_{2}".With(UserName, Name, Email).CompareTo("{0}_{1}_{2}".With(other.UserName, other.Name, other.Email));
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="user1">The user1.</param>
        /// <param name="user2">The user2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator == (User user1, User user2)
        {
            // Check for both null (need this casts to object or will run in a recursive loop).
            if ((object)user1 == null && (object)user2 == null)
            {
                return true;
            }

            if ((object)user1 == null || (object)user2 == null)
            {
                return false;
            }

            return user1.Equals(user2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="user1">The user1.</param>
        /// <param name="user2">The user2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(User user1, User user2)
        {
            return !(user1 == user2);
        }
        #endregion
    }
}