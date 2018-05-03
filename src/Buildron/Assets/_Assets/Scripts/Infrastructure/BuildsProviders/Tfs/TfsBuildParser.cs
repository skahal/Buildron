using System.Xml;
using System;
using System.Globalization;
using Buildron.Domain.Builds;
using System.Xml.Linq;
using Buildron.Infrastructure.BuildsProvider.Tfs.Models;
using Buildron.Domain.Users;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Buildron.Infrastructure.BuildsProvider.Tfs
{
	/// <summary>
	/// A parser for Build.
	/// </summary>
	public static class TfsBuildParser
	{
        private static TupleList<BuildStepType, Regex> BuildSteps = new TupleList<BuildStepType, Regex>
        {
            { BuildStepType.UnitTest, new Regex("test", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
            { BuildStepType.Deploy, new Regex("deploy", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
            { BuildStepType.Compilation, new Regex("build", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
            { BuildStepType.PackagePublishing, new Regex("package|publish", RegexOptions.Compiled | RegexOptions.IgnoreCase) },
        };

        public static BuildConfiguration Parse(TfsDefinition tfsDefinition)
        {
            // Project
            var project = new BuildProject
            {
                Name = string.Empty
            };

            if (tfsDefinition.project != null)
            {
                project.Name = tfsDefinition.project.name;
            };

            // Build steps
            var buildSteps = new List<IBuildStep>();
            if (tfsDefinition.build != null)
            {
                buildSteps.AddRange(tfsDefinition.build.Select(x => ParseBuildStep(x)));
            }

            // Build configuration
            return new BuildConfiguration
            {
                Id = tfsDefinition.id.ToString(),
                Name = tfsDefinition.name,
                Project = project,
                Steps = buildSteps
            };
        }

        public static Build Parse (BuildConfiguration config, TfsBuild tfsBuild, TfsCommit tfsCommit)
		{
            IUser user = null;
            //if (tfsBuild.requestedBy != null)
            //{
            //    user = Parse(tfsBuild.requestedBy);
            //}

            if (tfsCommit != null)
            {
                if (tfsCommit.committer != null)
                {
                    user = Parse(tfsCommit.committer);
                }
                else if (tfsCommit.author != null)
                {
                    user = Parse(tfsCommit.author);
                }
            }

            return new Build
            {
                Configuration = config,
                Id = tfsBuild.id.ToString(),
                Sequence = tfsBuild.id,
                TriggeredBy = user,
                Status = ParseBuildStatus(tfsBuild),
                Date = tfsBuild.FinishTime,
                PercentageComplete = 1.0f,
                LastChangeDescription = tfsCommit.comment
            };
		}

        public static User Parse(TfsGitUser tfsGitUser)
        {
            return new User
            {
                Name = tfsGitUser.name,
                Email = tfsGitUser.email,
                UserName = tfsGitUser.email
            };
        }

        //public static User Parse(TfsUser tfsUser)
        //{
        //    return new User
        //    {
        //        Name = tfsUser.displayName
        //    };
        //}

        private static BuildStatus ParseBuildStatus(TfsBuild tfsBuild)
        {
            switch (tfsBuild.Status)
            {
                case TfsBuildStatus.completed:
                    switch (tfsBuild.Result)
                    {
                        case TfsBuildResult.canceled:
                            return BuildStatus.Canceled;

                        case TfsBuildResult.succeeded:
                            return BuildStatus.Success;

                        case TfsBuildResult.failed:
                            return BuildStatus.Failed;

                        case TfsBuildResult.partiallySucceeded:
                            return BuildStatus.Error;
                    }
                    break;

                case TfsBuildStatus.inProgress:
                    return BuildStatus.Running;

                case TfsBuildStatus.notStarted:
                case TfsBuildStatus.postponed:
                    return BuildStatus.Queued;

                case TfsBuildStatus.cancelling:
                    return BuildStatus.Canceled;
            }

            return BuildStatus.Unknown;
        }

        private static IBuildStep ParseBuildStep(TfsBuildStep tfsBuildStep)
        {
            var buildStep = new BuildStep
            {
                Name = tfsBuildStep.displayName,
            };

            foreach (var buildStepParser in BuildSteps)
            {
                if (buildStepParser.Value.IsMatch(tfsBuildStep.displayName))
                {
                    buildStep.StepType = buildStepParser.Key;
                    break;
                }
            }

            return buildStep;
        }

        private class TupleList<T1, T2> : List<KeyValuePair<T1, T2>>
        {
            public void Add(T1 item, T2 item2)
            {
                Add(new KeyValuePair<T1, T2>(item, item2));
            }
        }
    }
}