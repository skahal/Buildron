using System;

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
	}
}
