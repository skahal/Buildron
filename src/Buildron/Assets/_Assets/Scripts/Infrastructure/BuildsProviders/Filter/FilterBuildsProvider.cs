using System;
using System.Collections.Generic;
using System.Linq;
using Skahal.Logging;
using Buildron.Domain.Builds;
using Buildron.Domain.Users;
using Buildron.Domain.RemoteControls;
using Buildron.Domain.Servers;

namespace Buildron.Infrastructure.BuildsProviders.Filter
{
	/// <summary>
	/// An IBuildsProvider implementation to apply filter (from RC) to a underlying IBuildsProvider.
	/// </summary>
	public class FilterBuildsProvider : IBuildsProvider, IBuildFilter
	{
		#region Fields
		private IBuildsProvider m_underlyingBuildsProvider;
		private IRemoteControlMessagesListener m_rcListener;
		private IServerService m_serverService;
		private Dictionary<string, IBuild> m_buildsCache = new Dictionary<string, IBuild> ();
		#endregion

		#region Events
		/// <summary>
		/// Occurs when builds refreshed.
		/// </summary>
		public event EventHandler BuildsRefreshed;

		/// <summary>
		/// Occurs when an build is updated.
		/// </summary>
		public event EventHandler<BuildUpdatedEventArgs> BuildUpdated;

		/// <summary>
		/// Occurs when server up that buils provider communicate is down.
		/// </summary>
		public event EventHandler ServerDown;

		/// <summary>
		/// Occurs when server up that buils provider communicate is up.
		/// </summary>
		public event EventHandler ServerUp;

		/// <summary>
		/// Occurs when user authentication failed.
		/// </summary>
		public event EventHandler UserAuthenticationFailed;

		/// <summary>
		/// Occurs when user authentication is successful.
		/// </summary>
		public event EventHandler UserAuthenticationSuccessful;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="Buildron.Infrastructure.BuildsProviders.Filter.FilterBuildsProvider"/> class.
		/// </summary>
		/// <param name="underlyingBuildsProvider">Underlying builds provider.</param>
		/// <param name="rcListener">Rc listener.</param>
		/// <param name="serverService">Server service.</param>
		public FilterBuildsProvider (
			IBuildsProvider underlyingBuildsProvider, 
			IRemoteControlMessagesListener rcListener,
			IServerService serverService)
		{
			if (underlyingBuildsProvider == null)
			{
				throw new ArgumentNullException ("underlyingBuildsProvider");
			}

			m_underlyingBuildsProvider = underlyingBuildsProvider;
			m_rcListener = rcListener;
			m_serverService = serverService;

			m_underlyingBuildsProvider.BuildsRefreshed += (sender, e) =>
			{
				OnBuildsRefreshed (e);
			};

			m_underlyingBuildsProvider.BuildUpdated += (sender, e) =>
			{
				var build = e.Build;
				m_buildsCache [build.Id] = build;

				if (Filter (build))
				{
					OnBuildUpdated (e);
				}
			};

			m_underlyingBuildsProvider.ServerDown += (sender, e) =>
			{
				if (ServerDown != null)
				{
					ServerDown (sender, e);
				}
			};

			m_underlyingBuildsProvider.ServerUp += (sender, e) =>
			{
				if (ServerUp != null)
				{
					ServerUp (sender, e);
				}
			};

			m_underlyingBuildsProvider.UserAuthenticationFailed += (sender, e) =>
			{
				if (UserAuthenticationFailed != null)
				{
					UserAuthenticationFailed (sender, e);
				}
			};

			m_underlyingBuildsProvider.UserAuthenticationSuccessful += (sender, e) =>
			{
				if (UserAuthenticationSuccessful != null)
				{
					UserAuthenticationSuccessful (sender, e);
				}
			};

			m_rcListener.BuildFilterUpdated += (sender2, e2) =>
			{
				var filteredBuilds = m_buildsCache.Values.Where (Filter).ToArray();

				SHLog.Debug (
					"Filter updated. There is {0} cached builds and {1} was filtered", 
					m_buildsCache.Count, 
					filteredBuilds.Length);

				foreach (var build in filteredBuilds)
				{
					OnBuildUpdated (new BuildUpdatedEventArgs (build));
				}

				OnBuildsRefreshed (EventArgs.Empty);
			};
		
			Build.EventInterceptors.Add (new FilterBuildEventInterceptor (this));
		}

		#endregion

		#region Properties
		/// <summary>
		/// Gets the authentication requirement.
		/// </summary>
		/// <value>The authentication requirement.</value>
		public AuthenticationRequirement AuthenticationRequirement
		{
			get
			{
				return m_underlyingBuildsProvider.AuthenticationRequirement;
			}
		}

		/// <summary>
		/// Gets the authentication tip.
		/// </summary>
		/// <value>The authentication tip.</value>
		public string AuthenticationTip
		{
			get
			{
				return m_underlyingBuildsProvider.AuthenticationTip;
			}
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get
			{
				return m_underlyingBuildsProvider.Name;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Authenticates the user.
		/// </summary>
		/// <param name="user">The user to authenticate.</param>
		public void AuthenticateUser (UserBase user)
		{
			m_underlyingBuildsProvider.AuthenticateUser (user);
		}

		/// <summary>
		/// Refreshs all builds.
		/// </summary>
		public void RefreshAllBuilds ()
		{
			m_underlyingBuildsProvider.RefreshAllBuilds ();
		}

		/// <summary>
		/// Runs the build.
		/// </summary>
		/// <param name="user">The user that triggered the run.</param>
		/// <param name="build">The build to run</param>
		public void RunBuild (UserBase user, IBuild build)
		{
			m_underlyingBuildsProvider.RunBuild (user, build);
		}

		/// <summary>
		/// Stops the build.
		/// </summary>
		/// <param name="user">The user that triggered the stop.</param>
		/// <param name="build">The build to stop</param>
		public void StopBuild (UserBase user, IBuild build)
		{
			m_underlyingBuildsProvider.StopBuild (user, build);
		}

		/// <summary>
		/// Raises the builds refreshed event.
		/// </summary>
		/// <param name="args">Arguments.</param>
		protected virtual void OnBuildsRefreshed (EventArgs args)
		{
			if (BuildsRefreshed != null)
			{
				BuildsRefreshed (m_underlyingBuildsProvider, args);
			}
		}

		/// <summary>
		/// Raises the build updated event.
		/// </summary>
		/// <param name="args">Arguments.</param>
		protected virtual void OnBuildUpdated (BuildUpdatedEventArgs args)
		{
			if (BuildUpdated != null)
			{
				BuildUpdated (m_underlyingBuildsProvider, args);
			}
		}

		/// <summary>
		/// Filters the build.
		/// </summary>
		/// <returns><c>true</c>, if build was filtered, <c>false</c> otherwise.</returns>
		/// <param name="build">Build.</param>
		public bool Filter (IBuild build)
		{
			var f = m_serverService.GetState ().BuildFilter;
			var success = f.SuccessEnabled;
			var running = f.RunningEnabled;
			var failed = f.FailedEnabled;
			var queued = f.QueuedEnabled;

			var show =
				(success && build.IsSuccess)
				|| (running && build.IsRunning)
				|| (failed && build.IsFailed)
				|| (queued && build.IsQueued);

			if (!String.IsNullOrEmpty (f.KeyWord))
			{
				var text = build.ToString ().ToUpperInvariant ();

				show = show
				&& (text.Contains (f.KeyWord.ToUpperInvariant ()) ^ f.KeyWordType != KeyWordFilterType.Contains);
			}

			return show;
		}
		#endregion
	}
}
