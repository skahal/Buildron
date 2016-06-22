#region Usings
using System;
using System.Collections.Generic;
using System.Xml;
using Buildron.Domain;
using Skahal.Logging;
#endregion

namespace Buildron.Infrastructure.BuildsProvider.TeamCity
{
	/// <summary>
	/// TeamCity builds provider.
	/// </summary>
	public class TeamCityBuildsProvider : BuildsProviderBase
	{
        #region Fields
        private int m_currentBuildsFoundCount;
        private List<string> m_currentUpdatedBuildIds = new List<string>();
        #endregion

        #region Constructors
        public TeamCityBuildsProvider (CIServer server) : base(server)
		{
			Name = "TeamCity";
			AuthenticationRequirement = AuthenticationRequirement.Always;
			AuthenticationTip =  "type a TeamCity's ADMIN username and password.\n*BASIC HTTP authentication will be used.";		
		}
		#endregion
			
		#region IBuildsProvider implementation
		public override void RefreshAllBuilds ()
		{	
			UserParser.Reset ();
            m_currentUpdatedBuildIds.Clear();

            Requester.GetText (GetNoRestUrl ("queue.html"), (html) => 
			{		
				var queuedBuildConfigurationsId = BuildQueueParser.ParseBuildConfigurationsIdsFromQueueHtml (html);
				
				// Gets all build configurations.
				GetAllBuildConfigurations ((config) =>
				{
					// Gets the build.
					Get ((buildResponse) => 
					{
						var build = BuildParser.Parse (config, buildResponse);
						
						if (!build.IsRunning && queuedBuildConfigurationsId.Contains (build.Configuration.Id)) {
							build.Status = BuildStatus.Queued;
						}
						
						Action raiseBuildUpdated = delegate {
							OnBuildUpdated(new BuildUpdatedEventArgs (build));

                            if(!m_currentUpdatedBuildIds.Contains(build.Id))
                            {
                                m_currentUpdatedBuildIds.Add(build.Id);
                            }
                            

                            if (m_currentBuildsFoundCount == m_currentUpdatedBuildIds.Count)
                            {
                                OnBuildsRefreshed();
                            }
                        };
						
						build.LastChangeDescription = string.Empty;
						
						if (build.TriggeredBy == null) {
							Get ((changesResponse) => 
							{
								var changeNode = changesResponse.SelectSingleNode ("changes/change");
								
								if (changeNode == null) {
									raiseBuildUpdated ();
								} else {
									Get ((changeResponse) => 
									{
										changeNode = changeResponse.SelectSingleNode ("change");
										build.LastChangeDescription = changeNode.FirstChild.InnerText;

										// Try to get the username from user node.
										var usernameNode = changeNode.SelectSingleNode("user");

										// If there is no user node, then try to get from change node.
										if(usernameNode == null)
										{
											SHLog.Debug("user node not found");
											usernameNode = changeNode;
										}

										GetUser (UserParser.ParseUserName (usernameNode.Attributes ["username"].Value), build, raiseBuildUpdated);
										
									}, raiseBuildUpdated, "changes/id:{0}", changeNode.Attributes ["id"].Value);
								}
								
							}, raiseBuildUpdated, "changes?build=id:{0}", build.Id);
							
						} else {
							GetUser (build.TriggeredBy.UserName, build, raiseBuildUpdated);
						}
						
					}, "buildTypes/id:{0}/builds/running:any,canceled:false", config.Id);
				});
			});
		}
		
		private void GetUser (string userName, Build build, Action raiseBuildUpdated)
		{
			if (UserParser.IsHuman (userName)) {
				Get ((userResponse) => 
				{
					build.TriggeredBy = UserParser.ParseFromUser (build, userResponse);
					raiseBuildUpdated ();
				}, raiseBuildUpdated, "users/username:{0}", userName);	
			} else {
				raiseBuildUpdated();
			}
		}
		
		public override void RunBuild (UserBase user, Build build)
		{
			var url = GetHttpBasicAuthUrl (user, "action.html?add2Queue={0}", build.Configuration.Id);
			SHLog.Debug ("RunBuild URL: {0}", url);
			Requester.RequestImmediately (url);
		}
		
		public override void StopBuild (UserBase user, Build build)
		{
			var url = GetHttpBasicAuthUrl (user, "httpAuth/ajax.html?submit=Stop&buildId={0}&kill", build.Id);
			SHLog.Debug ("StopBuild URL: {0}", url);
			Requester.RequestImmediately (url);
		}
		
		public override void AuthenticateUser (UserBase user)
		{
			var url = GetHttpBasicAuthUrl (user, "httpAuth/app/rest/users");
			
			Requester.GetImmediately (
			url, 
			(doc) => 
			{
				OnUserAuthenticationSuccessful ();
			},
			() => 
			{
				OnUserAuthenticationFailed();
			});
		}
		#endregion
		
		#region Helper
		private void GetAllBuildConfigurations (Action<BuildConfiguration> configReceived)
		{
			Get ((r) => 
			{
				var configs = r.SelectNodes ("buildTypes/buildType");
                m_currentBuildsFoundCount = configs.Count;

                foreach (XmlNode c in configs) {
					Get ((bc) => 
					{
						configReceived (BuildConfigurationParser.Parse (bc));

					}, "buildTypes/id:{0}", c.Attributes ["id"].Value);
				}
				
				OnServerUp ();				
			}, "buildTypes");
		}
		
		private string GetNoRestUrl (string urlEndPart, params object[] args)
		{
			return GetHttpBasicAuthUrl(Server, urlEndPart, args);
		}
		
		private string GetUrl (string urlEndPart, params object[] args)
		{
			var endPart = string.Format (urlEndPart, args);
			return GetHttpBasicAuthUrl (Server, string.Format ("httpAuth/app/rest/{0}", endPart));
		}
		
		private void Get (Action<XmlDocument> responseReceived, string urlEndPart, params object[] args)
		{
			Requester.Get (GetUrl (urlEndPart, args), (xml) =>
			{
				OnServerUp();
				responseReceived (xml);
			});
		}
		
		private void Get (Action<XmlDocument> responseReceived, Action errorReceived, string urlEndPart, params object[] args)
		{
			Requester.Get (GetUrl (urlEndPart, args), (xml) =>
			{
				OnServerUp ();
				responseReceived (xml);
			}, errorReceived);
		}
		#endregion
	}
}