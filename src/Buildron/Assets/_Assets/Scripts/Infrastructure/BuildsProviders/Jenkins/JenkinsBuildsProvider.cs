using System;
using System.Xml;
using Skahal.Logging;
using Buildron.Domain.Builds;
using Buildron.Domain.Users;
using Buildron.Domain.CIServers;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Buildron.Infrastructure.BuildsProvider.Jenkins
{
    /// <summary>
    /// Jenkins builds provider.
    /// </summary>
    public class JenkinsBuildsProvider : BuildsProviderBase
    {
		#region Fields
		private static readonly Regex s_getJobPartialUrlRegex = new Regex ("job/.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		#endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Buildron.Infrastructure.BuildsProvider.Jenkins.JenkinsBuildsProvider"/> class.
        /// </summary>
        /// <param name='server'>
        /// Server.
        /// </param>
        public JenkinsBuildsProvider(ICIServer server) : base(server)
        {
            Name = "Jenkins";
            AuthenticationRequirement = AuthenticationRequirement.Optional;
            AuthenticationTip = "If your Jenkins server does not require authentication, leave username and password empty, otherwise, type a Jenkins's username and password.\n*BASIC HTTP authentication will be used.";
            TreeDepth = 5;
        }
        #endregion

        #region Properties        
        /// <summary>
        /// Gets or sets the tree depth to look for builds (?tree=).
        /// </summary>
        public int TreeDepth { get; set; }
        #endregion

        #region Methods       
        public static string BuildTreeFilter(string filter, int depth)
        {
            var treeFilter = string.Join(",", Enumerable.Repeat(filter, depth).ToArray());
            return "{0}{1}".With(treeFilter, "".PadRight(depth, ']'));
        }

        protected override void PerformRefreshAllBuilds()
        {
            // Get build configuration list.
            Get("api", BuildTreeFilter("jobs[name,displayName,buildable,lastBuild[number],url", TreeDepth), (doc) =>
            {
                var configs = JenkinsBuildConfigurationParser.Parse(doc);
                CurrentBuildsFoundCount = configs.Count;

                foreach(var c in configs)
                {
                    GetBuild(c.Key, c.Value);
                }
            });
        }

        private string GetJobPartialUrl(XmlNode jobElement)
        {
			return s_getJobPartialUrlRegex.Match (jobElement ["url"].InnerText).Value;
        }

        private void GetBuild(BuildConfiguration bc, XmlNode bcNode)
        {
            CurrentBuildsFoundCount++;

            var jobPartialUrl = GetJobPartialUrl(bcNode);
            var lastBuild = bcNode["lastBuild"];            

            if(lastBuild == null)
            {
                return;
            }

            var number = lastBuild["number"].InnerText;

            Get(jobPartialUrl + "/lastBuild/api", "", (bDoc) =>
            {                
                GetText(GetJobPartialUrl(bcNode) + number + "/buildTimestamp?format=yyyy/MM/dd%20HH:mm:ss", buildTimestamp =>
                {
                    var build = JenkinsBuildParser.Parse(bc, bDoc, buildTimestamp);

                    // Get user details.
                    if (build.TriggeredBy == null || build.TriggeredBy.Kind == UserKind.ScheduledTrigger)
                    {
                        OnBuildUpdated(new BuildUpdatedEventArgs(build));
                    }
                    else
                    {
                        Get("/user/" + build.TriggeredBy.UserName + "/api", "", (userDoc) =>
                        {
                            build.TriggeredBy = JenkinsUserParser.ParseUserFromUserResponse(userDoc);
                            OnBuildUpdated(new BuildUpdatedEventArgs(build));
                        });
                    }
                });
            });
        }

		public override void RunBuild(IBasicUser user, IBuild build)
        {
            var id = build.Configuration.Id;
            var url = GetHttpBasicAuthUrl(user, "job/{0}/build", id);

            Requester.GetTextImmediately(url, (r) =>
           {
               SHLog.Debug("JenkinsBuildsProvider: build {0} trigged. Reponse: {1}", id, r);
           });
        }

		public override void StopBuild(IBasicUser user, IBuild build)
        {
            var id = build.Configuration.Id;
            var url = GetHttpBasicAuthUrl(user, "job/{0}/lastBuild/stop", id);

            Requester.GetTextImmediately(url, (r) =>
           {
               SHLog.Debug("JenkinsBuildsProvider: build {0} stopped. Reponse: {1}", id, r);
           });
        }

		public override void AuthenticateUser(IBasicUser user)
        {
            var url = GetHttpBasicAuthUrl(user, "");

            Requester.GetTextImmediately(
                url,
                (r) =>
                {
                    OnUserAuthenticationSuccessful();
                },
                (e) =>
                {
                    OnUserAuthenticationFailed();
                });
        }
        #endregion

        #region Helpers
        private void Get(string finalPart, string treeFilter, Action<XmlDocument> responseReceived)
        {
            string treeParameter = string.Empty;

            if(!string.IsNullOrEmpty(treeFilter))
            {
                treeParameter = "?tree={0}".With(treeFilter);
            }

            var url = GetHttpBasicAuthUrl(Server, "{0}/xml{1}", finalPart, treeParameter);

            if (url.Contains("/view/") && finalPart.StartsWith("/"))
            {
                url = url.Substring(0, url.IndexOf("/view/"));
                url += String.Format("{0}/xml{1}", EscapeUrl(finalPart), treeParameter);
            }

            Requester.Get(url, responseReceived);
        }


        private void GetText(string finalPart, Action<string> responseReceived)
        {
            var url = GetHttpBasicAuthUrl(Server, finalPart);
            Requester.GetText(url, responseReceived);
        }

        #endregion
    }
}