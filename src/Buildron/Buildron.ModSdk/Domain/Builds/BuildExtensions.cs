using System;
using System.Linq;
using Buildron.Domain.Users;
using System.Collections.Generic;

namespace Buildron.Domain.Builds
{
	/// <summary>
	/// Build extension methods.
	/// </summary>
	public static class BuildExtensions
	{
		/// <summary>
		/// Gets a value indicating whether the build is finish with success.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is success; otherwise, <c>false</c>.
		/// </value>
		public static bool IsSuccess(this IBuild build)
		{
			return build.Status == BuildStatus.Success;
		}

		/// <summary>
		/// Gets a value indicating whether the build is running.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is running; otherwise, <c>false</c>.
		/// </value>
		public static bool IsRunning(this IBuild build)
		{
			return build.Status >= BuildStatus.Running;
		}

		/// <summary>
		/// Gets a value indicating whether the build has been queued.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is queued; otherwise, <c>false</c>.
		/// </value>
		public static bool IsQueued(this IBuild build)
		{
			return build.Status == BuildStatus.Queued;
		}

		/// <summary>
		/// Gets a value indicating whether the build has failed (Canceled | Error | Failed)
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is failed; otherwise, <c>false</c>.
		/// </value>
		public static bool IsFailed(this IBuild build)
		{
			return build.Status >= BuildStatus.Error && build.Status <= BuildStatus.Canceled;
		}

		/// <summary>
		/// Gets the most relevant build for user.
		/// </summary>
		/// <param name="builds">The builds</param>
		/// <param name="user">User.</param>
		/// <returns>The most relevant build for user.</returns>
		public static IBuild GetMostRelevantBuildForUser (this IEnumerable<IBuild> builds, IUser user)
		{
			var comparer = new BuildMostRelevantStatusComparer();
			var userBuilds = builds
				.Where(b => b.TriggeredBy != null && b.TriggeredBy == user)
				.OrderBy(b => b, comparer);

			return userBuilds.FirstOrDefault();
		}	
	}
}
