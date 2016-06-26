#region Usings
using System;
using System.Collections.Generic;
using Buildron.Domain;
using Buildron.Domain.Builds;


#endregion

namespace Buildron.Domain
{
	/// <summary>
	/// Authentication requirement.
	/// </summary>
	public enum AuthenticationRequirement {
		Always,
		Optional,
		Never
	}

	/// <summary>
	/// Defines an interface for a builds provider.
	/// <remarks>>
	/// A builds provider can be a CI server, like TeamCity, Jenkins and Hudson.
	/// </remarks>
	/// </summary>
	public interface IBuildsProvider
	{
		#region Events
		/// <summary>
		/// Occurs when an build is updated.
		/// </summary>
		event EventHandler<BuildUpdatedEventArgs> BuildUpdated;

		/// <summary>
		/// Occurs when all builds are refreshed.
		/// <remarks>
		/// In the end of RefreshBuilds method executionl.
		/// <remarks>
		/// </summary>
		event EventHandler BuildsRefreshed;

		/// <summary>
		/// Occurs when server up that buils provider communicate is up.
		/// </summary>
		event EventHandler ServerUp;

		/// <summary>
		/// Occurs when server up that buils provider communicate is down.
		/// </summary>
		event EventHandler ServerDown;

		/// <summary>
		/// Occurs when user authentication is successful.
		/// </summary>
		event EventHandler UserAuthenticationSuccessful;

		/// <summary>
		/// Occurs when user authentication failed.
		/// </summary>
		event EventHandler UserAuthenticationFailed;
		#endregion
		
		#region Properties
		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		string Name { get; }

		/// <summary>
		/// Gets the authentication requirement.
		/// </summary>
		/// <value>The authentication requirement.</value>
		AuthenticationRequirement AuthenticationRequirement { get; }

		/// <summary>
		/// Gets the authentication tip.
		/// </summary>
		/// <value>The authentication tip.</value>
		string AuthenticationTip { get; }
		#endregion
	
		#region Methods
		/// <summary>
		/// Refreshs all builds.
		/// </summary>
		void RefreshAllBuilds();

		/// <summary>
		/// Runs the build.
		/// </summary>
		/// <param name="user">The user that triggered the run.</param>
		/// <param name="build">The build to run</param>
		void RunBuild(UserBase user, Build build);

		/// <summary>
		/// Stops the build.
		/// </summary>
		/// <param name="user">The user that triggered the stop.</param>
		/// <param name="build">The build to stop</param>
		void StopBuild(UserBase user, Build build);

		/// <summary>
		/// Authenticates the user.
		/// </summary>
		/// <param name="user">The user to authenticate.</param>
		void AuthenticateUser(UserBase user);
		#endregion
	}
}

