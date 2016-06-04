using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buildron.Domain;
using Skahal.Logging;

namespace Buildron.Infrastructure.BuildsProvider
{
    /// <summary>
    /// An IBuildsProvider implementation to apply filter (from RC) to a underlying IBuildsProvider.
    /// </summary>
    public class FilterBuildsProvider : IBuildsProvider
    {
        #region Fields
        private IBuildsProvider m_underlyingBuildsProvider;
		private Dictionary<string, Build> m_buildsCache = new Dictionary<string, Build> ();
        #endregion

        #region Events
        public event EventHandler BuildsRefreshed;
        public event EventHandler<BuildUpdatedEventArgs> BuildUpdated;
        public event EventHandler ServerDown;
        public event EventHandler ServerUp;
        public event EventHandler UserAuthenticationFailed;
        public event EventHandler UserAuthenticationSuccessful;
        #endregion

        #region Constructors
        public FilterBuildsProvider(IBuildsProvider underlyingBuildsProvider)
        {
            if(underlyingBuildsProvider == null)
            {
                throw new ArgumentNullException("underlyingBuildsProvider");
            }

            m_underlyingBuildsProvider = underlyingBuildsProvider;
			m_underlyingBuildsProvider.BuildsRefreshed += (sender, e) => {
				OnBuildsRefreshed (e);
			};

            m_underlyingBuildsProvider.BuildUpdated += (sender, e) =>
            {
				var build = e.Build;
				m_buildsCache[build.Id] = build;

				OnBuildUpdated(e);
            };

            m_underlyingBuildsProvider.ServerDown += (sender, e) =>
            {
                if (ServerDown != null)
                {
                    ServerDown(sender, e);
                }
            };

            m_underlyingBuildsProvider.ServerUp += (sender, e) =>
            {
                if (ServerUp != null)
                {
                    ServerUp(sender, e);
                }
            };

            m_underlyingBuildsProvider.UserAuthenticationFailed += (sender, e) =>
            {
                if (UserAuthenticationFailed != null)
                {
                    UserAuthenticationFailed(sender, e);
                }
            };

            m_underlyingBuildsProvider.UserAuthenticationSuccessful += (sender, e) =>
            {
                if (UserAuthenticationSuccessful != null)
                {
                    UserAuthenticationSuccessful(sender, e);
                }
            };

			ServerService.Initialized += (sender, e) => {
	            ServerService.Listener.BuildFilterUpdated += (sender2, e2) =>
	            {
					var filteredBuilds = m_buildsCache.Values.Where(b => Filter(b));
					SHLog.Debug(
						"Filter updated. There is {0} cached builds and {1} was filtered", 
						m_buildsCache.Count, 
						filteredBuilds.Count());

					foreach (var build in filteredBuilds)
					{
						OnBuildUpdated(new BuildUpdatedEventArgs(build));
					}

					OnBuildsRefreshed(EventArgs.Empty);
	            };
			};
        }

        #endregion

        #region Properties
        public AuthenticationRequirement AuthenticationRequirement
        {
            get
            {
                return m_underlyingBuildsProvider.AuthenticationRequirement;
            }
        }

        public string AuthenticationTip
        {
            get
            {
                return m_underlyingBuildsProvider.AuthenticationTip;
            }
        }

        public string Name
        {
            get
            {
                return m_underlyingBuildsProvider.Name;
            }
        }
        #endregion

        #region Methods
        public void AuthenticateUser(User user)
        {
            m_underlyingBuildsProvider.AuthenticateUser(user);
        }

        public void RefreshAllBuilds()
        {
            m_underlyingBuildsProvider.RefreshAllBuilds();
        }

        public void RunBuild(User user, Build build)
        {
            m_underlyingBuildsProvider.RunBuild(user, build);
        }

        public void StopBuild(User user, Build build)
        {
            m_underlyingBuildsProvider.StopBuild(user, build);
        }

		protected virtual void OnBuildsRefreshed(EventArgs args)
		{
			if(BuildsRefreshed != null)
			{
				BuildsRefreshed(m_underlyingBuildsProvider, args);
			}
		}

		protected virtual void OnBuildUpdated(BuildUpdatedEventArgs args)
		{
			if (BuildUpdated != null)
			{
				BuildUpdated(m_underlyingBuildsProvider, args);
			}
		}

		private bool Filter (Build build)
		{
			var f = ServerState.Instance.BuildFilter;
			var success = f.SuccessEnabled;
			var running = f.RunningEnabled;
			var failed = f.FailedEnabled;
			var queued = f.QueuedEnabled;

			var show = 
				(success && build.IsSuccess)
				|| (running && build.IsRunning)
				|| (failed && build.IsFailed)
				|| (queued && build.IsQueued);

			if (!String.IsNullOrEmpty (f.KeyWord)) {
				var text = build.ToString ().ToUpperInvariant ();

				show = show
				&& (text.Contains (f.KeyWord.ToUpperInvariant ()) ^ f.KeyWordType != KeyWordFilterType.Contains);
			}

			return show;	
		}
        #endregion
    }
}
