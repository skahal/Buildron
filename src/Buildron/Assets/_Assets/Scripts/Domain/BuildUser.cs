#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Buildron.Domain
{
	#region Enums
	/// <summary>
	/// Build user kind.
	/// </summary>
	public enum BuildUserKind {
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
	public class BuildUser : IEquatable<BuildUser>
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.BuildUser"/> class.
		/// </summary>
		public BuildUser ()
		{
			Builds = new List<Build> ();
			Kind = BuildUserKind.Human;
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
		public BuildUserKind Kind { get; set; }
		#endregion
		
		#region Methods
		public bool HasFailedBuild ()
		{
			return Builds.Count (b => b.Status == BuildStatus.Failed || b.Status == BuildStatus.Error) > 0;
		}
		
		public bool HasRunningBuild ()
		{
			return Builds.Count (b => b.Status >= BuildStatus.Running) > 0;
		}

        public bool Equals(BuildUser other)
        {
            return other != null && other.UserName.Equals(UserName, StringComparison.Ordinal);
        }
        #endregion
    }
}