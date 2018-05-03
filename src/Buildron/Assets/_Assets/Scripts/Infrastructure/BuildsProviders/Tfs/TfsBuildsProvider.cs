using System;
using System.Collections.Generic;
using System.Xml;
using Skahal.Logging;
using Buildron.Domain.Builds;
using Buildron.Domain.Users;
using Buildron.Domain.CIServers;
using Buildron.Infrastructure.BuildsProviders;
using System.Net;
using Buildron.Infrastructure.BuildsProvider.Tfs.Models;
using System.Collections;
using UnityEngine;
using System.Linq;

namespace Buildron.Infrastructure.BuildsProvider.Tfs
{
    /// <summary>
    /// Tfs builds provider.
    /// </summary>
    public class TfsBuildsProvider : BuildsProviderBase
    {
        #region Private fields

        private List<BuildConfiguration> buildConfigurations = new List<BuildConfiguration>();

        private string userName;

        private string password;

        #endregion

        #region Constructors

        public TfsBuildsProvider(ICIServer server) : base(server)
        {
            Name = "Tfs";
            AuthenticationRequirement = AuthenticationRequirement.Always;
            AuthenticationTip = "type a TFS ADMIN username and password.\n*BASIC HTTP authentication will be used.\nUsing API tokens? Leave user name empty and add token to password field.";
        }

        #endregion

        #region IBuildsProvider implementation

        protected override void PerformRefreshAllBuilds()
        {
            GetBuildDefinitions();
        }

        private void GetBuildDefinitions()
        {
            GetModel<TfsDefinitions>("build/Definitions", tfsDefinitions =>
            {
                CurrentBuildsFoundCount = tfsDefinitions.count;

                GetBuildDefinitions(tfsDefinitions);
            });
        }

        private void GetBuildDefinitions(TfsDefinitions tfsDefinitions)
        {
            foreach (var tfsDefinitionSummary in tfsDefinitions.value)
            {
                GetModel<TfsDefinition>("build/Definitions/" + tfsDefinitionSummary.id, tfsDefinition =>
                {
                    GetBuilds(TfsBuildParser.Parse(tfsDefinition));
                });
            }
        }

        private void GetBuilds(BuildConfiguration buildConfiguration)
        {
            GetModel<TfsBuilds>(string.Format("build/Builds?Definitions={0}", buildConfiguration.Id), tfsBuilds =>
            {
                // Only deal with Git repos
                foreach (var tfsBuild in tfsBuilds.value.Where(b => b.ContainsGitCommits()))
                {
                    GetBuild(buildConfiguration, tfsBuild);
                }
            });
        }

        private void GetBuild(BuildConfiguration buildConfiguration, TfsBuild tfsBuild)
        {
            GetModel<TfsCommit>(string.Format("git/repositories/{0}/commits/{1:n}", tfsBuild.repository.id, tfsBuild.sourceVersion), tfsCommit =>
            {
                var build = TfsBuildParser.Parse(buildConfiguration, tfsBuild, tfsCommit);

                OnBuildUpdated(new BuildUpdatedEventArgs(build));
            });
        }

        public override void RunBuild(IAuthUser user, IBuild build)
        {
        }

        public override void StopBuild(IAuthUser user, IBuild build)
        {
        }

        public override void AuthenticateUser(IAuthUser user)
        {
            this.userName = user.DomainAndUserName;
            this.password = user.Password;

            this.OnUserAuthenticationCompleted(new UserAuthenticationCompletedEventArgs(user, true));
        }

        #endregion

        #region Helper

        private string GetUrl(string urlEndPart, params object[] args)
        {
            var endPart = string.Format(urlEndPart, args);
            return GetHttpBasicAuthUrl(Server, string.Format("_apis/{0}", endPart));
        }

        private void GetModel<TModel>(string urlEndPart, Action<TModel> responseReceived)
        {
            Requester.GetText(GetUrl(urlEndPart), this.userName, this.password, (jsonResult) => {
                OnServerUp();
                responseReceived(JsonUtility.FromJson<TModel>(jsonResult));
            });
        }

        #endregion
    }
}