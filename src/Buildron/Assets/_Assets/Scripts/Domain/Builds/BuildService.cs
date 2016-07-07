using System;
using System.Collections.Generic;
using System.Linq;
using Buildron.Domain;
using Skahal.Common;
using Skahal.Logging;
using Buildron.Domain.Users;
using Buildron.Domain.CIServers;
using Buildron.Domain.RemoteControls;

namespace Buildron.Domain.Builds
{
	/// <summary>
	/// The builds service.
	/// </summary>
	public class BuildService : IBuildService
	{
		#region Events
		/// <summary>
		/// Occurs when a build is found.
		/// </summary>
		public event EventHandler<BuildFoundEventArgs> BuildFound;

		/// <summary>
		/// Occurs when a build is removed.
		/// </summary>
		public event EventHandler<BuildRemovedEventArgs> BuildRemoved;
		
		/// <summary>
		/// Occurs when builds are refreshed.
		/// </summary>
		public event EventHandler<BuildsRefreshedEventArgs> BuildsRefreshed;
		
		/// <summary>
		/// Occurs when a build is updated.
		/// </summary>
		public event EventHandler<BuildUpdatedEventArgs> BuildUpdated;			
		#endregion

		#region Fields
		private IBuildsProvider m_buildsProvider;
		private ISHLogStrategy m_log;
		private List<string> m_buildConfigurationIdsRefreshed;		
		private List<IBuild> m_buildsFoundInLastRefresh;		
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Builds.BuildService"/> class.
		/// </summary>
		/// <param name="log">Log.</param>
		public BuildService (ISHLogStrategy log)
		{
			m_log = log;
		}
        #endregion

        #region Properties		        
        /// <summary>
        /// Gets the builds.
        /// </summary>
        public IList<IBuild> Builds { get; private set; }

        /// <summary>
        /// Gets the name of the server.
        /// </summary>
        /// <value>The name of the server.</value>
        public string ServerName
		{
			get
			{
				return m_buildsProvider.Name;	
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Initialize the build service.
		/// </summary>
		/// <param name="buildsProvider">Builds provider.</param>
		public void Initialize (IBuildsProvider buildsProvider)
		{
			m_buildConfigurationIdsRefreshed = new List<string> ();
			Builds = new List<IBuild> ();
			m_buildsFoundInLastRefresh = new List<IBuild> ();
			m_buildsProvider = buildsProvider;
			
			m_buildsProvider.BuildUpdated += delegate(object sender, BuildUpdatedEventArgs e)
			{                
				var newBuild = e.Build;

				m_buildConfigurationIdsRefreshed.Add (newBuild.Configuration.Id);
				var oldBuild = Builds.FirstOrDefault (bld => bld.Configuration.Id.Equals (newBuild.Configuration.Id));
			
				if (oldBuild == null)
				{
					m_log.Debug ("BuildService.BuildUpdated: new build {0}", newBuild.Id);
					Builds.Add (newBuild);
					m_buildsFoundInLastRefresh.Add (newBuild);
					BuildFound.Raise (this, new BuildFoundEventArgs (newBuild));
				} else
				{
					m_log.Debug ("BuildService.BuildUpdated: old build {0}", newBuild.Id);
					oldBuild.PercentageComplete = newBuild.PercentageComplete;
					
					if (oldBuild.TriggeredBy != null && !oldBuild.Configuration.Id.Equals (newBuild.Configuration.Id))
					{
						oldBuild.TriggeredBy.Builds.Remove (oldBuild);
					}
					
					oldBuild.LastChangeDescription = newBuild.LastChangeDescription;
					oldBuild.Date = newBuild.Date;
					oldBuild.TriggeredBy = newBuild.TriggeredBy;
					oldBuild.LastRanStep = newBuild.LastRanStep;
					oldBuild.Status = newBuild.Status;
					oldBuild.Configuration = newBuild.Configuration;
				}
				
				BuildUpdated.Raise (this, e);
			};
			
			m_buildsProvider.BuildsRefreshed += delegate
			{

				var removedBuilds = Builds.Where (b => !m_buildConfigurationIdsRefreshed.Any (configId => b.Configuration.Id.Equals (configId))).ToList ();

				m_log.Warning ("BuildService.BuildsRefreshed: there is {0} builds and {1} were refreshed. {2} will be removed", Builds.Count, m_buildConfigurationIdsRefreshed.Count, removedBuilds.Count);

				foreach (var build in removedBuilds)
				{
					Builds.Remove (build);
					BuildRemoved.Raise (this, new BuildRemovedEventArgs (build));
				}

				var buildsStatusChanged = Builds.Where (b => b.PreviousStatus != BuildStatus.Unknown && b.PreviousStatus != b.Status).ToList ();

				m_buildConfigurationIdsRefreshed.Clear ();
				BuildsRefreshed.Raise (this, new BuildsRefreshedEventArgs (buildsStatusChanged, m_buildsFoundInLastRefresh.ToList (), removedBuilds));
				m_buildsFoundInLastRefresh.Clear ();
			};				
		}

		/// <summary>
		/// Refreshs all builds.
		/// </summary>
		public void RefreshAllBuilds ()
		{
			m_buildsProvider.RefreshAllBuilds ();
		}

		/// <summary>
		/// Runs the build.
		/// </summary>
		/// <param name="remoteControl">Remote control.</param>
		/// <param name="buildId">Build identifier.</param>
		public void RunBuild (RemoteControl remoteControl, string buildId)
		{
			ExecuteBuildCommand (remoteControl, buildId, m_buildsProvider.RunBuild);
		}

		/// <summary>
		/// Stops the build.
		/// </summary>
		/// <param name="remoteControl">Remote control.</param>
		/// <param name="buildId">Build identifier.</param>
		public void StopBuild (RemoteControl remoteControl, string buildId)
		{
			ExecuteBuildCommand (remoteControl, buildId, m_buildsProvider.StopBuild);
		}

		/// <summary>
		/// Gets the most relevant build for user.
		/// </summary>
		/// <returns>The most relevant build for user.</returns>
		/// <param name="user">User.</param>
		public IBuild GetMostRelevantBuildForUser (User user)
		{
            var comparer = new BuildMostRelevantStatusComparer();
            var userBuilds = Builds
                .Where(b => b.TriggeredBy != null && b.TriggeredBy == user)
                .OrderBy(b => b, comparer);

            return userBuilds.FirstOrDefault();
		}		

		private void ExecuteBuildCommand (RemoteControl remoteControl, string buildId, Action<RemoteControl, IBuild> command)
		{
			var build = Builds.FirstOrDefault (b => b.Id.Equals (buildId, StringComparison.OrdinalIgnoreCase));

			if (build == null)
            {
                m_log.Warning("No build with id '{0}' could be found to execute the command.", buildId);
            }
            else
			{
				build.Status = BuildStatus.Queued;
				command (remoteControl, build);
			}
		}
		#endregion
	}
}