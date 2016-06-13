#region Usings
using System;
using System.Collections.Generic;
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
	public class User : IEquatable<User>
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
        #endregion
    }
}