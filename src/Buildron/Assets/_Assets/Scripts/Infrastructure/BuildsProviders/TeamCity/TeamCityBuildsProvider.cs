using System;
using System.Collections.Generic;
using System.Xml;
using Skahal.Logging;
using Buildron.Domain.Builds;
using Buildron.Domain.Users;
using Buildron.Domain.CIServers;
using Buildron.Infrastructure.BuildsProviders;
using System.Net;

namespace Buildron.Infrastructure.BuildsProvider.TeamCity
{
	/// <summary>
	/// TeamCity builds provider.
	/// </summary>
	public class TeamCityBuildsProvider : BuildsProviderBase
	{
		#region Constructors

		public TeamCityBuildsProvider (ICIServer server) : base (server)
		{
			Name = "TeamCity";
			AuthenticationRequirement = AuthenticationRequirement.Always;
			AuthenticationTip = "type a TeamCity's ADMIN username and password.\n*BASIC HTTP authentication will be used.";		
		}

		#endregion

		#region IBuildsProvider implementation

		protected override void PerformRefreshAllBuilds ()
		{	
			UserParser.Reset ();            

			Requester.GetText (GetNoRestUrl ("queue.html"), (html) => {		
				var queuedBuildConfigurationsId = BuildQueueParser.ParseBuildConfigurationsIdsFromQueueHtml (html);
				
				// Gets all build configurations.
				GetAllBuildConfigurations ((config) => {
					// Gets the build.
					Get ((buildResponse) => {                        
						var build = BuildParser.Parse (config, buildResponse);
						
						if (!build.IsRunning () && queuedBuildConfigurationsId.Contains (build.Configuration.Id)) {
							build.Status = BuildStatus.Queued;
						}
						
						Action raiseBuildUpdated = delegate {
							OnBuildUpdated (new BuildUpdatedEventArgs (build));
						};
						
						build.LastChangeDescription = string.Empty;
						
						if (build.TriggeredBy == null) {
							Get ((changesResponse) => {
								var changeNode = changesResponse.SelectSingleNode ("changes/change");
								
								if (changeNode == null) {
									raiseBuildUpdated ();
								} else {
									Get ((changeResponse) => {
										changeNode = changeResponse.SelectSingleNode ("change");
										build.LastChangeDescription = changeNode.FirstChild.InnerText;

										// Try to get the username from user node.
										var usernameNode = changeNode.SelectSingleNode ("user");

										// If there is no user node, then try to get from change node.
										if (usernameNode == null) {
											SHLog.Debug ("user node not found");
											usernameNode = changeNode;
										}

										GetUser (UserParser.ParseUserName (usernameNode.Attributes ["username"].Value), build, raiseBuildUpdated);
										
									}, (e) => raiseBuildUpdated (), "changes/id:{0}", changeNode.Attributes ["id"].Value);
								}
								
							}, (e) => raiseBuildUpdated (), "changes?build=id:{0}", build.Id);
							
						} else {
							GetUser (build.TriggeredBy.UserName, build, raiseBuildUpdated);
						}
						
					},
						(e) => {
							if (e.StatusCode == HttpStatusCode.NotFound) {
								SHLog.Debug ("Looks like '{0}' has no build.", config.Id);
								CurrentBuildsFoundCount--;
							}
						}, "buildTypes/id:{0}/builds/running:any,canceled:false,branch:(default:any)".With (config.Id));
				});
			});
		}

		private void GetUser (string userName, Build build, Action raiseBuildUpdated)
		{
			if (UserParser.IsHuman (userName)) {
				Get ((userResponse) => {
					build.TriggeredBy = UserParser.ParseFromUser (build, userResponse);
					raiseBuildUpdated ();
				}, (e) => raiseBuildUpdated (), "users/username:{0}", userName);	
			} else {
				raiseBuildUpdated ();
			}
		}

		public override void RunBuild (IAuthUser user, IBuild build)
		{
			var url = GetHttpBasicAuthUrl (user, "action.html?add2Queue={0}", build.Configuration.Id);
			SHLog.Debug ("RunBuild URL: {0}", url);
			Requester.RequestImmediately (url);
		}

		public override void StopBuild (IAuthUser user, IBuild build)
		{
			var url = GetHttpBasicAuthUrl (user, "httpAuth/ajax.html?submit=Stop&buildId={0}&kill", build.Id);
			SHLog.Debug ("StopBuild URL: {0}", url);
			Requester.RequestImmediately (url);
		}

		public override void AuthenticateUser (IAuthUser user)
		{
			var url = GetHttpBasicAuthUrl (user, "httpAuth/app/rest/users");
			
			Requester.GetImmediately (
				url, 
				(doc) => {
					OnUserAuthenticationCompleted (new UserAuthenticationCompletedEventArgs (user, true));
				},
				(e) => {
					OnUserAuthenticationCompleted (new UserAuthenticationCompletedEventArgs (user, false));
				});
		}

		#endregion

		#region Helper

		private void GetAllBuildConfigurations (Action<BuildConfiguration> configReceived)
		{
			Get ((r) => {
				var configs = r.SelectNodes ("buildTypes/buildType");
				CurrentBuildsFoundCount = configs.Count;

				foreach (XmlNode c in configs) {
					Get ((bc) => {
						configReceived (BuildConfigurationParser.Parse (bc));

					}, "buildTypes/id:{0}", c.Attributes ["id"].Value);
				}
				
				OnServerUp ();				
			}, "buildTypes");
		}

		private string GetNoRestUrl (string urlEndPart, params object[] args)
		{
			return GetHttpBasicAuthUrl (Server, urlEndPart, args);
		}

		private string GetUrl (string urlEndPart, params object[] args)
		{
			var endPart = string.Format (urlEndPart, args);
			return GetHttpBasicAuthUrl (Server, string.Format ("httpAuth/app/rest/{0}", endPart));
		}

		private void Get (Action<XmlDocument> responseReceived, string urlEndPart, params object[] args)
		{
			Requester.Get (GetUrl (urlEndPart, args), (xml) => {
				OnServerUp ();
				responseReceived (xml);
			});
		}

		private void Get (Action<XmlDocument> responseReceived, Action<RequestError> errorReceived, string urlEndPart, params object[] args)
		{
			Requester.Get (GetUrl (urlEndPart, args), (xml) => {
				OnServerUp ();
				responseReceived (xml);
			}, errorReceived);
		}

		#endregion
	}
}