using System;
using System.Collections.Generic;
using Buildron.Domain.Sorting;
using Buildron.Domain.Builds;
using Buildron.Domain.Users;

namespace Buildron.Domain.Builds
{
	/// <summary>
	/// Defines an interface to a build service.
	/// </summary>
    public interface IBuildService
    {
		/// <summary>
		/// Occurs when a build is found.
		/// </summary>
        event EventHandler<BuildFoundEventArgs> BuildFound;

		/// <summary>
		/// Occurs when a build is removed.
		/// </summary>
        event EventHandler<BuildRemovedEventArgs> BuildRemoved;

		/// <summary>
		/// Occurs when builds are refreshed.
		/// </summary>
        event EventHandler<BuildsRefreshedEventArgs> BuildsRefreshed;

		/// <summary>
		/// Occurs when a build is updated.
		/// </summary>
        event EventHandler<BuildUpdatedEventArgs> BuildUpdated;

		/// <summary>
		/// Gets the builds count.
		/// </summary>
		/// <value>The builds count.</value>
		int BuildsCount { get; }

		/// <summary>
		/// Gets the name of the server.
		/// </summary>
		/// <value>The name of the server.</value>
		string ServerName { get; }

		/// <summary>
		/// Gets the most relevant build for user.
		/// </summary>
		/// <returns>The most relevant build for user.</returns>
		/// <param name="user">User.</param>
        Build GetMostRelevantBuildForUser(User user);

		/// <summary>
		/// Initialize.
		/// </summary>
		/// <param name="buildsProvider">Builds provider.</param>
        void Initialize(IBuildsProvider buildsProvider);

		/// <summary>
		/// Refreshs all builds.
		/// </summary>
        void RefreshAllBuilds();  

		/// <summary>
		/// Runs the build.
		/// </summary>
		/// <param name="remoteControl">Remote control.</param>
		/// <param name="buildId">Build identifier.</param>
        void RunBuild(RemoteControl remoteControl, string buildId);

		/// <summary>
		/// Stops the build.
		/// </summary>
		/// <param name="remoteControl">Remote control.</param>
		/// <param name="buildId">Build identifier.</param>
        void StopBuild(RemoteControl remoteControl, string buildId);
    }
}