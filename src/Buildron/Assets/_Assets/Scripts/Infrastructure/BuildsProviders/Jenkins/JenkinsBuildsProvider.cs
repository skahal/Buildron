#region Usings
using System;
using System.Xml;
using Buildron.Domain;
using Skahal.Logging;
#endregion

namespace Buildron.Infrastructure.BuildsProvider.Jenkins
{
	/// <summary>
	/// Jenkins builds provider.
	/// </summary>
	public class JenkinsBuildsProvider : BuildsProviderBase {
		
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="Buildron.Infrastructure.BuildsProvider.Jenkins.JenkinsBuildsProvider"/> class.
		/// </summary>
		/// <param name='server'>
		/// Server.
		/// </param>
		public JenkinsBuildsProvider (CIServer server) : base(server)
		{
			Name = "Jenkins";
			AuthenticationRequirement = AuthenticationRequirement.Optional;
			AuthenticationTip = "If your Jenkins server does not require authentication, leave username and password empty, otherwise, type a Jenkins's username and password.\n*BASIC HTTP authentication will be used.";
		}
		#endregion
		
		#region Methods
		public override void RefreshAllBuilds ()
		{
			// Get build configuration list.
			Get ("api", (doc) =>
			{
				var configs = doc.SelectNodes ("//job");
				
				foreach (XmlNode c in configs) {
					var bc = JenkinsBuildConfigurationParser.Parse (c);
					
					// Get each build configuration details.
					Get ("job/" + bc.Id + "/api", (bcDoc) => {
						if (bcDoc.SelectSingleNode ("//buildable").InnerText.Equals ("true", StringComparison.OrdinalIgnoreCase)) {
							JenkinsBuildConfigurationParser.ParsePartial (bc, bcDoc);
							
							// Get each build details.
							Get ("job/" + bc.Id + "/lastBuild/api", (bDoc) => {
								var number = bcDoc.SelectSingleNode ("//number").InnerText;
					
								GetText ("job/" + bc.Id + "/" + number + "/buildTimestamp?format=yyyy/MM/dd%20HH:mm:ss", buildTimestamp => {
									
									var build = JenkinsBuildParser.Parse (bc, bDoc, buildTimestamp);
									
									// Get user details.
									if (build.TriggeredBy == null || build.TriggeredBy.Kind == UserKind.ScheduledTrigger) {
										OnBuildUpdated (new BuildUpdatedEventArgs (build));
									} else {
										Get ("/user/" + build.TriggeredBy.UserName + "/api", (userDoc) => {
											build.TriggeredBy = JenkinsUserParser.ParseUserFromUserResponse (userDoc);	
											OnBuildUpdated (new BuildUpdatedEventArgs (build));
										});
									}
								});
							});
						}
					});
				}
				
				OnServerUp ();
				OnBuildsRefreshed ();
			});
		}
		
		public override void RunBuild (UserBase user, Build build)
		{
			var id = build.Configuration.Id;
			var url = GetHttpBasicAuthUrl (user, "job/{0}/build", id);
			
			Requester.GetTextImmediately (url, (r) => {
				SHLog.Debug ("JenkinsBuildsProvider: build {0} trigged. Reponse: {1}", id, r);
			});
		}

		public override void StopBuild (UserBase user, Build build)
		{
			var id = build.Configuration.Id;
			var url = GetHttpBasicAuthUrl (user, "job/{0}/lastBuild/stop", id);
			
			Requester.GetTextImmediately (url, (r) => {
				SHLog.Debug ("JenkinsBuildsProvider: build {0} stopped. Reponse: {1}", id, r);
			});
		}

		public override void AuthenticateUser (UserBase user)
		{
			var url = GetHttpBasicAuthUrl (user, "");
			
			Requester.GetTextImmediately (
			url, 
			(r) => 
			{
				OnUserAuthenticationSuccessful();
			},
			() => 
			{
				OnUserAuthenticationFailed();
			});
		}
		#endregion
		
		#region Helpers
		private void Get (string finalPart, Action<XmlDocument> responseReceived)
		{	
			var url = GetHttpBasicAuthUrl (Server, "{0}/xml", finalPart);
			
			if (url.Contains ("/view/") && finalPart.StartsWith ("/")) {
				url = url.Substring(0, url.IndexOf("/view/"));
				url += String.Format("{0}/xml", EscapeUrl(finalPart));
			}
			
			Requester.Get (url, responseReceived);	
		}
		
	
		private void GetText (string finalPart, Action<string> responseReceived)
		{
			var url = GetHttpBasicAuthUrl (Server, finalPart);
			Requester.GetText (url, responseReceived);	
		}
		
		#endregion
	}
}