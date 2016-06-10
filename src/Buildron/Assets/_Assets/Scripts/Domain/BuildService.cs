#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using Buidron.Domain;
using Buildron.Domain;
using Buildron.Domain.Sorting;
using Skahal.Common;
using Skahal.Logging;
#endregion

namespace Buildron.Domain
{
	/// <summary>
	/// The builds service.
	/// </summary>
	public static class BuildService
	{
		#region Constants
		public const int MaxServerDownFromProvider = 1;
		#endregion
		
		#region Events
		/// <summary>
		/// Occurs when a build is found.
		/// </summary>
		public static event EventHandler<BuildFoundEventArgs> BuildFound;

		/// <summary>
		/// Occurs when a build is removed.
		/// </summary>
		public static event EventHandler<BuildRemovedEventArgs> BuildRemoved;
		
		/// <summary>
		/// Occurs when builds are refreshed.
		/// </summary>
		public static event EventHandler<BuildsRefreshedEventArgs> BuildsRefreshed;
		
		/// <summary>
		/// Occurs when a build is updated.
		/// </summary>
		public static event EventHandler<BuildUpdatedEventArgs> BuildUpdated;
		
		public static event EventHandler ServerUp;
		public static event EventHandler ServerDown;
		
		public static event EventHandler UserAuthenticationSuccessful;
		public static event EventHandler UserAuthenticationFailed;
		#endregion
		
		#region Fields
		private static IBuildsProvider s_buildsProvider;
		private static List<string> s_buildConfigurationIdsRefreshed;
		private static List<Build> s_builds;
        private static List<Build> s_buildsFoundInLastRefresh;
		private static int s_serverDownFromProviderCount;
		#endregion
		
		#region Properties
		public static int BuildsCount { 
			get {
				return s_builds.Count;
			}
		}
		
		public static bool Initialized { get; private set; }
		
		public static string ServerName {
			get {
				return s_buildsProvider.Name;	
			}
		}
		#endregion
		
		#region Methods
	    public static void Initialize (IBuildsProvider buildsProvider)
		{
			s_buildConfigurationIdsRefreshed = new List<string> ();
			s_builds = new List<Build> ();
            s_buildsFoundInLastRefresh = new List<Build>();
            s_buildsProvider = buildsProvider;
			
			s_buildsProvider.BuildUpdated += delegate(object sender, BuildUpdatedEventArgs e) {                
                var newBuild = e.Build;

                s_buildConfigurationIdsRefreshed.Add(newBuild.Configuration.Id);
				var oldBuild = s_builds.FirstOrDefault (bld => bld.Configuration.Id.Equals (newBuild.Configuration.Id));
			
				if (oldBuild == null) {
                    SHLog.Debug("BuildService.BuildUpdated: new build {0}", newBuild.Id);
                    s_builds.Add (newBuild);
                    s_buildsFoundInLastRefresh.Add(newBuild);
                    BuildFound.Raise (typeof(BuildService), new BuildFoundEventArgs (newBuild));
				} else {
                    SHLog.Debug("BuildService.BuildUpdated: old build {0}", newBuild.Id);
                    oldBuild.PercentageComplete = newBuild.PercentageComplete;
					
					if (oldBuild.TriggeredBy != null && !oldBuild.Configuration.Id.Equals (newBuild.Configuration.Id)) {
						oldBuild.TriggeredBy.Builds.Remove (oldBuild);
					}
					
					oldBuild.LastChangeDescription = newBuild.LastChangeDescription;
					oldBuild.Date = newBuild.Date;
					oldBuild.TriggeredBy = newBuild.TriggeredBy;
					oldBuild.LastRanStep = newBuild.LastRanStep;
					oldBuild.Status = newBuild.Status;
					oldBuild.Configuration = newBuild.Configuration;
				}
				
				BuildUpdated.Raise (typeof(BuildService), e);
			};
			
			s_buildsProvider.BuildsRefreshed += delegate {

                var removedBuilds = s_builds.Where(b => !s_buildConfigurationIdsRefreshed.Any(configId => b.Configuration.Id.Equals(configId))).ToList();

                SHLog.Warning("BuildService.BuildsRefreshed: there is {0} builds and {1} were refreshed. {2} will be removed", s_builds.Count, s_buildConfigurationIdsRefreshed.Count, removedBuilds.Count);

                foreach (var build in removedBuilds)
				{
                    s_builds.Remove(build);
					BuildRemoved.Raise(typeof(BuildService), new BuildRemovedEventArgs(build));
				}

                s_buildConfigurationIdsRefreshed.Clear();
                BuildsRefreshed.Raise (typeof(BuildService), new BuildsRefreshedEventArgs(s_buildsFoundInLastRefresh.ToList(), removedBuilds));
                s_buildsFoundInLastRefresh.Clear();

            };
			
			s_buildsProvider.ServerDown += delegate {
				s_serverDownFromProviderCount++;
				
				if (s_serverDownFromProviderCount >= MaxServerDownFromProvider) {
					ServerDown.Raise (typeof(BuildService));
				}	
			};
			
			s_buildsProvider.ServerUp += delegate {
				s_serverDownFromProviderCount = 0;
				ServerUp.Raise (typeof(BuildService));
			};
			
			s_buildsProvider.UserAuthenticationSuccessful += delegate {
				Initialized = true;
				UserAuthenticationSuccessful.Raise (typeof(BuildService));
				ServerUp.Raise (typeof(BuildService));
			};
			
			s_buildsProvider.UserAuthenticationFailed += delegate {
				UserAuthenticationFailed.Raise (typeof(BuildService));
			};
		}
		
		public static void RefreshAllBuilds ()
		{
			s_buildsProvider.RefreshAllBuilds ();
		}
		
		public static void RunBuild (RemoteControl remoteControl, string buildId)
		{
			ExecuteBuildCommand(remoteControl, buildId, s_buildsProvider.RunBuild);
		}
		
		public static void StopBuild (RemoteControl remoteControl, string buildId)
		{
			ExecuteBuildCommand (remoteControl, buildId, s_buildsProvider.StopBuild);
		}
		
		private static void ExecuteBuildCommand (RemoteControl remoteControl, string buildId, Action<RemoteControl, Build> command)
		{
			var build = s_builds.FirstOrDefault (b => b.Id.Equals (buildId, StringComparison.OrdinalIgnoreCase));
			
			if (build != null) {
				build.Status = BuildStatus.Queued;
				command (remoteControl, build);
			}
		}
		
		public static void AuthenticateUser (User user)
		{
			s_buildsProvider.AuthenticateUser(user);
		}
		
		public static Build GetMostRelevantBuildForUser (BuildUser user)
		{
			var userBuilds = s_builds.Where (b => b.TriggeredBy != null && b.TriggeredBy.UserName.Equals (user.UserName));
			var build = userBuilds.FirstOrDefault (b => b.IsRunning);
			
			if (build == null) {
				build = userBuilds.FirstOrDefault (b => b.IsFailed);
				
				if (build == null) {
					build = userBuilds.FirstOrDefault ();
				}
			}
			
			return build;
		}
		
		public static void Reset ()
		{
			s_builds.Clear ();
		}
		
		public static IComparer<Build> GetComparer(SortBy sortBy)
		{
			switch (sortBy) {
			case SortBy.Date:
				return new BuildDateDescendingComparer ();
				
			default:
				return new BuildTextComparer ();
			}
		}
		#endregion
	}
}