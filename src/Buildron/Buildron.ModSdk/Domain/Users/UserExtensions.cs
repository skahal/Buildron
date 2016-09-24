using System;
using System.Linq;
using Buildron.Domain.Builds;

namespace Buildron.Domain.Users
{
	/// <summary>
	/// User extension methods.
	/// </summary>
	public static class UserExtensions
	{
		/// <summary>
		/// Determines whether this instance has failed build.
		/// </summary>
		/// <returns><c>true</c> if this instance has failed build; otherwise, <c>false</c>.</returns>
		public static bool HasFailedBuild (this IUser user)
		{
			return user.Builds.Count (b => b.Status == BuildStatus.Failed || b.Status == BuildStatus.Error) > 0;
		}

		/// <summary>
		/// Determines whether this instance has running build.
		/// </summary>
		/// <returns><c>true</c> if this instance has running build; otherwise, <c>false</c>.</returns>
		public static bool HasRunningBuild (this IUser user)
		{
			return user.Builds.Count (b => b.Status >= BuildStatus.Running) > 0;
		}
	}
}
